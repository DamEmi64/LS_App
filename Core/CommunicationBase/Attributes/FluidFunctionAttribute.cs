namespace CommunicationBase.Attributes
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class FluidFunctionAttribute : Attribute
    {
        /// <summary>
        ///    Attrbute for function used in FluidGenerator
        /// </summary>
        public FluidFunctionAttribute()
        {
        }
    }
}
