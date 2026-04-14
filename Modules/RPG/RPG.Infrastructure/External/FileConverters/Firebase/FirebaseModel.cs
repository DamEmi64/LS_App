using RPG.Domain.Entities;
using RPG.Infrastructure.External.Firebase;

namespace RPG.Infrastructure.External.FileConverters.Firebase
{
    public class StoryModel
    {
        public Guid Id { get; set; }
        public string Title { get; }
        public string Description { get; }
        public DateTime? StartDate { get; }
        public List<ChapterModel> Chapters { get; set; } = new();

        public StoryModel(FirebaseStory fb)
        {
            Id = Guid.TryParse(fb.Id, out var id) ? id : Guid.NewGuid();

            Title = fb.Title;
            Description = fb.Description;

            StartDate = DateTime.TryParse(fb.VersionDate, out var dt)
                ? dt
                : null;
        }
    }

    public class ChapterModel
    {
        public Guid Id { get; set; }
        public string Title { get; }
        public string Description { get; }
        public int Order { get; }

        public List<HeroModel> Heroes { get; }
        public List<PlaceModel> Places { get; }

        public ChapterModel(FirebaseChapter fb)
        {
            Id = Guid.TryParse(fb.Id, out var id) ? id : Guid.NewGuid();

            Title = fb.Title;
            Description = fb.Description;
            Order = fb.Order;

            Heroes = fb.Heroes?.Select(h => new HeroModel(h)).ToList() ?? new();
            Places = fb.Places?.Select(p => new PlaceModel(p)).ToList() ?? new();
        }
    }

    public class HeroModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; }
        public string LastName { get; }
        public string Description { get; }

        public string? Player { get; }
        public Guid? Image { get; }

        public string? ImageData { get; set; } // filled later

        public HeroModel(FirebaseHero fb)
        {
            Id = Guid.TryParse(fb.Id, out var id) ? id : Guid.NewGuid();

            FirstName = fb.FirstName;
            LastName = fb.LastName;
            Description = fb.Description;

            Player = fb.Player;

            Image = !string.IsNullOrEmpty(fb.Image) &&
                    Guid.TryParse(fb.Image, out var imgId)
                ? imgId
                : null;
        }
    }

    public class PlaceModel
    {
        public Guid Id { get; set; }
        public string Title { get; }
        public string Description { get; }

        public Guid? Image { get; }
        public string? ImageData { get; set; }

        public PlaceModel(FirebasePlace fb)
        {
            Id = Guid.TryParse(fb.Id, out var id) ? id : Guid.NewGuid();

            Title = fb.Title;
            Description = fb.Description;

            Image = !string.IsNullOrEmpty(fb.Image) &&
                    Guid.TryParse(fb.Image, out var imgId)
                ? imgId
                : null;
        }
    }

    public static class FirebaseMapper
    {
        public static void AttachImages(
            List<ChapterModel> chapters,
            Dictionary<string, FirebaseImage> images)
        {
            foreach (var chapter in chapters)
            {
                foreach (var hero in chapter.Heroes)
                {
                    if (hero.Image != null &&
                        images.TryGetValue(hero.Image.ToString() ?? string.Empty, out var img))
                    {
                        hero.ImageData = img.Content;
                    }
                }

                foreach (var place in chapter.Places)
                {
                    if (place.Image != null &&
                        images.TryGetValue(place.Image.ToString() ?? string.Empty, out var img))
                    {
                        place.ImageData = img.Content;
                    }
                }
            }
        }

        public static Story ToEntity(this StoryModel model)
            => new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Chapters = model.Chapters
                    .Select(c => c.ToEntity())
                    .ToList()
            };

        public static Chapter ToEntity(this ChapterModel model)
        {
            var chapter = new Chapter()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Order = model.Order
            };

            foreach (var hero in model.Heroes)
            {
                chapter.Heroes.Add(hero.ToEntity(chapter));
            }

            foreach (var place in model.Places)
            {
                chapter.Places.Add(place.ToEntity(chapter));
            }

            return chapter;
        }


        public static Hero ToEntity(this HeroModel model, Chapter chapter)
            => new()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Description = model.Description,

                Player = model.Player,
                Chapter = chapter
            };

        public static Place ToEntity(this PlaceModel model, Chapter chapter)
            => new()
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Chapter = chapter
            };
    }
}
