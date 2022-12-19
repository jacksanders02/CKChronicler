using System.Collections;
using System.Windows.Controls;

namespace CKChronicler; 

public partial class CharDetails : Page {
    private string _rank = "";
    private string _name = "";
    private string _title = "";
    
    public CharDetails() {
        InitializeComponent();
    }
    
    private void UpdateCharPreviewText(object sender, TextChangedEventArgs e) {
        _rank = CharRank.Text;
        _name = CharName.Text;
        _title = CharTitle.Text;
        CharPreview.Text = GetCharDisplayName();
    }

    public string[] GetCharDetails() {
        return new string[] { _rank, _name, _title };
    }

    public string GetCharDisplayName() {
        // Create spacing if needed, to prevent words being squashed together, and prevent unnecessary spacing
        string rankNameSpacing = _rank.Length > 0 && _name.Length > 0 ? " " : "";
        string nameTitleSpacing = (_name.Length > 0 || _rank.Length > 0) && _title.Length > 0 ? " of " : "";

        return $"{_rank}{rankNameSpacing}{_name}{nameTitleSpacing}{_title}";
    }
}