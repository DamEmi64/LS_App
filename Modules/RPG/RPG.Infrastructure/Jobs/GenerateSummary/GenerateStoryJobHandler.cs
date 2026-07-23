using Base;
using Microsoft.AspNetCore.Mvc.Razor;
using Razor.Templating.Core;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;
using System.Text;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateSummaryJobHandler : JobHandler<GenerateSummaryJob>
    {
        private const string TemplatePath = "/Views/GenTemplate.cshtml";

        private readonly IStoryRepository _storyRepository;
        private readonly IRazorViewEngine _razorViewEngine;
        private readonly IMediaProvider _mediaProvider;

        public GenerateSummaryJobHandler(
            IJobContext jobContext,
            IStoryRepository storyRepository,
            IRazorViewEngine razorViewEngine,
            IMediaProviderFactory mediaProviderFactory)
            : base(jobContext)
        {
            _storyRepository = storyRepository;
            _razorViewEngine = razorViewEngine;
            _mediaProvider = mediaProviderFactory.Create(AppConfiguration.GetValue<string>("DefaultStorage"));
        }

        public override async Task Execute(GenerateSummaryJob request)
        {
            if (request.Story is null)
            {
                request.Story = GetData<StoryModel>() ?? throw new ArgumentNullException();
            }

            var storyModel = await request.Story.ToExtendedModel(_mediaProvider);
            var story = await _storyRepository.Get(request.StoryId);

            if (story is null || storyModel is null)
            {
                return;
            }

            if (request.IsPdf)
            {
                var pdf = new Pdf.PdfGenerator(storyModel).Generate();

                var pdfMedia = await _mediaProvider.Save(pdf, story.Summary);

                if (story.Summary is null || !story.Files.Any(x => x.Title == story.Title))
                {
                    story.Summary = pdfMedia;

                    await _storyRepository.AddFile(story, new Domain.Entities.RPGFile
                    {
                        Title = story.Title,
                        Content = pdfMedia
                    });
                }
                else
                {
                    story.Summary = pdfMedia;
                    await _storyRepository.Update(story);
                }

                return;
            }

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, storyModel);

            var media = await _mediaProvider.Save(
                Encoding.UTF8.GetBytes(html),
                story.Summary,
                "html");

            if (story.Summary is null || !story.Files.Any(x => x.Title == story.Title))
            {
                story.Summary = media;

                await _storyRepository.AddFile(story, new Domain.Entities.RPGFile
                {
                    Title = story.Title,
                    Content = media
                });
            }
            else
            {
                story.Summary = media;
                await _storyRepository.Update(story);
            }
        }
    }
}