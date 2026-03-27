using Base;

namespace System.Infrastructure.Services.EntityContext
{
    public class EntityContext : IEntityContext
    {
        public string? Editor { get; set; }

        public void Process(Guid processId)
        {
            Editor = $"PROCESS({processId})";
        }

        public void System()
        {
            Editor = "System";
        }
    }
}
