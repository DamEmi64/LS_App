using System.Infrastructure.Services.ConnectorResolver;

namespace Base
{
    public static class ConnectorResolverInit
    {
        public static IConnectorResolver ToResolver(this IConnector connector) => new ConnectorResolver(connector);
    }
}
