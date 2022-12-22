using System.Windows;
using System.Windows.Controls;

namespace CKChronicler;

public partial class CreateSave : Page
{
    private readonly MainWindow _parentWindow;
    
    public CreateSave(MainWindow pw)
    {
        _parentWindow = pw;
        InitializeComponent();
    }
    
    private void ReturnButton_OnClick(object sender, RoutedEventArgs e) {
        _parentWindow.ShowMainMenu();
    }

    private void CreateSave_OnClick(object sender, RoutedEventArgs e)
    {
        string saveName = SaveNameBox.Text;
        App.LoadedSave = new Save(saveName);
        
        int firstChar = App.LoadedSave.AddCharacter();
        App.AppWindow.SetContent(new CreationPage(firstChar, "Initial Ruler", this, new MainMenu()));
    }
}