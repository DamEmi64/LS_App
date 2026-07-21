using Base;
using Microsoft.EntityFrameworkCore;
using System.Domain.Entities;
using System.Infrastructure.Db;
using System.Text.RegularExpressions;

namespace System.Infrastructure.Services.Media
{
    public class DatabaseMediaProvider : IMediaProvider
    {
        private readonly DriveContext _context;

        private static readonly Regex Base64PrefixRegex =
            new("^data:[a-z0-9/]*;base64,", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private static readonly Regex MimeRegex =
            new("^data:([a-z0-9/\\-.+]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);


        public DatabaseMediaProvider(DriveContext context)
        {
            _context = context;
        }

        public async Task Delete(Guid? id)
        {
            if (id is null) return;

            var metadata = await _context.Set<Metadata>().FirstOrDefaultAsync(x => x.Id == id);
            if (metadata is null) return;

            _context.Metadata.Remove(metadata);
            await _context.SaveChangesAsync();
        }

        public async Task<Base.Media?> Load(Guid id, bool removeWebsiteExtras = false)
        {
            var metadata = await _context.Metadata
                    .Include(x=>x.Blob)
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);


            if (metadata is null) return null;

            if (removeWebsiteExtras && !string.IsNullOrEmpty(metadata.Blob.ContentStr))
            {
                return new Base.Media
                {
                    Id = metadata.Id,
                    Extension = metadata.Extension ?? string.Empty,
                    Content = metadata.Blob.Content?.Decrypt(),
                    ContentStr = Base64PrefixRegex.Replace(metadata.Blob.ContentStr?.Decrypt() ?? string.Empty, "")
                };
            }

            return new Base.Media
            {
                Id = metadata.Id,
                Extension = metadata.Extension ?? string.Empty,
                Content = metadata.Blob.Content?.Decrypt(),
                ContentStr = metadata.Blob.ContentStr?.Decrypt()
            };
        }

        public async IAsyncEnumerable<Base.Media?> LoadMany(IEnumerable<Guid> ids, bool removeWebsiteExtras)
        {
            foreach (var id in ids)
            {
                var metadata = await _context.Metadata
                        .Include(x => x.Blob)
                        .AsNoTracking()
                        .FirstOrDefaultAsync(x => x.Id == id);

                if (metadata is not null)
                {
                    if (removeWebsiteExtras && !string.IsNullOrEmpty(metadata.Blob.ContentStr))
                    {
                        yield return new Base.Media
                        {
                            Id = metadata.Id,
                            Extension = metadata.Extension ?? string.Empty,
                            Content = metadata.Blob.Content?.Decrypt(),
                            ContentStr = Base64PrefixRegex.Replace(metadata.Blob.ContentStr?.Decrypt() ?? string.Empty, "")
                        };
                    }

                    yield return new Base.Media
                    {
                        Id = metadata.Id,
                        Extension = metadata.Extension ?? string.Empty,
                        Content = metadata.Blob.Content?.Decrypt(),
                        ContentStr = metadata.Blob.ContentStr?.Decrypt()
                    };
                }
            }
        }

        public async Task<Guid> Save(string content, Guid? id, string? extension = null, string? owner = null)
        {
            var ext = extension ?? GetFileExtension(content) ?? "(unknown)";
            Metadata media;

            if (id is null)
            {
                media = new Metadata
                {
                    Id = Guid.NewGuid(),
                    Extension = ext,
                    Blob = new Blob
                    {
                        ContentStr = content.Encrypt(),
                        Content = null,
                    }
                };

                await _context.Metadata.AddAsync(media);
            }
            else
            {
                media = await _context.Metadata
                        .Include(x => x.Blob)
                        .FirstOrDefaultAsync(x => x.Id == id)
                         ?? new Metadata { Id = id.Value, Blob = new Blob() };

                media.Blob.ContentStr = content.Encrypt();
                media.Extension = ext;

                _context.Metadata.Update(media);
            }

            await _context.SaveChangesAsync();

            return media.Id;
        }

        public async Task<Guid> Save(byte[] content, Guid? id, string extension = "pdf", string? owner = null)
        {
            Metadata media;

            if (id is null)
            {
                media = new Metadata
                {
                    Id = Guid.NewGuid(),
                    Extension = extension,
                    Size = content.Length,
                    Blob = new Blob
                    {
                        Content = content.Encrypt(),
                        ContentStr = null,
                    }
                };

                await _context.Metadata.AddAsync(media);
            }
            else
            {
                media = await _context.Metadata
                        .Include(x => x.Blob)
                        .FirstOrDefaultAsync(x => x.Id == id)
                         ?? new Metadata { Id = id.Value, Blob = new Blob() };

                media.Blob.Content = content.Encrypt();
                media.Extension = extension;

                _context.Metadata.Update(media);
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