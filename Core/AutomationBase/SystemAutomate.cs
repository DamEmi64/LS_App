using Base;

namespace AutomationBase
{
    public interface ISystemAutomate
    {
        int Key { get; }
        bool CanExecute(INotifyEvent @event);

        Task Execute(INotifyEvent @event);

    }

    public abstract class SystemAutomate : ISystemAutomate
    {
        private readonly IJobEngine _jobEngine;

        protected SystemAutomate(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }
        public abstract int Key { get; }

        public abstract bool CanExecute(INotifyEvent @event);

        public abstract Task Execute(INotifyEvent @event);

        public IProcessSchema CreateJobSchema(string title) => _jobEngine.Create($"[AUTOMAT] {title} - {DateTime.Now.ToString()}");

        public async Task ExecuteJobSchema(IProcessSchema schema)
        {
            await _jobEngine.Execute(schema, new UserData { UserId = Guid.NewGuid().ToString(), Login = $"SYSTEM AUTOMAT - {Key}", Email = "Automat" });
        }
    }
}
