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
        IReadOnlyCollection<ModuleInfo> Modules { get; }

        /// <summary>
        ///     Api version
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     List of permissions (key, description)
        /// </summary>
        IReadOnlyCollection<PermissionInfo> Permissions { get; }
    }
}