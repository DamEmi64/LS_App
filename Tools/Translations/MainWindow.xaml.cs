using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Translations
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DictionaryContext _context;
        private const string ConnString = @"Server=(localdb)\MSSQLLocalDB;Database=AppContext-dev;Trusted_Connection=True;MultipleActiveResultSets=true";

        public MainWindow()
        {
            InitializeComponent();

            var options = new DbContextOptionsBuilder<DictionaryContext>()
                .UseSqlServer(ConnString)
                .Options;
            _context = new DictionaryContext(options);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new Dictionaries(_context);
            dialog.Show();
        }
    }
}