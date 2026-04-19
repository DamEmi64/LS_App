namespace Base.Automation
{
    public class AutomationTask
    {
        /// <summary>
        ///     Operation id
        /// </summary>
        public int Operation { get; set; }

        /// <summary>
        ///     Task order
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
