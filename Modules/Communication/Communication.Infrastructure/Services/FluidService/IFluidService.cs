using CommunicationBase.Interfaces;

namespace Communication.Infrastructure.Services
{
    public interface IFluidService
    {
        List<IFluidFunction> GetFunctions();
        List<FluidVariableModel> GetVariables();
    }
}