using Base.Entities;

namespace Base
{
    /// <summary>
    ///     Connector
    /// </summary>
    public interface IConnector
    {
        /// <summary>
        ///     List of modules
        /// </summary>
        List<ModuleInfo> Modules { get; }

        /// <summary>
        ///     Base url
        /// </summary>
        string BaseUrl { get; }

        /// <summary>
        ///     Api version
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Get operation by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Operation? GetOperation(int id);

        /// <summary>
        ///     List of permissions (key, description)
        /// </summary>
        List<PermissionInfo> Permissions { get; }

        /// <summary>
        ///     Get dictionary by name
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IEnumerable<DictionaryItem> GetDictionary(string name);
    }
}