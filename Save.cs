using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace CKChronicler;

public class Save
{
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
        Directory.CreateDirectory(GetDirectory());  // Create save location
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

    public string GetName()
    {
        return _saveName;
    }

    public string GetDirectory()
    {
        return ".\\Saves\\" + new Regex("[\\/:|<>*?]").Replace(_saveName, "");
    }

    public Character GetCharacter(int id)
    {
        return _characters[id];
    }

    public void Delete()
    {
        Directory.Delete(GetDirectory(), true);
    }
}