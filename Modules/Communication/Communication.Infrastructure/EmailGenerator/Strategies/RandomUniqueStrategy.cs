using CommunicationBase;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomUniqueStrategy : IFluidFunction
    {
        public string TitleKey => "communication.templates.strategies.randomUnique.title";

        public string? DescriptionKey => "communication.templates.strategies.randomUnique.description";

        public string Invoker => "randomUnique";

        public Func<FunctionArguments, FluidContext, FluidValue> Method =>
                (args, ctx) => Handle(args,
                            ctx.GetProperty<List<string>>("receivers"),
                            ctx.GetProperty<string>("receiver"),
                            ctx.GetProperty<string>("sender"),
                            ctx.GetProperty<List<string>>("used"));

        private FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender, List<string> used)
        {
            int maxIterator = arguments.Count * 3, i = 0;

            var alreadyUsed = true;

            var item = string.Empty;

            while (i < maxIterator && alreadyUsed)
            {
                item = GetRandom(arguments, receivers, receiver);

                alreadyUsed = used.Contains(item);
                i++;
            }

            used.Add(item);
            return new StringValue(item);
        }

        private string GetRandom(FunctionArguments arguments, List<string> receivers, string receiver)
        {
            var rand = Random.Shared;

            if (arguments.HasNamed(receiver))
            {
                var perReceiver = arguments[receiver];
                var valuesPerReceiver = perReceiver.ToStringValue()
                                                   .Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (valuesPerReceiver.Length == 0)
                    return string.Empty;

                return valuesPerReceiver[rand.Next(valuesPerReceiver.Length)];
            }

            var namedArguments = arguments.Names.Select(x => arguments[x].ToStringValue());
            var values = arguments.Values
                                    .Select(x => x.ToStringValue())
                                    .Where(x => !namedArguments.Contains(x))
                                    .ToList();

            if (values.Count == 0)
                return string.Empty;

            return values[rand.Next(values.Count)];
        }
    }
}

