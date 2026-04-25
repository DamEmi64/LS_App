using System.Windows;

namespace Translations
{
    /// <summary>
    /// Interaction logic for LoadFromFile.xaml
    /// </summary>
    public partial class LoadFromFile : Window
    {
        public LoadFromFile()
        {
            InitializeComponent();
        }

        public string? PLPath { get; set; }
        public string? ENPath { get; set; }
        public string? DEPath { get; set; }
        public string? FRPath { get; set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PLPath = plTb.Text;
            ENPath = enTb.Text;
            DEPath = deTb.Text;
            FRPath = frTb.Text;
            Close();
        }
    }
}
