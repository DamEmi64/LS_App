using CommunicationBase.Dtos;
using Fluid;
using Fluid.Values;

namespace CommunicationBase.Interfaces
{
    public interface IFluidFunction
    {
        /// <summary>
        ///     Function title <br/>
        ///     Recommended: key for localization.This key will be used to display the function in the UI <br/>
        /// </summary>
        public string TitleKey { get; }

        /// <summary>
        ///     Function description <br/>
        ///     Recommended: key for localization.This key will be used to display the function in the UI <br/>
        /// </summary>
        public string? DescriptionKey { get; }

        /// <summary>
        ///     Function invoker (what user write to invoke the function)
        /// </summary>
        public string Invoker { get; }

        /// <summary>
        ///     Function method
        /// </summary>
        public Func<FunctionArguments, FluidContext, FluidValue> Method { get; }
    }
}
