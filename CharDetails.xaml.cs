using System.Windows.Controls;

namespace CKChronicler; 

public partial class CharDetails : Page
{
    private int _charID;
    public CharDetails(int charID)
    {
        _charID = charID;
        InitializeComponent();
    }
    
    private void UpdateCharPreviewText(object sender, TextChangedEventArgs e) {
        App.LoadedSave.GetCharacter(_charID).SetRank(CharRank.Text);
        App.LoadedSave.GetCharacter(_charID).SetName(CharName.Text);
        App.LoadedSave.GetCharacter(_charID).SetDynasty(CharDynasty.Text);
        App.LoadedSave.GetCharacter(_charID).SetPTitle(CharTitle.Text);

        CharPreview.Text = App.LoadedSave.GetCharacter(_charID).GetFullTitle();
    }
}