//  CK Chronicler - a Crusader Kings campaign tracker.
//  Copyright (C) 2022 Jack Sanders
// 
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
// 
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
// 
//   You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace CKChronicler;

public partial class CreationPage : Page {
    private string _currentButton;

    private int _charID;

    private readonly CharDetails _charDetails;
    private readonly TraitEditor _traitEditor;
    private readonly Page _prevPage;
    private readonly Page _nextPage;
    
    public CreationPage(int charID, string title, Page prevPage, Page nextPage)
    {
        _charID = charID;
        _currentButton = "DetailsButton";
        _charDetails = new CharDetails(charID);
        _traitEditor = new TraitEditor();
        _prevPage = prevPage;
        _nextPage = nextPage;
        InitializeComponent();

        BannerText.Text = title;
        CharacterCreateFrame.Content = _charDetails;
    }

    private void TabButton_OnClick(object o, RoutedEventArgs e) {
        var sender = (Button)o;

        if (_currentButton.Equals(sender.Name)) { return; } // No point wasting processing if already current
        
        // Update label of old current button (the tab being navigated away from)
        var currentLabel = (TextBlock)(FindName(_currentButton + "Label") ?? throw new NoNullAllowedException());
        string newLabel = currentLabel.Text.TrimStart('<', ' ').TrimEnd(' ', '>');

        currentLabel.Text = newLabel;
        
        _currentButton = sender.Name;

        switch (_currentButton) 
        {
            case "DetailsButton":
                DetailsButtonLabel.Text = "< DETAILS (NAME ETC.) >";
                CharacterCreateFrame.Content = _charDetails;
                break;
                
            case "TraitsButton":
                TraitsButtonLabel.Text = "< TRAITS >";
                _traitEditor.CharName.Text = App.LoadedSave.GetCharacter(_charID).GetFullTitle();
                CharacterCreateFrame.Content = _traitEditor;
                break;
            
            default:
                return;
        }
    }

    private void ReturnButton_OnClick(object sender, RoutedEventArgs e)
    {
        // Delete current save if this returns to save creation (creating initial ruler)
        if (_prevPage.GetType() == typeof(CreateSave)) 
        {
            App.LoadedSave.Delete();
        }
        App.AppWindow.SetContent(_prevPage);
    }

    private void NextButton_OnClick(object sender, RoutedEventArgs e)
    {
        _charDetails.SaveCharDetails();
        App.LoadedSave.SaveCharacter(_charID);
        App.AppWindow.SetContent(_nextPage);

        // Test deserialisation
        Save.LoadSave(App.LoadedSave.GetName())!.GetCharacter(_charID).TraceAllAttributes();
    }
}