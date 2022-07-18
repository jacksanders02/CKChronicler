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

namespace CKChronicler;

public class Character
{
    // NAMING
    private string rank = "";
    private string name = "";
    private string pTitle = "";

    // ATTRIBUTES
    private int diplo = 0;
    private int martial = 0;
    private int steward = 0;
    private int intrigue = 0;
    private int learning = 0;
    private int prowess = 0;

    public void SetRank(string r)
    {
        rank = r;
    }

    public void SetName(string n)
    {
        name = n;
    }

    public void SetPTitle(string pt)
    {
        pTitle = pt;
    }

    public string GetFullTitle()
    {
        return $"{rank} {name} {(pTitle != "" ? "of " + pTitle : "")}";
    }
}