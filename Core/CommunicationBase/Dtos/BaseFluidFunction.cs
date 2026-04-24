using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace CommunicationBase.Dtos
{
    public class BaseFluidFunction : IFluidFunction
    {
        public string TitleKey { get; set; } = string.Empty;

        public string? DescriptionKey { get; set; }

        public string Invoker { get; set; } = string.Empty;

        public Func<FunctionArguments, FluidContext, FluidValue> Method { get; set; } = (args, context) => new StringValue(string.Empty);
    }
}
