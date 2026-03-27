using Base;
using Microsoft.Extensions.DependencyInjection;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Jobs;
using RPG.Infrastructure.Repositories;
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
            services.AddAutomationJob<GetLastEditedRPGJob>()
                .AddAutomationJob<GenerateSummaryJob>();


            return services.AddScoped<ISummaryService, SummaryService>();
        }
    }
}