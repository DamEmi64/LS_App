
using Automation.Application;
using Base;
using Base.Entities;
using Communication.Application;
using RPG.Application;
using System.Application;

namespace Connector
{
    public class Connector : IConnector
    {
        public Connector(string baseUrl, string version)
        {
            BaseUrl = baseUrl;
            Version = version;
        }

        public List<ModuleInfo> Modules => new()
        {
            new SystemModule().Info(),
            new RPGModule().Info(),
            new CommunicationModule().Info(),
            new AutomationModule().Info()
        };

        public string BaseUrl { get; set; } = string.Empty;

        public string Version { get; set; } = "unknown";

        public IEnumerable<DictionaryItem> DictionaryItems { get; set; } = new List<DictionaryItem>();

        public Operation? GetOperation(int id)
        {
            return Modules.SelectMany(x => x.Module.Operations).FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<DictionaryItem> GetDictionary(string name) => DictionaryItems.Where(x => x.Dictionary.Equals(name, StringComparison.OrdinalIgnoreCase));

        public List<PermissionInfo> Permissions => new()
        {
            PermissionInfo.Create("rpg","Manage RPG sessions",true),
            PermissionInfo.Create("communication","Manage and send Emails",true),
            PermissionInfo.Create("process","Manage background processes",false),
            PermissionInfo.Create("automation","Manage automation tasks",false),
        };
    }
}