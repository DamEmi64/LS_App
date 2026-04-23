using System;
using System.Collections.Generic;
using System.Text;

namespace Base
{
    public class ServiceNotRegistredException<T> : Exception
    {
        public ServiceNotRegistredException()
            : base($"Service {typeof(T).Name} is not registred.")
        {

        }
    }

    public class ModuleInfoEx
    {
        public class NeccessaryModuleNeededException : Exception
        {
            public NeccessaryModuleNeededException(string module)
                : base($"Module '{module}' is required but was not found.")
            {

            }
        }

        public class ModuleVersionInvalidException : Exception
        {
            public ModuleVersionInvalidException(string module, string currentVersion, string moduleVersion)
                : base($"Module '{module}' version '{currentVersion}' is lower than required '{moduleVersion}'.")
            {
            }
        }
    }
}
