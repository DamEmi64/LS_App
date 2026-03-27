using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Pdf
{
    public class HeroComponent : IComponent
    {
        private HeroModelExtended _hero;

        public HeroComponent(HeroModelExtended hero)
        {
            _hero = hero;
        }

        public void Compose(IContainer container)
        {
            if (_hero.Image is null)
            {
                container.Column(column =>
                {
                    column.Item().Text(_hero.FirstName + "" + _hero.LastName);
                    column.Item().Text(_hero.Description);
                });
            }
            else
            {
                container.Row(row =>
                {
                    try
                    {
                        var img = Convert.FromBase64String(_hero.Image.Replace("data:image/jpeg;base64,", string.Empty));
                        row.RelativeItem().MinWidth(4, Unit.Centimetre).MinHeight(4, Unit.Centimetre).Image(img);
                        row.RelativeItem().EnsureSpace(10);
                    }
                    catch (Exception) { }

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text(_hero.FirstName + "" + _hero.LastName);
                        column.Item().Text(_hero.Description);
                        column.Item().EnsureSpace(20);
                    });
                });
            }
        }
    }
}
