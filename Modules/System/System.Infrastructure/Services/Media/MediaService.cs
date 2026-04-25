using Base;
using Microsoft.EntityFrameworkCore;
using System.Infrastructure.Db;
using System.Text.RegularExpressions;

namespace System.Infrastructure.Services.Media
{
    public class MediaService : IMediaProvider
    {
        private readonly SystemContext _context;

        private static readonly Regex Base64PrefixRegex =
            new("^data:[a-z0-9/]*;base64,", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex MimeRegex =
            new("^data:([a-z0-9/\\-.+]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(10);

        public MediaService(SystemContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid? id)
        {
            if (id is null) return;

            var media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id);
            if (media is null) return;

            _context.Media.Remove(media);
            await _context.SaveChangesAsync();
        }

        public async Task<Base.Media?> Load(Guid id, bool removeWebsiteExtras = false)
        {
            var media = await _context.Set<Base.Media>()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (media is null) return null;

            if (removeWebsiteExtras && !string.IsNullOrEmpty(media.ContentStr))
            {
                return new Base.Media
                {
                    Id = media.Id,
                    InsDate = media.InsDate,
                    UpdDate = media.UpdDate,
                    Extension = media.Extension,
                    Content = media.Content,
                    ContentStr = Base64PrefixRegex.Replace(media.ContentStr, "")
                };
            }

            return media;
        }

        public async IAsyncEnumerable<Base.Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras)
        {
            foreach (var id in ids)
            {
                var media = await _context.Set<Base.Media>()
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                if (media is not null)
                {
                    if (removeWebsiteExtras && !string.IsNullOrEmpty(media.ContentStr))
                    {
                        yield return new Base.Media
                        {
                            Id = media.Id,
                            InsDate = media.InsDate,
                            UpdDate = media.UpdDate,
                            Extension = media.Extension,
                            Content = media.Content,
                            ContentStr = Base64PrefixRegex.Replace(media.ContentStr, "")
                        };
                    }

                    yield return media;
                }
            }
        }

        public async Task<Guid> Save(string content, Guid? id, string? extension = null)
        {
            var ext = extension ?? GetFileExtension(content) ?? "(unknown)";
            Base.Media media;

            if (id is null)
            {
                media = new Base.Media
                {
                    Id = Guid.NewGuid(),
                    ContentStr = content,
                    Extension = ext
                };

                await _context.Set<Base.Media>().AddAsync(media);
            }
            else
            {
                media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id)
                         ?? new Base.Media { Id = id.Value };

                media.ContentStr = content;
                media.Extension = ext;

                _context.Set<Base.Media>().Update(media);
            }

            await _context.SaveChangesAsync();

            return media.Id;
        }

        public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf")
        {
            Base.Media media;

            if (id is null)
            {
                media = new Base.Media
                {
                    Id = Guid.NewGuid(),
                    Content = content,
                    Extension = extension
                };

                await _context.Set<Base.Media>().AddAsync(media);
            }
            else
            {
                media = await _context.Set<Base.Media>().FirstOrDefaultAsync(x => x.Id == id)
                         ?? new Base.Media { Id = id.Value };

                media.Content = content;
                media.Extension = extension;

                _context.Set<Base.Media>().Update(media);
            }

            await _context.SaveChangesAsync();

            return media.Id;
        }

        private string? GetFileExtension(string fileData)
        {
            try
            {
                var match = MimeRegex.Match(fileData);
                if (!match.Success) return null;

                return match.Groups[1].Value switch
                {
                    "application/vnd.openxmlformats-officedocument.wordprocessingml.document" => "docx",
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" => "xlsx",
                    "image/bmp" => "bmp",
                    "image/gif" => "gif",
                    "image/jpeg" => "jpg",
                    "image/png" => "png",
                    "application/pdf" => "pdf",
                    "text/html" => "html",
                    "text/plain" => "txt",
                    _ => "(unknown)"
                };
            }
            catch
            {
                return null;
            }
        }
    }
}