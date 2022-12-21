using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using ProtoBuf;

namespace CKChronicler;

public class Save
{
    private const string CharacterSubdir = "/Characters";
    private const string ImgSubdir = CharacterSubdir + "/Images";
    private static string DirSafeName(string name)
    {
        return new Regex("[\\/:|<>*?]").Replace(name, "");
    }
    private static string GetDirectory(string name)
    {
        return ".\\Saves\\" + DirSafeName(name);
    }
    
    private readonly Dictionary<int, Character> _characters;
    private readonly string _saveName;

    public Save()
    {
        _characters = new Dictionary<int, Character>();
        _saveName = "";
    }
    
    public Save(string name)
    {
        _saveName = name;
        _characters = new Dictionary<int, Character>();
        Directory.CreateDirectory(GetDirectory(_saveName));  // Create save location
        Directory.CreateDirectory(GetDirectory(_saveName) + CharacterSubdir);  // Create characters location
        Directory.CreateDirectory(GetDirectory(_saveName) + ImgSubdir);  // Create character images location

        string[] saveInfo = new[] { _saveName };
        File.WriteAllLines(GetDirectory(_saveName) + "/save.info", saveInfo);
    }

    public int AddCharacter()
    {
        int newID;
        if (_characters.Keys.Count == 0)
        {
            newID = 0;
        } else
        {
            newID = _characters.Keys.Max() + 1;
        }
        _characters.Add(newID, new Character());
        return newID;
    }

    public void saveCharacter(int charID)
    {
        var c = _characters[charID];
        using (var f = File.Create($"{CharacterSubdir}/{charID}.ck3char"))
        {
            Serializer.Serialize(f, c);
        }
    }

    public string GetName()
    {
        return _saveName;
    }

    public Character GetCharacter(int id)
    {
        return _characters[id];
    }

    public void Delete()
    {
        Directory.Delete(GetDirectory(_saveName), true);
    }
}