using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebasePlace
    {
        [FirestoreProperty]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Title { get; set; }

        [FirestoreProperty]
        public required string Description { get; set; }

        [FirestoreProperty]
        public string? Image { get; set; }
    }
}