using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKChronicler; 

public partial class Attributes : Page {
    public Attributes() {
        InitializeComponent();
    }
    
    private void UpdateAttributesText(object sender, TextChangedEventArgs e) {
    }

    public void SetCharName(string charName) {
        CharName.Text = charName;
    }

    private void IsNumber(object sender, TextCompositionEventArgs e) {
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
}