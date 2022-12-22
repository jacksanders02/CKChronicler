using System.Collections.Generic;
using System.Diagnostics;
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

    private static Character loadChar(string saveName, int id)
    {
        Character c;
        using (FileStream f = File.OpenRead($"{GetDirectory(saveName)}{CharacterSubdir}/{id}.ck3char"))
        {
            c = Serializer.Deserialize<Character>(f);
        }
        return c;
    }

    public static Save? LoadSave(string name)
    {
        string charLoc = GetDirectory(name) + CharacterSubdir;
        
        if (!Directory.Exists(charLoc))
        {
            return null;
        }

        DirectoryInfo d = new DirectoryInfo(charLoc);
        FileInfo[] fi = d.GetFiles("*.ck3char");
        Dictionary<int, Character> cs = new();

        foreach (FileInfo f in fi)
        {
            int charID = int.Parse(f.Name.Replace(".ck3char", ""));
            cs[charID] = loadChar(name, charID);
        }

        return new Save(name, cs);
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

    public Save(string name, Dictionary<int, Character> chars)
    {
        _saveName = name;
        _characters = chars;
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

    public void SaveCharacter(int charID)
    {
        var c = _characters[charID];
        c.TraceAllAttributes();
        using (var f = File.Create($"{GetDirectory(_saveName)}{CharacterSubdir}/{charID}.ck3char"))
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