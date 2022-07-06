using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CKChronicler;

public partial class MainMenu : Page
{
    public MainMenu()
    {
        InitializeComponent();
    }

    private void StartBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow != null)
        {
            ((CKChronicler.MainWindow)Application.Current.MainWindow).ShowCreatePage();
        }
        else
        {
            throw new NoNullAllowedException();
        }
    }
}