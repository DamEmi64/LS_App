using Fluid;
using Fluid.Values;

namespace Communication.Infrastructure.EmailGenerator
{
    public interface IGenEmailStrategy
    {
        FluidValue Handle(FunctionArguments arguments, List<string> receivers, string receiver, string sender);
    }
}