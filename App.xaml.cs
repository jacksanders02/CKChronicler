﻿//  CK Chronicler - a Crusader Kings campaign tracker.
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

using System.Collections.Generic;
using System.Windows;

namespace CKChronicler
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>

    public partial class App : Application
    {
        public static readonly List<Trait> AllTraits = TraitsLoader.LoadAllTraits();

        public static Save LoadedSave { get; set; } = new();
        public static MainWindow AppWindow { get; set; } = null!;
    }
}