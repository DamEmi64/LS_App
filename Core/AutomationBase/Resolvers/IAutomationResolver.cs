namespace Base.Automation
{
    /// <summary>
    ///     Automation resolver
    /// </summary>
    public interface IAutomationResolver
    {
        /// <summary>
        ///     Convert to event id using notify id (from notifier)
        /// </summary>
        /// <param name="notifyTypeId"></param>
        /// <returns>Event Id, if null there is no corresponding event</returns>
        public int? ConvertToEventId(int notifyTypeId);

        /// <summary>
        ///     Resolve automation process
        /// </summary>
        /// <param name="schema">Process schema</param>
        /// <param name="tasks">Automation task data</param>
        public void Resolve(IProcessSchema schema, IEnumerable<AutomationTask> tasks);
    }
}
