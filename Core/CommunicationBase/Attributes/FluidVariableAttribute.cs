namespace CommunicationBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FluidVariableAttribute : Attribute
    {
        /// <summary>
        ///    Attrbute for variable used in FluidGenerator
        /// </summary>
        /// <param name="name">Name of the variable.</param>
        public FluidVariableAttribute(string? name = null)
        {
            Name = name;
        }

        public string? Name { get; set; }
    }
}
