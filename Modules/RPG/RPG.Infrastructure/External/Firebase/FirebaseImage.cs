using Google.Cloud.Firestore;

namespace RPG.Infrastructure.External.Firebase
{
    [FirestoreData]
    public class FirebaseImage
    {
        [FirestoreDocumentId]
        public required string Id { get; set; }

        [FirestoreProperty]
        public required string Content { get; set; }
    }
}