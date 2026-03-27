using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using RPG.Infrastructure.Models;


namespace RPG.Infrastructure.Pdf
{
    public class PdfGenerator
    {
        private readonly StoryModelExtended _storyModel;

        public PdfGenerator(StoryModelExtended storyModel)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            _storyModel = storyModel;
        }

        public byte[] Generate()
        {
            var backgroundImg = ImageLoader.GetImageBytes("background.jpg");

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);

                    if (backgroundImg is not null)
                    {
                        page.Background().Image(backgroundImg).FitUnproportionally();
                    }

                    page.Header().AlignCenter().Text(_storyModel.Title).FontSize(10).SemiBold().Underline();

                    page.Content().Column(column =>
                    {
                        column.Item().Text(_storyModel.Title).FontSize(40).FontFamily("Times New Roman");
                        column.Item().PageBreak();

                        column.Item().Component(new StoryComponent(_storyModel));
                        column.Item().EnsureSpace(150);

                        foreach (var chapter in _storyModel.Chapters)
                        {
                            column.Item().Component(new ChapterComponent(chapter));
                            column.Item().PageBreak();

                        }
                    });

                });
            });

            byte[] pdfBytes;

            using (var stream = new MemoryStream())
            {
                document.GeneratePdf(stream);
                pdfBytes = stream.ToArray();
            }

            return pdfBytes;
        }
    }
}
