namespace Base
{
    /// <summary>
    ///     Indicates that a required service was not registered in dependency injection.
    /// </summary>
    public class ServiceNotRegistredException<T> : Exception
    {
        public ServiceNotRegistredException()
            : base($"Service {typeof(T).Name} is not registred.")
        {

        }
    }

    /// <summary>
    ///     Contains exceptions used while validating required module availability and versions.
    /// </summary>
    public class ModuleInfoEx
    {
        /// <summary>
        ///     Indicates that a required module is missing from the running connector.
        /// </summary>
        public class NeccessaryModuleNeededException : Exception
        {
            public NeccessaryModuleNeededException(string module)
                : base($"Module '{module}' is required but was not found.")
            {

            }
        }

        /// <summary>
        ///     Indicates that a loaded module does not satisfy the required version.
        /// </summary>
        public class ModuleVersionInvalidException : Exception
        {
            public ModuleVersionInvalidException(string module, string currentVersion, string moduleVersion)
                : base($"Module '{module}' version '{currentVersion}' is lower than required '{moduleVersion}'.")
            {
            }
        }
    }
}
