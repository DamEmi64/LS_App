using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Pdf
{
    public class StoryComponent : IComponent
    {
        private readonly StoryModelExtended _storyModel;

        public StoryComponent(StoryModelExtended storyModel)
        {
            _storyModel = storyModel;
        }

        public void Compose(IContainer container)
        {
            container.Column(column =>
            {
                column.Item().Text(_storyModel.Description).FontSize(12);
            });
        }
    }
}
