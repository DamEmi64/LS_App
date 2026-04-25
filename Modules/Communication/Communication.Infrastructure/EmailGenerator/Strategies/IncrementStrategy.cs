using CommunicationBase;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class IncrementStrategy : IFluidFunction
    {
        public string TitleKey => "communication.templates.strategies.increment.title";

        public string? DescriptionKey => "communication.templates.strategies.increment.description";

        public string Invoker => "increment";
        public Func<FunctionArguments, FluidContext, FluidValue> Method => Handle;

        private FluidValue Handle(FunctionArguments arguments, FluidContext context)
        {
            var seq = context.GetProperty<int>("seq");
            context.SetProperty("seq", ++seq);

            return new StringValue(seq.ToString());
        }
    }
}