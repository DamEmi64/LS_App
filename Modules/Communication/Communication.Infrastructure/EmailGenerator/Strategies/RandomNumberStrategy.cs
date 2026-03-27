using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class RandomNumberStrategy : IGenEmailStrategy
    {
        /// <summary>
        /// Generates a random number within the specified range.
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="receivers"></param>
        /// <param name="receiver"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender)
        {
            var rand = new Random(DateTime.Now.Microsecond);
            var minVal = arguments.At(0).ToNumberValue();
            var maxVal = arguments.At(1).ToNumberValue();

            if (minVal != 0 && maxVal != 0)
            {
                return NumberValue.Create(rand.Next((int)minVal, (int)maxVal));
            }

            return NumberValue.Create(rand.Next());
        }
    }
}