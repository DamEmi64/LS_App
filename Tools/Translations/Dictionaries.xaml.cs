using Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Windows;
using Translations.Dtos;

namespace Translations
{
    /// <summary>
    /// Interaction logic for Dictionaries.xaml
    /// </summary>
    public partial class Dictionaries : Window
    {
        private readonly List<DictionaryItem> _dbDictionaries;

        public Dictionaries(DictionaryContext context)
        {
            InitializeComponent();
            _dbDictionaries = context.Dictionaries.AsNoTracking().ToList();
            SetItemSource();
        }

        private void SetItemSource()
        {
            var dictionaries = new List<DictionaryDto>();
            var buf = new List<DictionaryDto>();

            if (File.Exists("dictionaries.json"))
            {
                var json = File.ReadAllText("dictionaries.json");
                buf = JsonConvert.DeserializeObject<List<DictionaryDto>>(json) ?? new List<DictionaryDto>();
            }

            foreach (var item in _dbDictionaries)
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

            datatable.ItemsSource = dictionaries.OrderBy(x=>x.Dictionary).ThenBy(x=>x.Key).ToList();
        }

        private void Generate(string output)
        {
            if (Directory.Exists(output))
                Directory.Delete(output, true);
            Directory.CreateDirectory(output);

            var dictionaries = _dbDictionaries.GroupBy(x => x.Dictionary);

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

            var translations = datatable.ItemsSource as IEnumerable<DictionaryDto>;

            if (translations is null)
                return;

            var dict2 = translations.GroupBy(x => x.Dictionary!);

            var translatedEn = dict2.ToDictionary(
                   e => e.Key.Replace(" ","_"),
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
            
            var langFolder = System.IO.Path.Combine(output, "public", "pl");
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                System.IO.Path.Combine(langFolder, "dictionaries.json"),
                JsonConvert.SerializeObject(translatedPl)
            );

            langFolder = System.IO.Path.Combine(output, "public", "en");
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                System.IO.Path.Combine(langFolder, "dictionaries.json"),
                JsonConvert.SerializeObject(translatedEn)
            );

            langFolder = System.IO.Path.Combine(output, "public", "de");
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                System.IO.Path.Combine(langFolder, "dictionaries.json"),
                JsonConvert.SerializeObject(translatedDe)
            );

            langFolder = System.IO.Path.Combine(output, "public", "fr");
            Directory.CreateDirectory(langFolder);

            File.WriteAllText(
                System.IO.Path.Combine(langFolder, "dictionaries.json"),
                JsonConvert.SerializeObject(translatedFr)
            );

            File.WriteAllText("dictionaries.json", JsonConvert.SerializeObject(translations));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
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
                Generate(Path.GetDirectoryName(selectedPath) ?? throw new NullReferenceException());
            }

        }
    }
}
