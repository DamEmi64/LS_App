using Automation.Application;
using Base;
using Communication.Application;
using RPG.Application;
using System.Application;

namespace Connector
{
    public class Connector : IConnector
    {
        public IReadOnlyCollection<ModuleInfo> Modules => new List<ModuleInfo>
        {
            new SystemModule().Info(),
            new RPGModule().Info(),
            new CommunicationModule().Info(),
            new AutomationModule().Info()
        };

        public string Version => AppConfiguration.Version;

        public IEnumerable<DictionaryItem> DictionaryItems { get; set; } = new List<DictionaryItem>();

        public IReadOnlyCollection<PermissionInfo> Permissions => new List<PermissionInfo>
        {
            PermissionInfo.Create("rpg","Read RPG sessions",true),
            PermissionInfo.Create("rpg_write","Manage RPG sessions",false),
            PermissionInfo.Create("rpg_draft","Manage Drafts of RPG sessions",false),
            PermissionInfo.Create("communication","Manage and send Emails",true),
            PermissionInfo.Create("process","Manage background processes",false),
            PermissionInfo.Create("automation","Manage automation tasks",false),
        };
    }
}