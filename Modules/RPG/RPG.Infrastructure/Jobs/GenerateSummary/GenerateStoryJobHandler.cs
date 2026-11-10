using Base;
using Microsoft.AspNetCore.Mvc.Razor;
using Razor.Templating.Core;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;
using System.Text;
using Base.Helpers;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateSummaryJobHandler : JobHandler<GenerateSummaryJob>
    {
        private const string TemplatePath = "/Views/GenTemplate.cshtml";
        private const string PdfTemplatePath = "/Views/PdfTemplate.cshtml";

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
                var pdfHtml = await RazorTemplateEngine.RenderAsync(PdfTemplatePath, new StoryPdfTemplateModel
                {
                    Story = storyModel,
                    BackgroundImage = GetPdfBackgroundDataUri()
                });
                var pdf = await HtmlPdfRenderer.RenderAsync(pdfHtml, 1);

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

        private static string GetPdfBackgroundDataUri()
        {
            var extrasAssembly = System.Reflection.Assembly.Load("RPG.Extras");
            using var background = extrasAssembly.GetManifestResourceStream("RPG.Extras.Resources.Background.jpg")
                ?? throw new InvalidOperationException("RPG PDF background resource was not found.");
            using var buffer = new MemoryStream();
            background.CopyTo(buffer);
            return $"data:image/jpeg;base64,{Convert.ToBase64String(buffer.ToArray())}";
        }

    }
}
