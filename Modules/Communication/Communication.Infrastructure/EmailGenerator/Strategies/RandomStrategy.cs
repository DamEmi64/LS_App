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
            var rand = new Random(DateTime.Now.Microsecond);

            if (arguments.Names.Contains(receiver))
            {
                var perReceiver = arguments[receiver];

                var valuesPerReceiver = perReceiver.ToStringValue().Split(";");

                return new StringValue(valuesPerReceiver[rand.Next() % valuesPerReceiver.Count()]);
            }

            var values = arguments.Names.Where(x => !receivers.Contains(x)).Select(x => arguments[x].ToStringValue()).ToList();

            return new StringValue(values[rand.Next() % values.Count()]);
        }
    }
}