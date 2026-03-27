using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Pdf
{
    public class ChapterComponent : IComponent
    {
        private readonly ChapterModelExtended _chapter;

        public ChapterComponent(ChapterModelExtended chapter)
        {
            _chapter = chapter;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text(_chapter.Title).AlignCenter();
                column.Item().Text(_chapter.Description);
                column.Spacing(10);
                column.Item().Text("Miejsca").AlignCenter();

                foreach (var place in _chapter.Places)
                {
                    column.Item().Border(1, Color.FromRGB(10, 0, 0)).CornerRadius(10).Component(new PlacesComponent(place));
                }

                column.Item().Text("Bohaterowie").AlignCenter();

                foreach (var hero in _chapter.Heroes)
                {
                    column.Item().Border(1, Color.FromRGB(10, 0, 0)).CornerRadius(10).Component(new HeroComponent(hero));
                }
            });
        }
    }
}
