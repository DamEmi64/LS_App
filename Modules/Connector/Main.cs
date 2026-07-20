using Base;
using Microsoft.Extensions.Hosting;

namespace Connector
{
    /// <summary>
    ///     CLASS NOT TO REMOVE (ONLY EDIT)
    /// </summary>
    public static class Main
    {
        public static BaseStartup InitializeConnector(this IHostApplicationBuilder builder)
        {
            return new Connector(builder);
        }
    }
}
