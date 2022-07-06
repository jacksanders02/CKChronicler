using System;
using System.Printing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKChronicler
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void ShowMainMenu()
        {
            Main.Content = new CKChronicler.MainMenu();
        }

        public void ShowCreatePage()
        {
            Main.Content = new CKChronicler.CreationPage();
        }
    }
}