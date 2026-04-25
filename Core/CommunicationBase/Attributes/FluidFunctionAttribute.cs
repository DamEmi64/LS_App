namespace CommunicationBase.Attributes
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class FluidFunctionAttribute : Attribute
    {
        /// <summary>
        ///    Attrbute for function used in FluidGenerator
        /// </summary>
        /// <param name="invoker">The invoker associated with the function.</param>
        /// <param name="title">The title of the function.</param>
        /// <param name="description">The description of the function.</param>
        public FluidFunctionAttribute(string? invoker = null, string? title = null, string? description = null)
        {
            Invoker = invoker;
            Title = title;
            Description = description;
        }

        public string? Invoker { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
