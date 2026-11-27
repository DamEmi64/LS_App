using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomNumberStrategy : IFluidFunction
    {
        public string TitleKey => "Fluid_functions.80301.title";

        public string? DescriptionKey => "Fluid_functions.80301.description";

        public string Invoker => "randomNumber";

        public Func<FunctionArguments, FluidContext, FluidValue> Method =>
                (args, ctx) => Handle(args);

        private FluidValue Handle(FunctionArguments arguments)
        {
            var minVal = arguments.At(0).ToNumberValue();
            var maxVal = arguments.At(1).ToNumberValue();

            if (minVal != 0 && maxVal != 0)
            {
                return NumberValue.Create(Random.Shared.Next((int)minVal, (int)maxVal));
            }

            return NumberValue.Create(Random.Shared.Next());
        }
    }
}