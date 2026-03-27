using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using RPG.Infrastructure.Models;

namespace RPG.Infrastructure.Pdf
{
    public class PlacesComponent : IComponent
    {
        private PlaceModelExtended _place;

        public PlacesComponent(PlaceModelExtended place)
        {
            _place = place;
        }

        public void Compose(IContainer container)
        {
            if (_place.Image is null)
            {
                container.Column(column =>
                {
                    column.Item().Text(_place.Title);
                    column.Item().Text(_place.Description);
                });
            }
            else
            {
                container.Row(row =>
                {
                    try
                    {
                        var img = Convert.FromBase64String(_place.Image.Replace("data:image/jpeg;base64,", string.Empty));
                        row.RelativeItem().Image(img).FitArea();
                        row.RelativeItem().EnsureSpace(10);
                    }
                    catch (Exception) { }

                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text(_place.Title);
                        column.Item().Text(_place.Description);
                        column.Item().EnsureSpace(20);
                    });
                });
            }
        }
    }
}
