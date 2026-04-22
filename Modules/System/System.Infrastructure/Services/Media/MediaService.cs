using Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using System.Infrastructure.Db;
using System.Text.RegularExpressions;

namespace System.Infrastructure.Services.Media
{
    public class MediaService : IMediaProvider
    {
        private readonly SystemContext _context;
        private static readonly Regex Base64PrefixRegex = new Regex("^data:[a-z0-9/]*;base64,", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private readonly IMemoryCache _memoryCache;

        public MediaService(SystemContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        public async Task Delete(Guid? id)
        {
            var media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return;
            }

            _memoryCache.Remove(media);
            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }

        public Task<Base.Media?> Load(Guid id, bool removeWebsiteExtras = false)
            => _memoryCache.GetOrCreateAsync($"MEDIA_{id}", async(entry) =>
            {
                var media = await _context.Set<Base.Media>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                if (media is null)
                {
                    return null;
                }

                _memoryCache.CreateEntry(media);

                if (removeWebsiteExtras && !string.IsNullOrEmpty(media.ContentStr))
                {
                    var ext = Regex.Match(media.ContentStr, "data:[a-z0-9\\/]*;base64,").Value;

                    return new Base.Media
                    {
                        Id = media.Id,
                        InsDate = DateTime.Now,
                        ContentStr = media.ContentStr.Replace(ext, string.Empty),
                        Extension = media.Extension,
                        Content = media.Content,
                        UpdDate = media.UpdDate
                    };
                }

                return media;
            });

        public async IAsyncEnumerable<Base.Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras)
        {
            foreach (var id in ids)
            {
                yield return await _memoryCache.GetOrCreateAsync($"MEDIA_{id}", async (entry) =>
                {
                    var media = await _context.Set<Base.Media>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

                    if (media is null)
                    {
                        return null;
                    }

                    _memoryCache.CreateEntry(media);

                    if (removeWebsiteExtras && !string.IsNullOrEmpty(media.ContentStr))
                    {
                        var ext = Regex.Match(media.ContentStr, "data:[a-z0-9\\/]*;base64,").Value;

                        return new Base.Media
                        {
                            Id = media.Id,
                            InsDate = DateTime.Now,
                            ContentStr = media.ContentStr.Replace(ext, string.Empty),
                            Extension = media.Extension,
                            Content = media.Content,
                            UpdDate = media.UpdDate
                        };
                    }

                    return media;
                });
            }
        }

        public async Task<Guid> Save(string content, Guid? id, string? extension = null)
        {
            Base.Media? media;
            var isNew = false;

            if (id is null)
            {
                media = new Base.Media { Id = Guid.NewGuid(), ContentStr = content, Extension = GetFileExtension(content) ?? "(unknown)" };

                await _context.Set<Base.Media>().AddAsync(media);
                await _context.SaveChangesAsync();
                var entry = _memoryCache.CreateEntry($"MEDIA_{id}");
                entry.SetValue(media);
                return media.Id;
            }
            else
            {
                media = await _memoryCache.GetOrCreateAsync($"MEDIA_{id}", async entry =>
                {
                    var media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id);
                    if (media is null)
                    {
                        media = new Base.Media { Id = Guid.NewGuid(), ContentStr = content, Extension = GetFileExtension(content) ?? "(unknown)" };
                        await _context.Set<Base.Media>().AddAsync(media);
                        await _context.SaveChangesAsync();
                        isNew = true;
                        return media;
                    }

                    return media;
                });

                ArgumentNullException.ThrowIfNull(media);

                if (!isNew)
                {
                    media.ContentStr = content;
                    media.Extension = extension ?? GetFileExtension(content) ?? "(unknown)";

                    var entry = _memoryCache.CreateEntry($"MEDIA_{id}");
                    entry.SetValue(media);

                    _context.Set<Base.Media>().Update(media);
                    await _context.SaveChangesAsync();
                }
            }

            return media.Id;
        }

        public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf")
        {
            Base.Media? media;
            var isNew = false;

            if (id is null)
            {
                media = new Base.Media { Id = Guid.NewGuid(), Content = content, Extension = extension };

                await _context.Set<Base.Media>().AddAsync(media);
                await _context.SaveChangesAsync();
                var entry = _memoryCache.CreateEntry($"MEDIA_{id}");
                entry.SetValue(media);
                return media.Id;
            }
            else
            {
                media = await _memoryCache.GetOrCreateAsync($"MEDIA_{id}", async entry =>
                {
                    var media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id);
                    if (media is null)
                    {
                        media = new Base.Media { Id = Guid.NewGuid(), Content = content, Extension = extension };
                        await _context.Set<Base.Media>().AddAsync(media);
                        await _context.SaveChangesAsync();
                        isNew = true;
                        return media;
                    }

                    return media;
                });

                if (!isNew && media is not null)
                {
                    media.Content = content;
                    media.Extension = extension;

                    var entry = _memoryCache.CreateEntry($"MEDIA_{id}");
                    entry.SetValue(media);

                    _context.Set<Base.Media>().Update(media);
                    await _context.SaveChangesAsync();
                }
            }

            return media.Id;
        }

        private string? GetFileExtension(string fileData)
        {
            try
            {
                var ext = Regex.Match(fileData, "data:[a-z0-9\\/]*").Value;
                ext = ext?.Replace("data:", string.Empty);

                if (ext is not null)
                    return ext switch
                    {
                        "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "doc",
                        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "xlsx",
                        "image/bmp" => "bmp",
                        "image/gif" => "gif",
                        "image/jpeg" => "jpg",
                        "image/png" => "png",
                        "application/pdf" => "pdf",
                        "application/html" => "html",
                        "text/plain" => "txt",
                        _ => "(unknown)"
                    };

                var bytes = Convert.FromBase64String(fileData);

                // PDF: %PDF
                if (fileData[0] == 0x25 && fileData[1] == 0x50 && fileData[2] == 0x44 && fileData[3] == 0x46)
                    return "pdf";

                // PNG: 89 50 4E 47
                if (fileData[0] == 0x89 && fileData[1] == 0x50 && fileData[2] == 0x4E && fileData[3] == 0x47)
                    return "png";

                // JPG: FF D8 FF
                if (fileData[0] == 0xFF && fileData[1] == 0xD8 && fileData[2] == 0xFF)
                    return "jpg";

                // GIF: 47 49 46 38
                if (fileData[0] == 0x47 && fileData[1] == 0x49 && fileData[2] == 0x46 && fileData[3] == 0x38)
                    return "gif";

                // DOCX/XLSX/PPTX: ZIP-based
                if (fileData[0] == 0x50 && fileData[1] == 0x4B)
                    return "zip"; // could be .docx/.xlsx/.pptx, need extra check inside ZIP

                return null; // unknown
            }
            catch
            {
                return null;
            }
        }
    }
}