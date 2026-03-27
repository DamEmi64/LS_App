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
            var rand = new Random(DateTime.Now.Microsecond);

            if (arguments.Names.Contains(receiver))
            {
                var perReceiver = arguments[receiver];

                var valuesPerReceiver = perReceiver.ToStringValue().Split(";");

                return valuesPerReceiver[rand.Next() % valuesPerReceiver.Count()];
            }

            var values = arguments.Names.Where(x => !receivers.Contains(x)).Select(x => arguments[x].ToStringValue()).ToList();

            return values[rand.Next() % values.Count];
        }
    }
}