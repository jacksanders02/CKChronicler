using System;
using System.Windows;

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

        private void StartBtn_OnClick(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hello World!");
        }
    }
}