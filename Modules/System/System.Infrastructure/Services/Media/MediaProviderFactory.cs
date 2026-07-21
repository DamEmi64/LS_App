using Base;
using Microsoft.Extensions.DependencyInjection;

namespace System.Infrastructure.Services.Media
{
    public class MediaProviderFactory : IMediaProviderFactory
    {
        private readonly IServiceProvider _services;

        public MediaProviderFactory(IServiceProvider services)
        {
            _services = services;
        }

        public IMediaProvider Create(string? providerName = null)
        {
            IMediaProvider? mediaProvider = null;

            if (!string.IsNullOrEmpty(providerName))
            {
                mediaProvider = _services.GetKeyedService<IMediaProvider>(providerName);
            }

            if (mediaProvider is null)
            {
                mediaProvider = _services.GetRequiredKeyedService<IMediaProvider>("db");
            }

            return mediaProvider;
        }
    }
}
