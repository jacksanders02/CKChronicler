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

using System;

namespace CKChronicler;

[Serializable]
public class Character
{
    // NAMING
    private string _rank = "";
    private string _name = "";
    private string _dynasty = "";
    private string _pTitle = "";

    // ATTRIBUTES
    private int _diplo = 0;
    private int _martial = 0;
    private int _steward = 0;
    private int _intrigue = 0;
    private int _learning = 0;
    private int _prowess = 0;

    public void SetRank(string r)
    {
        _rank = r;
    }

    public void SetName(string n)
    {
        _name = n;
    }

    public void SetPTitle(string pt)
    {
        _pTitle = pt;
    }
    
    public void SetDynasty(string d) {
        _dynasty = d;
    }

    public string GetFullTitle() {
        string nameSpacing = _rank.Length > 0 && _name.Length > 0  ? " " : "";
        string dynSpacing = (_rank.Length > 0 || _name.Length > 0) && _dynasty.Length > 0 ? " " : "";
        string titleSpacing = (_rank.Length > 0 || _name.Length > 0 || _dynasty.Length > 0) && _pTitle.Length > 0 ? " of\x00A0" : "";
        return $"{_rank}{nameSpacing}{_name}{dynSpacing}{_dynasty}{titleSpacing}{_pTitle}";
    }
}