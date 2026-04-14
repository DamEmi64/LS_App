namespace Base
{
    /// <summary>
    ///     Automation resolver
    /// </summary>
    public interface IAutomationResolver
    {
        /// <summary>
        ///     Resolve automation process
        /// </summary>
        /// <param name="schema">Process schema</param>
        /// <param name="tasks">Automation task data</param>
        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks);
    }

    public class AutomationTask
    {
        /// <summary>
        ///     Operation Id
        /// </summary>
        public int Operation { get; set; }

        /// <summary>
        ///     Operation order
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        ///     Is task handled
        /// </summary>
        public bool Handled { get; set; }

        /// <summary>
        ///     Task data as json
        /// </summary>
        public string? JsonData { get; set; }
    }
}
