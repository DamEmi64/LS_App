using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebasePlayerData
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty(Name = "id")]
        public required string PlayerDataId { get; set; }

        [FirestoreProperty(Name ="title")]
        public required string Title { get; set; }

        [FirestoreProperty(Name ="skills")]
        public List<FirebaseSkill> Skills { get; set; } = new List<FirebaseSkill>();
    }

    public class FirebaseSkill
    {
        [FirestoreProperty(Name ="id")]
        public required string Id { get; set; }
        [FirestoreProperty(Name ="title")]
        public required string Title { get; set; }
        [FirestoreProperty(Name = "value")]
        public decimal Value { get; set; }

    }
}
