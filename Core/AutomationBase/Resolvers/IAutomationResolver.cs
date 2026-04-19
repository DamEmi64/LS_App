namespace Base.Automation
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
}
