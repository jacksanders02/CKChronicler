using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKChronicler; 

public partial class CharDetails : Page
{
    private readonly int _charID;
    public CharDetails(int charID)
    {
        _charID = charID;
        InitializeComponent();
    }
    
    private void UpdateCharPreviewText(object sender, TextChangedEventArgs e) {
        App.LoadedSave.GetCharacter(_charID).Rank = CharRank.Text;
        App.LoadedSave.GetCharacter(_charID).Name = CharName.Text;
        App.LoadedSave.GetCharacter(_charID).Dynasty = CharDynasty.Text;
        App.LoadedSave.GetCharacter(_charID).PTitle = CharTitle.Text;

        CharPreview.Text = App.LoadedSave.GetCharacter(_charID).GetFullTitle();
    }
    
    private void IsNumberInput(object sender, TextCompositionEventArgs e) {
        e.Handled =  !InputValidation.IsNumeric(e.Text);
    }
    
    private void IsNumberPasted(object sender, DataObjectPastingEventArgs e) {
        if (e.DataObject.GetDataPresent(typeof(string))) {
            string text = (string) e.DataObject.GetData(typeof(string));
            if (!InputValidation.IsNumeric(text)) {
                e.CancelCommand();
            }
        }
        else {
            e.CancelCommand();
        }
    }

    public void SaveCharDetails()
    {
        App.LoadedSave.GetCharacter(_charID).Rank = CharRank.Text;
        App.LoadedSave.GetCharacter(_charID).Name = CharName.Text;
        App.LoadedSave.GetCharacter(_charID).Dynasty = CharDynasty.Text;
        App.LoadedSave.GetCharacter(_charID).PTitle = CharTitle.Text;
        App.LoadedSave.GetCharacter(_charID).Religion = CharFaith.Text;
        App.LoadedSave.GetCharacter(_charID).Culture = CharCulture.Text;
        
        App.LoadedSave.GetCharacter(_charID).Diplo = int.Parse(Diplo.Text);
        App.LoadedSave.GetCharacter(_charID).Martial = int.Parse(Martial.Text);
        App.LoadedSave.GetCharacter(_charID).Steward = int.Parse(Steward.Text);
        App.LoadedSave.GetCharacter(_charID).Intrigue = int.Parse(Intrigue.Text);
        App.LoadedSave.GetCharacter(_charID).Learning = int.Parse(Learning.Text);
        App.LoadedSave.GetCharacter(_charID).Prowess = int.Parse(Prowess.Text);
    }
}