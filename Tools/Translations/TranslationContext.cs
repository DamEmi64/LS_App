using Base;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Translations.Dtos;

namespace Translations
{
    public class TranslationContext
    {
        private const string PL = "pl";
        private const string EN = "en";
        private const string FR = "fr";
        private const string DE = "de";

        private const string ConnString = @"Server=(localdb)\MSSQLLocalDB;Database=AppContext;Trusted_Connection=True;MultipleActiveResultSets=true";

        public TranslationContext()
        {
            LoadDictionaries();
            LoadTranslations();
        }

        public List<DictionaryItem> DbDictionaries { get; set; } = [];

        public ObservableCollection<DictionaryDto> Dictionaries { get; set; } = [];

        public ObservableCollection<TranslationDto> Translations { get; set; } = [];


        public ICommand Generate => new RelayCommand(GenerateData);
        public ICommand Load => new RelayCommand(LoadTranslationsFromFile);

        public void GenerateData()
        {
            try
            {
                var dialog = new OpenFileDialog()
                {
                    CheckFileExists = false,
                    CheckPathExists = true,
                    FileName = "Select folder",
                    Filter = "Folders|*.this.directory"
                };

                if (dialog.ShowDialog() == true)
                {
                    string selectedPath = dialog.FileName;
                    GenerateTranslations(Path.GetDirectoryName(selectedPath) ?? throw new NullReferenceException());
                    GenerateDictionaries(Path.GetDirectoryName(selectedPath) ?? throw new NullReferenceException());
                    MessageBox.Show("Saved");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void LoadTranslationsFromFile()
        {
            var dialog = new LoadFromFile();
            dialog.ShowDialog();

            if (!string.IsNullOrEmpty(dialog.ENPath))
            {
                LoadDataToTranslation(dialog.ENPath, EN);
            }

            if (!string.IsNullOrEmpty(dialog.PLPath))
            {
                LoadDataToTranslation(dialog.PLPath, PL);
            }

            if (!string.IsNullOrEmpty(dialog.FRPath))
            {
                LoadDataToTranslation(dialog.FRPath, FR);
            }

            if (!string.IsNullOrEmpty(dialog.DEPath))
            {
                LoadDataToTranslation(dialog.DEPath, DE);
            }

            MessageBox.Show("Loaded");
        }

        private void LoadDataToTranslation(string filePath, string lang)
        {
            var dict = FlattenJsonFromFile(filePath);

            foreach (var item in dict)
            {
                var translationItem = Translations.FirstOrDefault(x => x.Key == item.Key);

                if (translationItem is null)
                {
                    Translations.Add(new TranslationDto
                    {
                        Key = item.Key,
                        PL = lang == PL ? item.Value : null,
                        EN = lang == EN ? item.Value : null,
                        DE = lang == DE ? item.Value : null,
                        FR = lang == FR ? item.Value : null,
                    });
                }
                else
                {
                    switch (lang)
                    {
                        case "pl": translationItem.PL = item.Value ?? string.Empty; break;
                        case "en": translationItem.EN = item.Value ?? string.Empty; break;
                        case "fr": translationItem.FR = item.Value ?? string.Empty; break;
                        case "de": translationItem.DE = item.Value ?? string.Empty; break;
                    }
                }
            }
        }

        private void LoadDictionaries()
        {
            var options = new DbContextOptionsBuilder<DbContext>()
                            .UseSqlServer(ConnString)
                            .Options;
            var context = new DbContext(options);
            DbDictionaries = context.Dictionaries.AsNoTracking().ToList();

            var dictionaries = new List<DictionaryDto>();
            var buf = new List<DictionaryDto>();

            if (File.Exists("dictionaries.json"))
            {
                var json = File.ReadAllText("dictionaries.json");
                buf = JsonConvert.DeserializeObject<List<DictionaryDto>>(json) ?? new List<DictionaryDto>();
            }

            foreach (var item in DbDictionaries)
            {
                var dbItem = buf.FirstOrDefault(x => x.Key == item.Key);
                if (dbItem is not null)
                {
                    dictionaries.Add(dbItem);
                }
                else
                {
                    dictionaries.Add(new DictionaryDto
                    {
                        Key = item.Key,
                        Dictionary = item.Dictionary
                    });
                }
            }

            Dictionaries = new ObservableCollection<DictionaryDto>(dictionaries.OrderBy(x => x.Dictionary).ThenBy(x => x.Key));
        }

        private void GenerateDictionaries(string output)
        {
            Directory.CreateDirectory(output);

            var dictionaries = DbDictionaries.GroupBy(x => x.Dictionary);

            var dictionaryData = new Dictionary<string, Dictionary<int, string>>();

            foreach (var dictionary in dictionaries)
            {
                var dict = new Dictionary<int, string>();
                foreach (var item in dictionary.OrderBy(x => x.Key))
                {
                    dict.Add(item.Key, item.Name);
                }

                dictionaryData.Add(dictionary.Key.Replace(" ", "_"), dict);
            }

            File.WriteAllText(
                System.IO.Path.Combine(output, "dictionaries.json"),
                JsonConvert.SerializeObject(dictionaryData)
            );

            if (Dictionaries is null)
                return;

            var dict2 = Dictionaries.GroupBy(x => x.Dictionary!);

            var translatedEn = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleEN, description = x.DescriptionEN }));

            var translatedPl = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitlePL, description = x.DescriptionPL }));

            var translatedDe = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleDE, description = x.DescriptionDE }));

            var translatedFr = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleFR, description = x.DescriptionFR }));

            Save(output, "en", "dictionaries.json", translatedEn);
            Save(output, "pl", "dictionaries.json", translatedPl);
            Save(output, "de", "dictionaries.json", translatedDe);
            Save(output, "fr", "dictionaries.json", translatedFr);

            File.WriteAllText("dictionaries.json", JsonConvert.SerializeObject(Dictionaries));
        }

        private void LoadTranslations()
        {
            var dictionaries = new List<TranslationDto>();
            var buf = new List<TranslationDto>();

            if (File.Exists("translations.json"))
            {
                var json = File.ReadAllText("translations.json");
                Translations = new ObservableCollection<TranslationDto>(JsonConvert.DeserializeObject<List<TranslationDto>>(json) ?? new List<TranslationDto>());
            }
        }

        private Dictionary<string, string> FlattenJsonFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                MessageBox.Show("File not exist");
                return new();
            }

            var json = File.ReadAllText(filePath);
            var token = JToken.Parse(json);

            var result = new Dictionary<string, string>();
            FlattenToken(token, result, "");

            return result;
        }

        private void FlattenToken(JToken token, Dictionary<string, string> result, string prefix)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    foreach (var property in token.Children<JProperty>())
                    {
                        var newPrefix = string.IsNullOrEmpty(prefix)
                            ? property.Name
                            : $"{prefix}.{property.Name}";

                        FlattenToken(property.Value, result, newPrefix);
                    }
                    break;

                case JTokenType.Array:
                    int index = 0;
                    foreach (var item in token.Children())
                    {
                        var newPrefix = $"{prefix}[{index}]";
                        FlattenToken(item, result, newPrefix);
                        index++;
                    }
                    break;

                default:
                    result[prefix] = token.ToString();
                    break;
            }
        }

        private void GenerateTranslations(string output)
        {
            Directory.CreateDirectory(output);
            GenerateTranslationObject(output, "pl");
            GenerateTranslationObject(output, "fr");
            GenerateTranslationObject(output, "de");
            GenerateTranslationObject(output, "en");

            File.WriteAllText("translations.json", JsonConvert.SerializeObject(Translations));
        }

        private void GenerateTranslationObject(string output, string lang)
        {
            var root = new Dictionary<string, object>();

            foreach (var langTranslation in Translations?.Where(x => !string.IsNullOrEmpty(x.Key)) ?? Array.Empty<TranslationDto>())
            {
                if (langTranslation is null)
                    continue;

                var parts = langTranslation.Key.Split('.');

                var current = root;
                var key = parts[0];
                for (int i = 0; i < parts.Length; i++)
                {
                    key = parts[i];

                    if (i == parts.Length - 1)
                    {
                        continue;
                    }
                    else
                    {
                        if (current.TryGetValue(key, out var value) && value is Dictionary<string, object> d)
                        {
                            current = d;
                        }
                        else
                        {
                            var next = new Dictionary<string, object>();
                            current[key] = next;
                            current = next;
                        }
                    }
                }

                switch (lang)
                {
                    case "pl": current[key] = langTranslation.PL ?? string.Empty; break;
                    case "en": current[key] = langTranslation.EN ?? string.Empty; break;
                    case "fr": current[key] = langTranslation.FR ?? string.Empty; break;
                    case "de": current[key] = langTranslation.DE ?? string.Empty; break;
                }
            }

            Save(output, lang, "translation.json", root);
        }

        private void Save(string output, string lang, string title, object data)
        {
            var langFolder = System.IO.Path.Combine(output, lang);
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                System.IO.Path.Combine(langFolder, title),
                JsonConvert.SerializeObject(data));
        }

    }
}
