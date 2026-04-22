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
        public Dictionary<string, object> GetVariables() => _parsers.SelectMany(x => x.Variables).ToDictionary(v => v.Key, v => v.Value);
    }
}
