using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebaseHero
    {
        [FirestoreProperty]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string FirstName { get; set; }

        [FirestoreProperty]
        public required string LastName { get; set; }

        [FirestoreProperty]
        public required string Description { get; set; }

        [FirestoreProperty]
        public string? Player { get; set; }

        [FirestoreProperty]
        public string? Image { get; set; }
    }
}