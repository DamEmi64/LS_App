using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator.Strategies
{
    public class IncrementStrategy : IGenEmailStrategy
    {
        private int _seq = 0;

        /// <summary>
        ///     Generates next value
        /// </summary>
        /// <param name="arguments"></param>
        /// <param name="receivers"></param>
        /// <param name="receiver"></param>
        /// <param name="sender"></param>
        /// <returns></returns>
        public FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender)
        {
            _seq++;

            return new StringValue(_seq.ToString());
        }
    }
}