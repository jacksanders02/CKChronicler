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

using System.Diagnostics;
using ProtoBuf;

namespace CKChronicler;

[ProtoContract]
public class Character
{
    // NAMING
    [ProtoMember(1)]
    public string Rank { get; set; } = "";
    [ProtoMember(2)]
    public string Name { get; set; } = "";
    [ProtoMember(3)]
    public string Dynasty { get; set; } = "";
    [ProtoMember(4)]
    public string PTitle { get; set; } = "";

    // STUFF
    [ProtoMember(5)]
    public string Religion { get; set; } = "";
    [ProtoMember(6)]
    public string Culture { get; set; } = "";

    // ATTRIBUTES
    [ProtoMember(7)]
    public int Diplo { get; set; } = 0;
    [ProtoMember(8)]
    public int Martial { get; set; } = 0;
    [ProtoMember(9)]
    public int Steward { get; set; } = 0;
    [ProtoMember(10)]
    public int Intrigue { get; set; } = 0;
    [ProtoMember(11)]
    public int Learning { get; set; } = 0;
    [ProtoMember(12)]
    public int Prowess { get; set; } = 0;

    public string GetFullTitle() {
        string nameSpacing = Rank.Length > 0 && Name.Length > 0  ? " " : "";
        string dynSpacing = (Rank.Length > 0 || Name.Length > 0) && Dynasty.Length > 0 ? " " : "";
        string titleSpacing = (Rank.Length > 0 || Name.Length > 0 || Dynasty.Length > 0) && PTitle.Length > 0 ? " of\x00A0" : "";
        return $"{Rank}{nameSpacing}{Name}{dynSpacing}{Dynasty}{titleSpacing}{PTitle}";
    }

    public void TraceAllAttributes()
    {
        Trace.WriteLine(GetFullTitle());
        Trace.WriteLine(Religion + " / " + Culture);
        Trace.WriteLine("Diplomacy: " + Diplo);
        Trace.WriteLine("Martial: " + Martial);
        Trace.WriteLine("Stewardship: " + Steward);
        Trace.WriteLine("Intrigue: " + Intrigue);
        Trace.WriteLine("Learning: " + Learning);
        Trace.WriteLine("Prowess: " + Prowess);
    }
}