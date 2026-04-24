using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
