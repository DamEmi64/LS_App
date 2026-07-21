using AutoMapper;
using Base;
using Newtonsoft.Json;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;

namespace RPG.Infrastructure.External.FileConverters.Json
{
    public interface IJsonConverter : IRPGDataConverter
    {
    }

    public class JsonConverter : IJsonConverter
    {
        private readonly IMapper _mapper;
        private readonly IMediaProvider _mediaProvider;

        public JsonConverter(IMapper mapper, IMediaProviderFactory mediaProviderFactory)
        {
            _mapper = mapper;
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
        }

        public int Type => RPGFileTypes.Json;

        public async Task<Story> Convert(string data)
        {
            var dto = JsonConvert.DeserializeObject<StoryDto>(data);

            var story = _mapper.Map<Story>(dto);

            foreach (var chapter in story.Chapters)
            {
                var chapterDto = dto?.Chapters.FirstOrDefault(x => x.Title == chapter.Title);

                if (chapterDto is null)
                    continue;

                foreach (var hero in chapter.Heroes)
                {
                    var image = chapterDto.Heroes.FirstOrDefault(x => x.FirstName == hero.FirstName && x.LastName == hero.LastName)?.Image;

                    if (string.IsNullOrWhiteSpace(image))
                        continue;

                    var imageId = await _mediaProvider.Save(image, null, "jpeg");

                    hero.Image = imageId;
                }

                foreach (var place in chapter.Places)
                {
                    var image = chapterDto.Places.FirstOrDefault(x => x.Title == place.Title)?.Image;

                    if (string.IsNullOrWhiteSpace(image))
                        continue;

                    var imageId = await _mediaProvider.Save(image, null, "jpeg");

                    place.Image = imageId;
                }
            }

            return story;
        }

        public async Task<string> Convert(Story story)
        {
            var dto = _mapper.Map<StoryDto>(story);

            foreach (var chapterDto in dto.Chapters)
            {
                var chapter = story.Chapters.FirstOrDefault(x => x.Title == chapterDto.Title);

                if (chapter is null)
                    continue;

                foreach (var heroDto in chapterDto.Heroes)
                {
                    var hero = chapter.Heroes.FirstOrDefault(x =>
                        x.FirstName == heroDto.FirstName &&
                        x.LastName == heroDto.LastName);

                    if (hero?.Image is null)
                        continue;

                    if (hero.Image is not null)
                    {
                        var media = await _mediaProvider.Load(hero.Image ?? Guid.Empty);

                        if (media is not null)
                        {
                            heroDto.Image = media.ContentStr;
                        }
                    }
                }

                foreach (var placeDto in chapterDto.Places)
                {
                    var place = chapter.Places.FirstOrDefault(x =>
                        x.Title == placeDto.Title);

                    if (place?.Image is null)
                        continue;

                    if (place.Image is not null)
                    {
                        var media = await _mediaProvider.Load(place.Image ?? Guid.Empty);

                        if (media is not null)
                        {
                            placeDto.Image = media.ContentStr;
                        }
                    }
                }
            }

            return JsonConvert.SerializeObject(dto);
        }
    }
}
