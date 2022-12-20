using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;
// ReSharper disable InconsistentNaming

namespace CKChronicler;

public static class TraitsLoader
{
    public abstract class Effect
    {
        public string effected_attr { get; set; } = null!;
        public string effect_magnitude { get; set; } = null!;
        public string positivity { get; set; } = null!;
    }

    public abstract class Trait
    {
        public string name { get; set; } = null!;
        public string icon { get; set; } = null!;
        public List<Effect> effects { get; set; } = null!;
        public string description { get; set; } = null!;
    }

    public static List<Trait> LoadAllTraits()
    {
        string jsonString = File.ReadAllText("traits.json");
        return JsonSerializer.Deserialize<List<Trait>>(jsonString)!;
    }
}