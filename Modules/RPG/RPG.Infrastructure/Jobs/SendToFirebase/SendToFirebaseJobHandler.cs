using Base;
using RPG.Infrastructure.External.Firebase;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class SendToFirebaseJobHandler : JobHandler<SendToFirebaseJob>
    {
        private const string StoriesCollection = "stories";
        private const string ChaptersCollection = "chapters";
        private const string ImagesCollection = "images";
        private const string PlayerDataCollection = "skills";

        private readonly IMediaProvider _mediaProvider;

        public SendToFirebaseJobHandler(
            IJobContext jobContext,
            IMediaProviderFactory mediaProviderFactory)
            : base(jobContext)
        {
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
        }

        public override async Task Execute(SendToFirebaseJob request)
        {
            var story = request.Story
                ?? GetData<StoryModel>()
                ?? throw new InvalidOperationException("Story data is missing.");

            request.Story = story;

            var firestore = await FirebaseExtensions.GetDb();

            var storyRef = firestore
                .Collection(StoriesCollection)
                .Document(request.StoryId.ToString());

            story.Id = request.StoryId;

            await storyRef.SetAsync(story.ToFirebase());

            var chapters = story.Chapters ?? Enumerable.Empty<ChapterModel>();

            foreach (var chapter in chapters)
            {
                var chapterRef = firestore
                    .Collection(ChaptersCollection)
                    .Document(chapter.Id.ToString());

                await chapterRef.SetAsync(chapter.ToFirebase());

                foreach (var image in await GetHeroesImages(chapter))
                {
                    try
                    {
                        var imageRef = firestore
                            .Collection(ImagesCollection)
                            .Document(image.Id.ToString());

                        await imageRef.SetAsync(image);
                    }
                    catch
                    {
                    }
                }

                foreach (var image in await GetPlacesImages(chapter))
                {
                    try
                    {
                        var imageRef = firestore
                            .Collection(ImagesCollection)
                            .Document(image.Id.ToString());

                        await imageRef.SetAsync(image);
                    }
                    catch
                    {
                    }
                }

                foreach (var hero in (chapter.Heroes ?? Enumerable.Empty<HeroModel>())
                    .Where(x => x.Player is not null))
                {
                    if (hero.PlayerData is not null)
                    {
                        var playerDataId = hero.PlayerData.Id != Guid.Empty
                            ? hero.PlayerData.Id
                            : Guid.NewGuid();

                        var skillsRef = firestore
                            .Collection(PlayerDataCollection)
                            .Document(playerDataId.ToString());

                        await skillsRef.SetAsync(
                            hero.PlayerData.ToFirebase($"{hero.FirstName} {hero.LastName}"));
                    }
                }
            }
        }

        private async Task<List<FirebaseImage>> GetHeroesImages(ChapterModel chapter)
        {
            var list = new List<FirebaseImage>();

            var imageIds = chapter.Heroes?.Select(x => x.Image)
                ?? Enumerable.Empty<Guid?>();

            foreach (var imageId in imageIds)
            {
                var id = imageId ?? Guid.Empty;

                var image = await _mediaProvider.Load(id);

                if (image is not null)
                {
                    var content = image.ContentStr;

                    list.Add(new FirebaseImage
                    {
                        Id = id.ToString(),
                        Content = content ?? string.Empty
                    });
                }
            }

            return list;
        }

        private async Task<List<FirebaseImage>> GetPlacesImages(ChapterModel chapter)
        {
            var list = new List<FirebaseImage>();

            var imageIds = chapter.Places?.Select(x => x.Image)
                ?? Enumerable.Empty<Guid?>();

            foreach (var imageId in imageIds)
            {
                var id = imageId ?? Guid.Empty;

                var image = await _mediaProvider.Load(id);

                if (image is not null)
                {
                    var content = image.ContentStr;

                    list.Add(new FirebaseImage
                    {
                        Id = id.ToString(),
                        Content = content ?? string.Empty
                    });
                }
            }

            return list;
        }
    }
}