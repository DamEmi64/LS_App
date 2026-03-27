
namespace RPG.Infrastructure.Pdf;

public static class ImageLoader
{
    public static byte[]? GetImageBytes(string resourceName)
    {
        try
        {
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName?.Contains("Extras") ?? false);

            if (assembly is null)
                return null;

            var fullName = assembly
                .GetManifestResourceNames()
                .FirstOrDefault(n => n.EndsWith(resourceName, StringComparison.OrdinalIgnoreCase));

            if (fullName == null) 
                return null;

            using (var stream = assembly.GetManifestResourceStream(fullName))
            {
                if (stream is null)
                    return null;

                using (MemoryStream ms = new())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }
        catch
        {
            return null;
        }

    }
}