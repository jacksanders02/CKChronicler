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

    private readonly CharDetails _charDetails;
    private readonly Attributes _attributes;
    
    public CreationPage() {
        _currentButton = "DetailsButton";
        _charDetails = new CharDetails();
        _attributes = new Attributes();
        InitializeComponent();
        
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
                CharacterCreateFrame.Content = new CKChronicler.CharDetails();
                break;
            
            default:
                return;
        }
    }
}