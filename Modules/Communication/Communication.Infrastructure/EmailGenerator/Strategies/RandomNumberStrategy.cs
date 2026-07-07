using Base;
using CommunicationBase;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomNumberStrategy : IFluidFunction
    {
        public string TitleKey => "communication.templates.strategies.randomNumber.title";

        public string? DescriptionKey => "communication.templates.strategies.randomNumber.description";

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