using System.Windows.Controls;

namespace CKChronicler; 

public partial class CharDetails : Page {
    public CharDetails() {
        InitializeComponent();
    }
    
    private void UpdateCharPreviewText(object sender, TextChangedEventArgs e) {
        string rank = CharRank.Text.Length > 0 ? CharRank.Text : "";
        string name = CharName.Text.Length > 0 ? " " + CharName.Text : "";
        string title = CharTitle.Text.Length > 0 ? " of " + CharTitle.Text : "";

        CharPreview.Text = $"{rank}{name}{title}";
    }
}