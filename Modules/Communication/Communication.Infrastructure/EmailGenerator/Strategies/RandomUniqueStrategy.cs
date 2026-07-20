using Base;
using CommunicationBase;
using CommunicationBase.Dtos;
using CommunicationBase.Interfaces;
using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomUniqueStrategy : IFluidFunction
    {
        private readonly List<string> _used = [];

        public string TitleKey => "communication.templates.strategies.randomUnique.title";

        public string? DescriptionKey => "communication.templates.strategies.randomUnique.description";

        public string Invoker => "randomUnique";

        public Func<FunctionArguments, FluidContext, FluidValue> Method =>
                (args, ctx) => Handle(args,
                            ctx.GetProperty<List<UserData>>("receivers"),
                            ctx.GetProperty<UserData>("receiver"),
                            ctx.GetProperty<UserData>("sender"));

        private FluidValue Handle(FunctionArguments arguments, List<UserData> receivers, UserData receiver, UserData sender)
        {
            int maxIterator = arguments.Count * 3, i = 0;

            var alreadyUsed = true;

            var item = string.Empty;

            while (i < maxIterator && alreadyUsed)
            {
                item = GetRandom(arguments, receivers, receiver);

                alreadyUsed = _used.Contains(item);
                i++;
            }

            _used.Add(item);
            return new StringValue(item);
        }

        private string GetRandom(FunctionArguments arguments, List<UserData> receivers, UserData receiver)
        {
            var rand = Random.Shared;

            if (arguments.HasNamed(receiver.Login))
            {
                var perReceiver = arguments[receiver.Login];
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

