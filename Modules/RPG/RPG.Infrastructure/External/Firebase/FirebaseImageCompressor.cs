using SkiaSharp;

namespace RPG.Infrastructure.External.Firebase
{
    internal static class FirebaseImageCompressor
    {
        private const int MaxDimension = 1280;
        private const int WebpQuality = 78;
        private const string WebpDataUriPrefix = "data:image/webp;base64,";

        public static string Compress(string content)
        {
            try
            {
                var base64 = content;
                var commaIndex = content.IndexOf(',');
                if (content.StartsWith("data:", StringComparison.OrdinalIgnoreCase) && commaIndex >= 0)
                    base64 = content[(commaIndex + 1)..];

                var sourceBytes = Convert.FromBase64String(base64);
                using var source = SKBitmap.Decode(sourceBytes);
                if (source is null)
                    return content;

                var width = source.Width;
                var height = source.Height;
                if (width > MaxDimension || height > MaxDimension)
                {
                    var scale = Math.Min(
                        (double)MaxDimension / width,
                        (double)MaxDimension / height);
                    width = Math.Max(1, (int)Math.Round(width * scale));
                    height = Math.Max(1, (int)Math.Round(height * scale));
                }

                using var resized = new SKBitmap(new SKImageInfo(width, height, source.ColorType, source.AlphaType));
                using var sourceImage = SKImage.FromBitmap(source);
                using (var canvas = new SKCanvas(resized))
                {
                    canvas.DrawImage(
                        sourceImage,
                        new SKRect(0, 0, source.Width, source.Height),
                        new SKRect(0, 0, width, height),
                        new SKSamplingOptions(SKFilterMode.Linear, SKMipmapMode.Linear));
                    canvas.Flush();
                }

                using var image = SKImage.FromBitmap(resized);
                using var encoded = image.Encode(SKEncodedImageFormat.Webp, WebpQuality);
                if (encoded is null)
                    return content;

                if (encoded.Size >= sourceBytes.Length)
                    return content;

                return WebpDataUriPrefix + Convert.ToBase64String(encoded.ToArray());
            }
            catch
            {
                // Keep unsupported or malformed image content exportable as before.
                return content;
            }
        }
    }
}
