namespace CommunicationBase.Interfaces
{
    public interface IFluidParser
    {
        /// <summary>
        ///     Fluid functions
        /// </summary>
        List<IFluidFunction> Functions { get; }

        /// <summary>
        ///     Fluid variables
        /// </summary>
        Dictionary<string, object?> Variables { get; }

        /// <summary>
        ///     Get tranlsation key for a given invoker. This is used to retrieve the correct translation for a variable or function in the Fluid template.
        /// </summary>
        /// <param name="invoker"></param>
        /// <returns></returns>
        int GetTranslationKey(string invoker);
    }
}
