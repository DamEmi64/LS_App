using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebaseStory
    {
        [FirestoreDocumentId] //
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Title { get; set; }

        [FirestoreProperty]
        public required string VersionDate { get; set; }

        [FirestoreProperty]
        public required string Description { get; set; }

        [FirestoreProperty]
        public List<string> Chapters { get; set; } = new();
    }
}