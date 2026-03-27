using Base;
using Google.Cloud.Firestore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RPG.Infrastructure.External.Firebase;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class SendToFirebaseJob : IJob
    {
        private const string StoriesCollection = "stories";
        private const string ChaptersCollection = "chapters";
        private const string ImagesCollection = "images";
        private const string PlayerDataCollection = "skills";

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Send {Story?.Title ?? StoryId.ToString()} to firebase";

        public Guid StoryId { get; set; }

        // Story może być null dopóki nie zostanie załadowany w Execute
        public StoryModel? Story { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.SentToFirebase;

        public async Task Execute(IJobContext jobContext)
        {
            var options = jobContext.ServiceProvider.GetRequiredService<IOptions<FirebaseOptions>>();
            var mediaProvider = jobContext.ServiceProvider.GetRequiredService<IMediaProvider>();

            var story = Story ?? jobContext.GetData<StoryModel>() ?? throw new InvalidOperationException("Story data is missing.");
            Story = story;

            await ExecuteInternal(options.Value, mediaProvider, story);
        }

        public async Task ExecuteInternal(FirebaseOptions options, IMediaProvider mediaProvider, StoryModel story)
        {
            var firestore = await FirestoreDb.CreateAsync(options.ProjectId);

            var storyRef = firestore.Collection(StoriesCollection).Document(StoryId.ToString());

            story.Id = StoryId;

            await storyRef.SetAsync(story.ToFirebase());

            var chapters = story.Chapters ?? Enumerable.Empty<ChapterModel>();
            foreach (var chapter in chapters)
            {
                var chapterRef = firestore.Collection(ChaptersCollection).Document(chapter.Id.ToString());

                await chapterRef.SetAsync(chapter.ToFirebase());

                foreach (var image in await GetHeroesImages(chapter, mediaProvider))
                {
                    try
                    {
                        var imageRef = firestore.Collection(ImagesCollection).Document(image.Id.ToString());
                        await imageRef.SetAsync(image);
                    }
                    catch { }
                }

                foreach (var image in await GetPlacesImages(chapter, mediaProvider))
                {
                    try
                    {
                        var imageRef = firestore.Collection(ImagesCollection).Document(image.Id.ToString());
                        await imageRef.SetAsync(image);
                    }
                    catch { }
                }

                foreach (var hero in (chapter.Heroes ?? Enumerable.Empty<HeroModel>()).Where(x => x.Player is not null))
                {
                    if (hero.PlayerData is not null)
                    {
                        var playerDataId = hero.PlayerData.Id != Guid.Empty ? hero.PlayerData.Id : Guid.NewGuid();
                        var skillsRef = firestore.Collection(PlayerDataCollection).Document(playerDataId.ToString());
                        await skillsRef.SetAsync(hero.PlayerData.ToFirebase($"{hero.FirstName} {hero.LastName}"));
                    }
                }
            }
        }

        private async Task<List<FirebaseImage>> GetHeroesImages(ChapterModel chapter, IMediaProvider mediaProvider)
        {
            var list = new List<FirebaseImage>();
            var imageIds = chapter.Heroes?.Select(x => x.Image) ?? Enumerable.Empty<Guid?>();

            foreach (var imageId in imageIds)
            {
                var id = imageId ?? Guid.Empty;
                var image = await mediaProvider.Load(id);
                if (image is not null)
                {
                    list.Add(new FirebaseImage { Id = id.ToString(), Content = image.ContentStr ?? string.Empty });
                }
            }

            return list;
        }

        private async Task<List<FirebaseImage>> GetPlacesImages(ChapterModel chapter, IMediaProvider mediaProvider)
        {
            var list = new List<FirebaseImage>();
            var imageIds = chapter.Places?.Select(x => x.Image) ?? Enumerable.Empty<Guid?>();

            foreach (var imageId in imageIds)
            {
                var id = imageId ?? Guid.Empty;
                var image = await mediaProvider.Load(id);
                if (image is not null)
                {
                    list.Add(new FirebaseImage { Id = id.ToString(), Content = image.ContentStr ?? string.Empty });
                }
            }

            return list;
        }
    }
}