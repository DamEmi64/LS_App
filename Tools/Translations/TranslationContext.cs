using Base;
using CommunityToolkit.Mvvm.Input;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Text;
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
        public ICommand SendToServer => new RelayCommand(SendDictionariesToServer);
        public ICommand DownloadFromServer => new RelayCommand(DownloadTranslationsFromServer);
        public ICommand Load => new RelayCommand(LoadTranslationsFromFile);
        public ICommand MultiAdd => new RelayCommand(MultiAddTranslation);
        public ICommand MultiAddDict => new RelayCommand(MultiAddTranslationDict);

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

        private void SendDictionariesToServer()
        {
            try
            {
                var output = SelectOutputFolder();
                if (output is null)
                    return;

                GenerateTranslations(output);
                GenerateTranslations(output);
                GenerateDictionaries(output);

                var projectRoot = FindProjectRoot(AppContext.BaseDirectory);
                var clientDirectory = Path.Combine(projectRoot, "v2.client");
                var startInfo = new ProcessStartInfo("cmd.exe")
                {
                    WorkingDirectory = clientDirectory,
                    UseShellExecute = false,
                };
                startInfo.ArgumentList.Add("/c");
                startInfo.ArgumentList.Add("npm");
                startInfo.ArgumentList.Add("run");
                startInfo.ArgumentList.Add("firebase:sync-content");
                startInfo.ArgumentList.Add("--");
                startInfo.ArgumentList.Add("--dictionaries");
                startInfo.ArgumentList.Add(output);

                using var process = Process.Start(startInfo)
                    ?? throw new InvalidOperationException("Failed to start the Firebase seeding script.");
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new InvalidOperationException($"Firebase seeding failed with exit code {process.ExitCode}.");

                MessageBox.Show("Translations sent to server.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DownloadTranslationsFromServer()
        {
            try
            {
                var output = SelectOutputFolder();
                if (output is null)
                    return;

                var projectRoot = FindProjectRoot(AppContext.BaseDirectory);
                var clientDirectory = Path.Combine(projectRoot, "v2.client");
                var startInfo = new ProcessStartInfo("cmd.exe")
                {
                    WorkingDirectory = clientDirectory,
                    UseShellExecute = false,
                };
                startInfo.ArgumentList.Add("/c");
                startInfo.ArgumentList.Add("npm");
                startInfo.ArgumentList.Add("run");
                startInfo.ArgumentList.Add("firebase:download-content");
                startInfo.ArgumentList.Add("--");
                startInfo.ArgumentList.Add("--output");
                startInfo.ArgumentList.Add(output);

                using var process = Process.Start(startInfo)
                    ?? throw new InvalidOperationException("Failed to start the Firebase download script.");
                process.WaitForExit();
                if (process.ExitCode != 0)
                    throw new InvalidOperationException($"Firebase download failed with exit code {process.ExitCode}.");

                MessageBox.Show("Translations downloaded from server.");

                LoadTranslationsFromFiles(Directory.GetFiles(output,"*",SearchOption.AllDirectories));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private static string? SelectOutputFolder()
        {
            var dialog = new OpenFileDialog
            {
                CheckFileExists = false,
                CheckPathExists = true,
                FileName = "Select folder",
                Filter = "Folders|*.this.directory"
            };

            return dialog.ShowDialog() == true ? Path.GetDirectoryName(dialog.FileName) : null;
        }

        private static string FindProjectRoot(string startDirectory)
        {
            var directory = new DirectoryInfo(startDirectory);
            while (directory is not null && !File.Exists(Path.Combine(directory.FullName, "V2.sln")))
                directory = directory.Parent;

            return directory?.FullName
                ?? throw new DirectoryNotFoundException("Could not find the solution root (V2.sln).");
        }

        public void LoadTranslationsFromFile()
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select files",
                Multiselect = true,
                Filter = "All files (*.*)|*.*"
            };

            var result = dialog.ShowDialog();
            if (result == true)
            {
                LoadTranslationsFromFiles(dialog.FileNames);
            }

            MessageBox.Show("Loaded");
        }

        private void LoadTranslationsFromFiles(string[] selectedFiles)
        {
            var dictionariesLoaded = false;
            foreach (var file in selectedFiles)
            {
                if (Path.GetFileName(file).Equals("dictionaries.json", StringComparison.OrdinalIgnoreCase))
                {
                    var language = Path.GetFileName(Path.GetDirectoryName(file));
                    LoadDictionaryData(file, language is PL or EN or FR or DE ? language : null);
                    dictionariesLoaded = true;
                    continue;
                }

                if (file.EndsWith("pl.json") || file.EndsWith("\\pl\\translation.json"))
                {
                    LoadDataToTranslation(file, PL);
                }

                if (file.EndsWith("en.json") || file.EndsWith("\\en\\translation.json"))
                {
                    LoadDataToTranslation(file, EN);
                }

                if (file.EndsWith("de.json") || file.EndsWith("\\de\\translation.json"))
                {
                    LoadDataToTranslation(file, DE);
                }

                if (file.EndsWith("fr.json") || file.EndsWith("\\fr\\translation.json"))
                {
                    LoadDataToTranslation(file, FR);
                }
            }

            if (dictionariesLoaded)
            {
                var rows = Dictionaries.ToList();
                Dictionaries.Clear();
                foreach (var row in rows)
                    Dictionaries.Add(row);
            }
        }

        private void LoadDictionaryData(string filePath, string? language)
        {
            var data = JObject.Parse(File.ReadAllText(filePath));
            foreach (var dictionary in data.Properties())
            {
                var dictionaryName = dictionary.Name.Replace('_', ' ');
                if (dictionary.Value is not JObject items)
                    continue;

                foreach (var item in items.Properties())
                {
                    if (!int.TryParse(item.Name, out var key))
                        continue;

                    var row = Dictionaries.FirstOrDefault(x => x.Key == key &&
                        string.Equals(x.Dictionary.Replace('_', ' '), dictionaryName, StringComparison.OrdinalIgnoreCase));
                    if (row is null)
                    {
                        row = new DictionaryDto { Key = key, Dictionary = dictionaryName };
                        Dictionaries.Add(row);
                    }

                    if (language is null && item.Value.Type == JTokenType.String)
                    {
                        row.Dictionary = dictionaryName;
                        row.TitleEN ??= item.Value.Value<string>();
                        continue;
                    }

                    if (language is null || item.Value is not JObject translatedItem)
                        continue;

                    var title = translatedItem.Value<string>("title");
                    var description = translatedItem.Value<string>("description");
                    switch (language)
                    {
                        case PL:
                            row.TitlePL = title;
                            row.DescriptionPL = description;
                            break;
                        case EN:
                            row.TitleEN = title;
                            row.DescriptionEN = description;
                            break;
                        case FR:
                            row.TitleFR = title;
                            row.DescriptionFR = description;
                            break;
                        case DE:
                            row.TitleDE = title;
                            row.DescriptionDE = description;
                            break;
                    }
                }
            }
        }

        private void MultiAddTranslation()
        {
            var multiAddWindow = new MultiAddTranslationWindow();

            multiAddWindow.ShowDialog();

            if (multiAddWindow.TranslateStr is null)
                return;

            var translations = multiAddWindow.TranslateStr.Split("\n");

            foreach (var translationStr in translations)
            {
                var translateArray = translationStr.Split(";");
                Translations.Add(new TranslationDto
                {
                    Key = translateArray[0],
                    PL = translateArray[1],
                    EN = translateArray[2],
                    FR = translateArray[3],
                    DE = translateArray[4]
                });
            }

            MessageBox.Show("Loaded");
        }

        private void MultiAddTranslationDict()
        {
            var multiAddWindow = new MultiAddTranslationWindow();

            multiAddWindow.ShowDialog();

            if (multiAddWindow.TranslateStr is null)
                return;

            var translations = multiAddWindow.TranslateStr.Split("\n");

            foreach (var translationStr in translations)
            {
                var translateArray = translationStr.Split(";");

                var dict = Dictionaries.FirstOrDefault(x => x.Key.ToString() == translateArray[0]);

                if (dict is not null)
                {
                    dict.TitlePL = translateArray[1];
                    dict.TitleEN = translateArray[2];
                    dict.TitleFR = translateArray[3];
                    dict.TitleDE = translateArray[4];
                }
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
                JsonConvert.SerializeObject(dictionaryData, Formatting.Indented)
            );

            if (Dictionaries is null)
                return;

            var dict2 = Dictionaries.GroupBy(x => x.Dictionary!);

            var translatedEn = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleEN, description = x.DescriptionEN ?? string.Empty }));

            var translatedPl = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitlePL, description = x.DescriptionPL ?? string.Empty }));

            var translatedDe = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleDE, description = x.DescriptionDE ?? string.Empty }));

            var translatedFr = dict2.ToDictionary(
                   e => e.Key.Replace(" ", "_"),
                   e => e.ToDictionary(x => x.Key, x => new { title = x.TitleFR, description = x.DescriptionFR ?? string.Empty }));

            Save(output, "en", "dictionaries.json", translatedEn);
            Save(output, "pl", "dictionaries.json", translatedPl);
            Save(output, "de", "dictionaries.json", translatedDe);
            Save(output, "fr", "dictionaries.json", translatedFr);

            File.WriteAllText("dictionaries.json", JsonConvert.SerializeObject(Dictionaries, Formatting.Indented));
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

            File.WriteAllText("translations.json", JsonConvert.SerializeObject(Translations, Formatting.Indented));
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
                JsonConvert.SerializeObject(data, Formatting.Indented)); ;
        }

    }
}
