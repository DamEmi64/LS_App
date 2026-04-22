using Base.Entities;
using System.Text.Json;

public static class DictionaryJsonGenerator
{
    public static void Generate(
        IEnumerable<DictionaryItem> entries,
        string outputPath,
        Func<string, string, string> translate // (text, lang) => translatedText
    )
    {
        if (Directory.Exists(outputPath))
            Directory.Delete(outputPath, true);
        Directory.CreateDirectory(outputPath);

        var dictionaries = entries.GroupBy(x => x.Dictionary);

        var dictionaryData = new Dictionary<string, Dictionary<int, string>>();


        foreach (var dictionary in dictionaries)
        {
            var dict = new Dictionary<int, string>();
            foreach (var item in dictionary.OrderBy(x=>x.Key))
            {
                dict.Add(item.Key, item.Name);
            }

            dictionaryData.Add(dictionary.Key.Replace(" ","_"), dict);
        }

        File.WriteAllText(
            Path.Combine(outputPath, "dictionaries.json"),
            JsonSerializer.Serialize(dictionaryData, new JsonSerializerOptions { WriteIndented = true })
        );

        // 2. Languages to generate
        var languages = new[] { "fr", "en", "de", "pl" };

        foreach (var lang in languages)
        {
            var translated = dictionaryData.ToDictionary(
                e => e.Key,
                e => e.Value.ToDictionary(x => x.Key, x => TranslateEntry(entries, e.Key, x.Key, lang, translate)));

            var langFolder = Path.Combine(outputPath, "public", lang);
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                Path.Combine(langFolder, "dictionaries.json"),
                JsonSerializer.Serialize(translated, new JsonSerializerOptions { WriteIndented = true })
            );
        }
    }

    private static object? TranslateEntry(IEnumerable<DictionaryItem> items, string dict, int key, string lang, Func<string, string, string> translate)
    {
        var item = items.FirstOrDefault(x => x.Key == key && x.Dictionary.Replace(" ", "_") == dict);

        if (item is null)
            return null;

        return new
        {
            title = translate(item.Name, lang),
            description = translate(item.Description ?? string.Empty, lang)
        };
    }
}