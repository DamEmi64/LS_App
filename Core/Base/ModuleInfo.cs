namespace Base
{
    /// <summary>
    ///     Module information
    /// </summary>
    public class ModuleInfo
    {
        /// <summary>
        ///     Module name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        ///     Module version
        ///     If used in <see cref="Extensions.UseModules(Microsoft.AspNetCore.Builder.IApplicationBuilder, ModuleInfo[])"/> verify minimal version to run
        /// </summary>
        public required string Version { get; set; }

        /// <summary>
        ///     Module configuration
        /// </summary>
        public required IModule Module { get; set; }
    }
}
