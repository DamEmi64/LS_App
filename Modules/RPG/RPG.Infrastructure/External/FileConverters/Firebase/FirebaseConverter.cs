using Base;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using RPG.Domain.Dictionaries;
using RPG.Domain.Entities;
using RPG.Infrastructure.External.Firebase;

namespace RPG.Infrastructure.External.FileConverters.Firebase
{
    public class FirebaseConverter : IRPGDataConverter
    {
        private const string StoriesCollection = "stories";
        private const string ChaptersCollection = "chapters";
        private const string ImagesCollection = "images";

        private readonly IMediaProvider _mediaProvider;
        private readonly FirebaseOptions _options;

        public FirebaseConverter(IMediaProvider mediaProvider, IOptions<FirebaseOptions> options)
        {
            _mediaProvider = mediaProvider;
            _options = options.Value;
        }

        public int Type => RPGFileTypes.Firebase;

        public async Task<Story> Convert(string data)
        {
            var storyModel = await LoadFromFirebaseByTitle(data);

            ArgumentNullException.ThrowIfNull(storyModel, "Story not found in Firebase");

            var story = storyModel.ToEntity();

            foreach (var chapter in story.Chapters)
            {
                var chapterModel = storyModel.Chapters.FirstOrDefault(c => c.Title == chapter.Title);
                if (chapterModel is null)
                    continue;

                foreach (var hero in chapter.Heroes)
                {
                    var heroModel = chapterModel.Heroes?.FirstOrDefault(h => h.FirstName == hero.FirstName && h.LastName == hero.LastName);
                    if (heroModel?.ImageData != null)
                    {
                        var imageId = await _mediaProvider.Save(heroModel.ImageData, null, "jpeg");
                        hero.Image = imageId;
                    }
                }
                foreach (var place in chapter.Places)
                {
                    var placeModel = chapterModel.Places?.FirstOrDefault(p => p.Title == place.Title);
                    if (placeModel?.ImageData != null)
                    {
                        var imageId = await _mediaProvider.Save(placeModel.ImageData, null, "jpeg");
                        place.Image = imageId;
                    }
                }
            }

            return story;
        }

        public Task<string> Convert(Story story)
        {
            throw new NotImplementedException();
        }

        private async Task<StoryModel?> LoadFromFirebaseByTitle(string title)
        {
            var firestore = await FirestoreDb.CreateAsync(_options.ProjectId);

            var storyQuery = firestore
                                .Collection(StoriesCollection)
                                .WhereEqualTo("Title", title)
                                .Limit(1);

            var storySnapshot = await storyQuery.GetSnapshotAsync();
            var storyDoc = storySnapshot.Documents.FirstOrDefault();

            if (storyDoc == null)
                return null;

            var fbStory = storyDoc.ConvertTo<FirebaseStory>();
            var story = new StoryModel(fbStory);

            // load chapters by ids
            var chapters = new List<ChapterModel>();

            foreach (var chapterId in fbStory.Chapters)
            {
                var doc = await firestore
                    .Collection(ChaptersCollection)
                    .Document(chapterId)
                    .GetSnapshotAsync();

                if (doc.Exists)
                {
                    var fbChapter = doc.ConvertTo<FirebaseChapter>();
                    chapters.Add(new ChapterModel(fbChapter));
                }
            }

            story.Chapters = chapters;

            var imagesSnapshot = await firestore.Collection(ImagesCollection).GetSnapshotAsync();
            var imagesDict = imagesSnapshot.Documents
                .ToDictionary(
                    doc => doc.Id,
                    doc => doc.ConvertTo<FirebaseImage>()
                );

            // Attach images to heroes / places
            foreach (var chapter in chapters)
            {
                if (chapter.Heroes != null)
                {
                    foreach (var hero in chapter.Heroes)
                    {
                        if (hero.Image != null && imagesDict.TryGetValue(hero.Image.ToString() ?? Guid.Empty.ToString(), out var img))
                        {
                            hero.ImageData = img.Content;
                        }
                    }
                }

                if (chapter.Places != null)
                {
                    foreach (var place in chapter.Places)
                    {
                        if (place.Image != null && imagesDict.TryGetValue(place.Image.ToString() ?? Guid.Empty.ToString(), out var img))
                        {
                            place.ImageData = img.Content;
                        }
                    }
                }
            }

            return story;
        }
    }
}

