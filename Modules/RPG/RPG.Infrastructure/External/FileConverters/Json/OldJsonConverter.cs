using Base;
using Newtonsoft.Json;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;
using RPG.Infrastructure.External.FileConverters.Json;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace RPG.Infrastructure.External.FileConverters
{
    public class OldJsonConverter : IRPGDataConverter
    {
        private readonly IMediaProvider _mediaProvider;

        public OldJsonConverter(IMediaProviderFactory mediaProviderFactory)
        {
            _mediaProvider = mediaProviderFactory.Create();
        }

        public int Type => RPGFileTypes.OldJson;

        public async Task<Story> Convert(string data)
        {
            var oldStory = JsonConvert.DeserializeObject<OldJsonStory>(data);

            if (oldStory is null)
            {
                throw new InvalidOperationException("Failed to deserialize story");
            }

            var story = new Story();

            story.Title = oldStory?.Title ?? string.Empty;

            foreach (var item in oldStory?.Chapters ?? new List<OldJsonElement>())
            {
                var chapter = new Chapter
                {
                    Title = item.Title ?? string.Empty,
                    Description = item.Description ?? string.Empty,
                };

                story.Chapters.Add(chapter);
            }

            var firstchapter = story.Chapters.FirstOrDefault();

            if (firstchapter is not null)
            {
                var heroes = new List<Hero>();

                foreach (var hero in oldStory?.Heroes ?? new List<OldJsonHero>())
                {
                    heroes.Add(await Convert(hero, firstchapter));
                }

                foreach (var hero in oldStory?.Npcs ?? new List<OldJsonElement>())
                {
                    heroes.Add(await ConvertHero(hero, firstchapter));
                }

                var places = new List<Place>();

                foreach (var place in oldStory?.Places ?? new List<OldJsonElement>())
                {
                    places.Add(await Convert(place, firstchapter));
                }

                firstchapter.Heroes = heroes;
                firstchapter.Places = places;
            }

            return story;
        }

        public async Task<string> Convert(Story story)
        {
            throw new NotImplementedException("NOT SUPPORTED");
        }

        private async Task<Hero> Convert(OldJsonHero oldHero, Chapter chapter)
        {
            var names = oldHero.Title?.Split(' ');

            PlayerData? playerData = null;
            Guid? imageId = null;

            if (oldHero.Image is not null)
            {
                var imageData = CompressBase64Image(oldHero.Image);
                var image = $"data:image/jpeg,base64,{imageData}";
                var media = await _mediaProvider.Save(image, null);
                imageId = media;
            }

            if (oldHero.Skills?.Count > 0)
            {
                playerData = new PlayerData
                {
                    Id = Guid.NewGuid(),
                    Skills = oldHero.Skills?.Select(x => new Skill
                    {
                        Id = Guid.NewGuid(),
                        Title = x.Name ?? "unknown skill",
                        Value = x.Value
                    }).ToList() ?? new List<Skill>()
                };
            }

            return new Hero
            {
                FirstName = names is null ? "unknown" : names[0],
                LastName = names is null ? "unknown" : oldHero?.Title?.Replace($"{names[0]} ", string.Empty) ?? "unknown",
                Description = oldHero?.Description ?? "unknown description",
                Player = oldHero?.Player,
                Chapter = chapter,
                PlayerData = playerData,
                Image = imageId
            };
        }

        private async Task<Hero> ConvertHero(OldJsonElement oldHero, Chapter chapter)
        {
            var names = oldHero.Title?.Split(' ');

            PlayerData? playerData = null;
            Guid? imageId = null;

            if (oldHero.Image is not null)
            {
                var imageData = CompressBase64Image(oldHero.Image);
                var image = $"data:image/jpeg,base64,{imageData}";
                var media = await _mediaProvider.Save(image, null);
                imageId = media;
            }

            return new Hero
            {
                FirstName = names is null ? "unknown" : names[0],
                LastName = names is null ? "unknown" : oldHero?.Title?.Replace($"{names[0]} ", string.Empty) ?? "unknown",
                Description = oldHero?.Description ?? "unknown description",
                Chapter = chapter,
                PlayerData = playerData,
                Image = imageId
            };
        }

        private async Task<Place> Convert(OldJsonElement oldPlace, Chapter chapter)
        {
            Guid? imageId = null;

            if (oldPlace.Image is not null)
            {
                var imageData = CompressBase64Image(oldPlace.Image);
                var image = $"data:image/jpeg,base64,{imageData}";
                var media = await _mediaProvider.Save(image, null);
                imageId = media;
            }

            return new Place
            {
                Title = oldPlace.Title ?? string.Empty,
                Description = oldPlace.Description ?? string.Empty,
                Chapter = chapter,
                Image = imageId
            };
        }

        private string CompressBase64Image(string base64)
        {
            var bytes = System.Convert.FromBase64String(base64);

            using var image = Image.Load(bytes);

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Mode = ResizeMode.Max,
                Size = new Size(800, 800)
            }));

            var encoder = new JpegEncoder { Quality = 50 };

            using var ms = new MemoryStream();
            image.Save(ms, encoder);

            return System.Convert.ToBase64String(ms.ToArray());
        }
    }
}
