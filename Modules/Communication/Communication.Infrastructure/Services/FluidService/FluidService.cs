using CommunicationBase.Interfaces;

namespace Communication.Infrastructure.Services
{
    public class FluidService : IFluidService
    {
        private readonly List<IFluidParser> _parsers;

        public FluidService(IEnumerable<IFluidParser> parsers)
        {
            _parsers = parsers.ToList();
        }

        public List<IFluidFunction> GetFunctions() => _parsers.SelectMany(x => x.Functions).ToList();
        public List<FluidVariableModel> GetVariables() => _parsers.SelectMany(x => x.Variables.Select(y => new FluidVariableModel
        {
            Data = y.Value,
            Invoker = y.Key,
            Translation = x.GetTranslationKey(y.Key)
        })).ToList();
    }
}
