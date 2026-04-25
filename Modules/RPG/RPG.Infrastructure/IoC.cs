using Base.Automation;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Dictionaries;
using RPG.Domain.Repositories;
using RPG.Infrastructure.External.FileConverters;
using RPG.Infrastructure.External.FileConverters.Firebase;
using RPG.Infrastructure.External.FileConverters.Json;
using RPG.Infrastructure.Repositories;
using RPG.Infrastructure.Services;
using RPG.Infrastructure.Services.SummaryService;

namespace RPG.Infrastructure
{
    public static class IoC
    {
        public static IServiceCollection AddRepos(this IServiceCollection services)
        {
            return services.AddScoped<IHeroRepository, HeroRepository>()
                .AddScoped<IPlaceRepository, PlaceRepository>()
                .AddScoped<IChapterRepository, ChapterRepository>()
                .AddScoped<IStoryRepository, StoryRepository>();
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddKeyedScoped<IRPGDataConverter, OldJsonConverter>(RPGFileTypes.OldJson)
                     .AddKeyedScoped<IRPGDataConverter, FirebaseConverter>(RPGFileTypes.Firebase)
                     .AddKeyedScoped<IRPGDataConverter, JsonConverter>(RPGFileTypes.Json)
                     .AddScoped<IJsonConverter, JsonConverter>();

            return services.AddScoped<IAutomationResolver, RPGAutomationResolver>()
                            .AddScoped<ISummaryService, SummaryService>()
                            .AddScoped<IImportService, ImportService>();
        }
    }
}