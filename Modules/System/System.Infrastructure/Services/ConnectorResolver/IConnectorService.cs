using Base;
using System.Collections.ObjectModel;

namespace System.Infrastructure.Services.ConnectorResolver
{
    public interface IConnectorService
    {
        IReadOnlyCollection<ModuleInfo> Modules { get; }
        IReadOnlyCollection<PermissionInfo> Permissions { get; }
        string Version { get; }

        ReadOnlyCollection<DictionaryItem> GetDictionary(string name);
        Operation? GetOperation(int id);
        void SetDictionary(IEnumerable<DictionaryItem> dictionaries);
    }
}