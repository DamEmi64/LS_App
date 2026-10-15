using Base;

namespace AutomationBase
{
    public interface ISystemAutomate
    {
        /// <summary>
        ///     Automat key
        /// </summary>
        int Key { get; }

        /// <summary>
        ///     Default configuration
        /// </summary>
        object? Configuration { get; }
        /// <summary>
        ///     Can Automat be executed
        /// </summary>
        /// <param name="event"></param>
        /// <returns></returns>
        bool CanExecute(INotifyEvent @event);

        /// <summary>
        ///     Automat execution method
        /// </summary>
        /// <param name="event"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        Task Execute(INotifyEvent @event, string configuration);

    }

    public abstract class SystemAutomate : ISystemAutomate
    {
        private readonly IJobEngine _jobEngine;

        protected SystemAutomate(IJobEngine jobEngine)
        {
            _jobEngine = jobEngine;
        }
        public abstract int Key { get; }

        public virtual object? Configuration => null;

        public abstract bool CanExecute(INotifyEvent @event);

        public abstract Task Execute(INotifyEvent @event, string configuration);

        public IProcessSchema CreateJobSchema(string title) => _jobEngine.Create($"[AUTOMAT] {title} - {DateTime.Now.ToString()}");

        public async Task ExecuteJobSchema(IProcessSchema schema)
        {
            await _jobEngine.Execute(schema, new UserData { UserId = Guid.NewGuid().ToString(), Login = $"SYSTEM AUTOMAT - {Key}", Email = "Automat" });
        }
    }
}
