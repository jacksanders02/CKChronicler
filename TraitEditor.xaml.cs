using System.Collections.Generic;
using System.Windows.Controls;

namespace CKChronicler;

public partial class TraitEditor : Page
{
    public List<Trait> TraitList { get; set; }

    public TraitEditor()
    {
        TraitList = App.AllTraits;
        InitializeComponent();
        TraitsListBox.ItemsSource = TraitList;
    }

    private void TraitFilterBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        string filter = TraitFilterBox.Text;
        List<Trait> filtered = new List<Trait>();

        foreach (var t in App.AllTraits)
        {
            if (t.name.ToLower().Contains(filter.ToLower()))
            {
                filtered.Add(t);
            }
        }

        TraitList = filtered;
        TraitsListBox.ItemsSource = TraitList;
    }
}