using System.Windows.Controls;

namespace CKChronicler; 

public partial class CharDetails : Page {
    public CharDetails() {
        InitializeComponent();
    }
    
    private void UpdateCharPreviewText(object sender, TextChangedEventArgs e) {
        App.CurrentChar.SetRank(CharRank.Text);
        App.CurrentChar.SetName(CharName.Text);
        App.CurrentChar.SetPTitle(CharTitle.Text);

        CharPreview.Text = App.CurrentChar.GetFullTitle();
    }
}