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

        public IMediaProviderWrapper Create(string? providerName = null)
        {
            IMediaProvider? mediaProvider = null;
            var notifier = _services.GetRequiredService<Notifier>();

            if (!string.IsNullOrEmpty(providerName))
            {
                mediaProvider = _services.GetKeyedService<IMediaProvider>(providerName);
            }

            if (mediaProvider is null)
            {
                mediaProvider = _services.GetRequiredKeyedService<IMediaProvider>(AppConfiguration.GetValue<string>("DefaultStorage"));
            }

            return new MediaProviderWrapper(mediaProvider, notifier);
        }
    }
}
