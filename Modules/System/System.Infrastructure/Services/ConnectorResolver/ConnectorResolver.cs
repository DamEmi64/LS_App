using Base;
using Base.Entities;
using System.Collections.ObjectModel;

namespace System.Infrastructure.Services.ConnectorResolver
{
    public class ConnectorResolver : IConnectorResolver
    {
        private readonly IConnector _connector;
        private List<DictionaryItem> _dictionaries { get; set; } = new List<DictionaryItem>();

        public ConnectorResolver(IConnector connector)
        {
            _connector = connector;
        }

        public IReadOnlyCollection<ModuleInfo> Modules => _connector.Modules;

        public string Version => _connector.Version;

        public IReadOnlyCollection<PermissionInfo> Permissions => _connector.Permissions;

        public void SetDictionary(IEnumerable<DictionaryItem> dictionaries)
        {
            _dictionaries = dictionaries.ToList();
        }

        public Operation? GetOperation(int id) => _connector.Modules.SelectMany(x => x.Module.Operations).FirstOrDefault(x => x.Id == id);

        public ReadOnlyCollection<DictionaryItem> GetDictionary(string name)
            => _dictionaries.Where(x => x.Dictionary.Equals(name, StringComparison.OrdinalIgnoreCase)).ToList().AsReadOnly();
    }
}
