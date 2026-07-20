using System.Windows;

namespace Translations
{
    /// <summary>
    /// Interaction logic for MultiAddTranslationWindow.xaml
    /// </summary>
    public partial class MultiAddTranslationWindow : Window
    {
        public MultiAddTranslationWindow()
        {
            InitializeComponent();
        }

        public string? TranslateStr { get; private set; }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            TranslateStr = translationTb.Text;
            Close();
        }
    }
}
