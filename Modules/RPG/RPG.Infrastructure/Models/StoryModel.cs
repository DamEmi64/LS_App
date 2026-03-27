using Base;
using RPG.Domain.Entities;

namespace RPG.Infrastructure.Models
{
    public class StoryModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public DateTimeOffset? StartDate { get; set; }
        public DateTimeOffset? EndDate { get; set; }
        public List<ChapterModel> Chapters { get; set; } = new List<ChapterModel>();
        public Guid? Summary { get; set; }
    }

    public class ChapterModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Order { get; set; } = 1;
        public required List<HeroModel> Heroes { get; set; }
        public required List<PlaceModel> Places { get; set; }
        public List<Session> Sessions { get; set; } = new List<Session>();
        public List<Link> Links { get; set; } = new List<Link>();
    }

    public class HeroModel
    {
        public Guid Id { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string Description { get; set; }
        public string? Player { get; set; }
        public Guid? Image { get; set; }
        public PlayerData? PlayerData { get; set; }
    }

    public class PlaceModel
    {
        public Guid Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public Guid? Image { get; set; }
    }

    public static class ModelExtensions
    {
        public static async Task<StoryModelExtended?> ToExtendedModel(this StoryModel? storyModel, IMediaProvider mediaProvider)
        {
            if (storyModel is null)
            {
                return null;
            }

            var model = new StoryModelExtended
            {
                Id = storyModel.Id,
                Title = storyModel.Title,
                Description = storyModel.Description,
                StartDate = storyModel.StartDate,
                EndDate = storyModel.EndDate,
                Chapters = storyModel.Chapters.Select(x => x.ToExtendedModel()).ToList()
            };

            model.Summary = (await mediaProvider.Load(storyModel.Summary ?? Guid.Empty))?.Content;

            foreach (var chapter in model.Chapters)
            {
                foreach (var hero in chapter.Heroes)
                {
                    var image = await mediaProvider.Load(hero.ImageId ?? Guid.Empty);

                    if (image is not null)
                    {
                        hero.Image = image.ContentStr;
                    }
                }
                foreach (var place in chapter.Places)
                {
                    var image = await mediaProvider.Load(place.ImageId ?? Guid.Empty);

                    if (image is not null)
                    {
                        place.Image = image.ContentStr;
                    }
                }
            }

            return model;
        }

        public static ChapterModelExtended ToExtendedModel(this ChapterModel chapterModel)
        => new()
        {
            Id = chapterModel.Id,
            Title = chapterModel.Title,
            Description = chapterModel.Description,
            Sessions = chapterModel.Sessions,
            Links = chapterModel.Links,
            Order = chapterModel.Order,
            Heroes = chapterModel.Heroes.Select(h => h.ToExtendedModel()).ToList(),
            Places = chapterModel.Places.Select(p => p.ToExtendedModel()).ToList()
        };

        public static PlaceModelExtended ToExtendedModel(this PlaceModel placeModel)
            => new()
            {
                Id = placeModel.Id,
                Title = placeModel.Title,
                Description = placeModel.Description,
                ImageId = placeModel.Image
            };

        public static HeroModelExtended ToExtendedModel(this HeroModel heroModel)
            => new()
            {
                Id = heroModel.Id,
                FirstName = heroModel.FirstName,
                LastName = heroModel.LastName,
                Description = heroModel.Description,
                Player = heroModel.Player,
                PlayerData = heroModel.PlayerData,
                ImageId = heroModel.Image
            };

        public static StoryModel ToModel(this Story story)
            => new()
            {
                StartDate = story.StartDate,
                Title = story.Title,
                Id = story.Id,
                Chapters = story.Chapters.Select(x => x.ToModel()).ToList(),
                Description = story.Description,
                EndDate = story.EndDate,
            };

        public static ChapterModel ToModel(this Chapter chapter)
            => new()
            {
                Description = chapter.Description,
                Sessions = chapter.Sessions,
                Id = chapter.Id,
                Order = chapter.Order,
                Title = chapter.Title,
                Heroes = chapter.Heroes.Select(x => x.ToModel()).ToList(),
                Places = chapter.Places.Select(x => x.ToModel()).ToList(),
                Links = chapter.Links
            };

        public static HeroModel ToModel(this Hero hero)
            => new()
            {
                Description = hero.Description,
                Id = hero.Id,
                FirstName = hero.FirstName,
                LastName = hero.LastName,
                Image = hero.Image,
                Player = hero.Player,
                PlayerData = hero.PlayerData,
            };

        public static PlaceModel ToModel(this Place place)
            => new()
            {
                Description = place.Description,
                Id = place.Id,
                Image = place.Image,
                Title = place.Title,
            };
    }
}