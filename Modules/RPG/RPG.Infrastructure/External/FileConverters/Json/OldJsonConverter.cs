using Base;
using Newtonsoft.Json;
using RPG.Domain.Entities;
using RPG.Infrastructure.External.FileConverters.Json;

namespace RPG.Infrastructure.External.FileConverters
{
    public class OldJsonConverter : IFileConverter
    {
        private readonly IMediaProvider _mediaProvider;

        public OldJsonConverter(IMediaProvider mediaProvider)
        {
            _mediaProvider = mediaProvider;
        }

        public FileConverterType Type => FileConverterType.OldJson;

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

                var heroes = new List<Hero>();

                foreach (var hero in oldStory?.Heroes ?? new List<OldJsonHero>())
                {
                    heroes.Add(await Convert(hero, chapter));
                }

                var places = new List<Place>();

                foreach (var place in oldStory?.Places ?? new List<OldJsonElement>())
                {
                    places.Add(await Convert(place, chapter));
                }

                chapter.Heroes = heroes;
                chapter.Places = places;

                story.Chapters.Add(chapter);
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
                var image = $"data:image/jpeg,base64,{oldHero.Image}";
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
                LastName = names is null ? "unknown" : names[0],
                Description = oldHero.Description ?? "unknown description",
                Player = oldHero.Player,
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
                var image = $"data:image/jpeg,base64,{oldPlace.Image}";
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
    }
}
