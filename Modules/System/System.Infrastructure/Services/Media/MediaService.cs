using Base;
using Microsoft.EntityFrameworkCore;
using System.Infrastructure.Db;
using System.Text.RegularExpressions;

namespace System.Infrastructure.Services.Media
{
    public class MediaService : IMediaProvider
    {
        private readonly SystemContext _context;
        private static readonly Regex Base64PrefixRegex = new Regex("^data:[a-z0-9/]*;base64,", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public MediaService(SystemContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid? id)
        {
            var media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return;
            }

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }

        public async Task<Base.Media?> Load(Guid id, bool removeWebsiteExtras = false)
        {
            var media = await _context.Set<Base.Media>().AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            if (media is null)
            {
                return null;
            }

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
        }

        public async IAsyncEnumerable<Base.Media> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras)
        {
            var query = _context.Set<Base.Media>()
                .AsNoTracking()
                .Where(x => ids.Contains(x.Id))
                .AsAsyncEnumerable(); // 👈 stream instead of loading all

            await foreach (var item in query)
            {
                string? contentStr = item.ContentStr;

                if (removeWebsiteExtras && !string.IsNullOrEmpty(contentStr))
                {
                    var match = Base64PrefixRegex.Match(contentStr);

                    if (match.Success)
                    {
                        // 👇 avoid Replace (full scan), just slice once
                        contentStr = contentStr.Substring(match.Length);
                    }
                }

                yield return new Base.Media
                {
                    Id = item.Id,
                    InsDate = item.InsDate,
                    ContentStr = contentStr,
                    Extension = item.Extension,
                    Content = item.Content,
                    UpdDate = item.UpdDate
                };
            }
        }

        public async Task<Guid> Save(string content, Guid? id, string? extension = null)
        {
            Base.Media? media;
            if (id is null)
            {
                media = new Base.Media { Id = Guid.NewGuid(), ContentStr = content, Extension = GetFileExtension(content) ?? "(unknown)" };

                await _context.Set<Base.Media>().AddAsync(media);
                await _context.SaveChangesAsync();
            }
            else
            {
                media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id.Value);

                if (media is null)
                {
                    media = new Base.Media { Id = Guid.NewGuid(), ContentStr = content, Extension = GetFileExtension(content) ?? "(unknown)" };
                    await _context.Set<Base.Media>().AddAsync(media);
                    await _context.SaveChangesAsync();
                    return media.Id;
                }

                media.ContentStr = content;
                media.Extension = extension ?? GetFileExtension(content) ?? "(unknown)";

                _context.Set<Base.Media>().Update(media);
                await _context.SaveChangesAsync();
            }

            return media.Id;
        }

        public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf")
        {
            Base.Media? media;
            if (id is null)
            {
                media = new Base.Media { Id = Guid.NewGuid(), Content = content, Extension = extension };

                await _context.Set<Base.Media>().AddAsync(media);
                await _context.SaveChangesAsync();
            }
            else
            {
                media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id.Value);

                if (media is null)
                {
                    media = new Base.Media { Id = Guid.NewGuid(), Content = content, Extension = extension };
                    await _context.Set<Base.Media>().AddAsync(media);
                    await _context.SaveChangesAsync();
                    return media.Id;
                }

                media.Content = content;
                media.Extension = extension;

                _context.Set<Base.Media>().Update(media);
                await _context.SaveChangesAsync();
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