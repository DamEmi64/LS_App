using Base;
using Microsoft.AspNetCore.Mvc.Razor;
using Razor.Templating.Core;
using RPG.Domain.Repositories;
using RPG.Infrastructure.Models;
using System.Text;

namespace RPG.Infrastructure.Jobs
{
    public class GenerateSummaryJob : IJob
    {
        private const string TemplatePath = "/Views/GenTemplate.cshtml";

        public Guid Id { get; set; } = Guid.NewGuid();

        public List<IJob> Children { get; set; } = new();

        public DateTimeOffset RequestDate => DateTimeOffset.Now;

        public string Name => $"Generate summary for {Story?.Title ?? StoryId.ToString()}";

        public Guid StoryId { get; set; }

        public StoryModel? Story { get; set; }

        public bool IsPdf { get; set; }

        public int OperationId => Domain.Dictionaries.Operations.GenerateSummary;

        public async Task Execute(IJobContext jobContext)
        {
            if (Story is null)
            {
                Story = jobContext.GetData<StoryModel>() ?? throw new ArgumentNullException();
            }

            var storyRepo = jobContext.Resolve<IStoryRepository>();
            var razorViewEngine = jobContext.Resolve<IRazorViewEngine>();
            var mediaProvider = jobContext.Resolve<IMediaProvider>();
            await ExecuteInternal(storyRepo, mediaProvider);
        }

        private async Task ExecuteInternal(IStoryRepository storyRepository, IMediaProvider mediaProvider)
        {
            var storyModel = await (Story ?? throw new InvalidOperationException()).ToExtendedModel(mediaProvider);
            var story = await storyRepository.Get(StoryId);
            if (story is null || storyModel is null)
            {
                return;
            }

            if (IsPdf)
            {
                var pdf = new Pdf.PdfGenerator(storyModel).Generate();

                var pdfMedia = await mediaProvider.Save(pdf, story.Summary);
                story.Summary = pdfMedia;
                await storyRepository.Update(story);
                return;
            }

            var html = await RazorTemplateEngine.RenderAsync(TemplatePath, storyModel);
            var media = await mediaProvider.Save(Encoding.UTF8.GetBytes(html), story.Summary, "html");
            story.Summary = media;
            await storyRepository.Update(story);
        }
    }
}