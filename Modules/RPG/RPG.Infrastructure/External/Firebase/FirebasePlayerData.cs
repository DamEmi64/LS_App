using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebasePlayerData
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Title { get; set; }

        [FirestoreProperty]
        public List<FirebaseSkill> Skills { get; set; } = new List<FirebaseSkill>();
    }

    public class FirebaseSkill
    {
        [FirestoreProperty]
        public required string Id { get; set; }
        [FirestoreProperty]
        public required string Title { get; set; }
        [FirestoreProperty]
        public decimal Value { get; set; }

    }
}
