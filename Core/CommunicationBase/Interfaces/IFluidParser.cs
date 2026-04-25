namespace CommunicationBase.Interfaces
{
    public interface IFluidParser
    {
        /// <summary>
        ///     Fluid functions
        /// </summary>
        List<IFluidFunction> Functions { get; }

        Dictionary<string, object> Variables { get; }
    }
}
