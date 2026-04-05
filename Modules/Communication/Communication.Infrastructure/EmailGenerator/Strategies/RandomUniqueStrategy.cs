using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomUniqueStrategy : IGenEmailStrategy
    {
        private List<string> _used = new();

        public FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender)
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

