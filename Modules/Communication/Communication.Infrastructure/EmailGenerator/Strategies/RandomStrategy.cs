using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomStrategy : IGenEmailStrategy
    {
        /// <summary>
        ///  Generates a random value from the provided arguments.
        ///  If argument have name equal receiver it will take random from values from receiver name argument,
        ///  otherwise it will take random value from values from all other arguments that not named as receivers.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="receivers"></param>
        /// <param name="receiver"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender)
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