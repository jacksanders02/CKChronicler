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

public partial class MainMenu : Page
{
    public MainMenu()
    {
        InitializeComponent();
    }

    private void StartBtn_OnClick(object sender, RoutedEventArgs e)
    {
        if (Application.Current.MainWindow != null)
        {
            ((CKChronicler.MainWindow)Application.Current.MainWindow).ShowCreatePage();
        }
        else
        {
            throw new NoNullAllowedException();
        }
    }
}