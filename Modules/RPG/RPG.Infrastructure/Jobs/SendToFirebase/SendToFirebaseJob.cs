using Base;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Jobs
{
    public class SendToFirebaseJob : IJob
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Send {Story?.Title ?? StoryId.ToString()} to firebase";

        public Guid StoryId { get; set; }

        public StoryModel? Story { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.SentToFirebase;
    }
}