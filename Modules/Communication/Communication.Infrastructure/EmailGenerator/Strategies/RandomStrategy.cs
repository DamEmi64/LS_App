using CommunicationBase;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomStrategy : IFluidFunction
    {
        public string TitleKey => "communication.templates.strategies.random.title";

        public string? DescriptionKey => "communication.templates.strategies.random.description";

        public string Invoker => "random";

        public Func<FunctionArguments, FluidContext, FluidValue> Method =>
                (args, ctx) => Handle(args,
                            ctx.GetProperty<List<string>>("receivers"),
                            ctx.GetProperty<string>("receiver"),
                            ctx.GetProperty<string>("sender"));

        private FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender)
        {
            var rand = Random.Shared;

            if (arguments.HasNamed(receiver))
            {
                var perReceiver = arguments[receiver];
                var valuesPerReceiver = perReceiver.ToStringValue()
                                                   .Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (valuesPerReceiver.Length == 0)
                    return new StringValue(string.Empty);

                return new StringValue(valuesPerReceiver[rand.Next(valuesPerReceiver.Length)]);
            }

            var namedArguments = arguments.Names.Select(x => arguments[x].ToStringValue());
            var values = arguments.Values
                                    .Select(x => x.ToStringValue())
                                    .Where(x => !namedArguments.Contains(x))
                                    .ToList();

            if (values.Count == 0)
                return new StringValue(string.Empty);

            return new StringValue(values[rand.Next(values.Count)]);
        }
    }
}