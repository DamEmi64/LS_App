using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebaseChapter
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Title { get; set; }

        [FirestoreProperty]
        public int Order { get; set; }

        [FirestoreProperty]
        public required string Description { get; set; }

        [FirestoreProperty]
        public List<FirebaseHero> Heroes { get; set; } = new List<FirebaseHero>();

        [FirestoreProperty]
        public List<FirebasePlace> Places { get; set; } = new List<FirebasePlace>();
    }
}