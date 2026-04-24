using CommunicationBase.Interfaces;

namespace Communication.Infrastructure.Services
{
    public interface IFluidService
    {
        List<IFluidFunction> GetFunctions();
        Dictionary<string, object> GetVariables();
    }
}