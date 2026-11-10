using PdfPecker;

namespace Base.Helpers;

public static class HtmlPdfRenderer
{
    public static Task<byte[]> RenderAsync(string html, float marginMillimeters = 20)
    {
        var options = new PdfOptions
        {
            PageSize = PageSize.A4,
            Margins = PageMargins.FromMillimeters(marginMillimeters, marginMillimeters, marginMillimeters, marginMillimeters)
        };

        return HtmlToPdf.ConvertAsync(html, options);
    }
}
