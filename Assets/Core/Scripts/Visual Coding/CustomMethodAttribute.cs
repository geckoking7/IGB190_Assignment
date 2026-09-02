using System;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class VisualScriptingFunction : Attribute
{
    public string dropdownDescription;
    public string dynamicDescription;
    public string tooltip;
    public string icon;
    public bool allowsChildren = false;
    
    public VisualScriptingFunction()
    {

    } 

    public VisualScriptingFunction(string description, string dynamicDescription, string icon)
    {
        this.dropdownDescription = description;
        this.dynamicDescription = dynamicDescription;
        this.icon = icon;
    }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public class VisualScriptingEvent : VisualScriptingFunction
{

}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class EventPreset : Attribute
{
    public string presetName;
    public EventPreset() { }
    public EventPreset(string presetName) { this.presetName = presetName;  }
}

public class Arg : Attribute
{
    public ArgType argType = ArgType.Temp;

    public bool allowValue = true;
    public bool allowPreset = true;
    public bool allowFunction = true;

    public string suffix = "";
    public string tempLabel = "";
    public string preset = "";


    public Arg(ArgType argType, string tempLabel)
    {
        this.tempLabel = tempLabel;
    }

    public Arg()
    {

    }

    public virtual Type GetStoredType() { return null; }
    public virtual object GetValue() { return null; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class NumberArg : Arg
{
    public float defaultValue;
    public NumberArg() { }
    public override Type GetStoredType() { return typeof(float); }
    public override object GetValue() { return defaultValue; }
}

public enum PresetChoices
{
    NoRestriction,
    IncreaseDecrease,
    BoolComparators,
    NumberComparators,
    Buffs,
    PlayOptions,
    VectorComponents,
    Rarities,
    Factions,
    Keybinds,
    IsIsNot,
    IsInIsNotIn,
    ContainsDoesNotContain,
    CanCanNot,
    HasDoesntHave,
    MatchesDoesNotMatch,
    HasHasNot,
    DoesDoesNot,
    ItemSlots,
    EnableDisable,
    AttachPoints,
}

public class PresetStrings
{
    public const string Is = "Is";
    public const string IsNot = "Is Not";

    public const string IsIn = "Is In";
    public const string IsNotIn = "Is Not In";

    public const string Contains = "Contains";
    public const string DoesNotContain = "Does Not Contain";

    public const string Can = "Can";
    public const string Cannot = "Cannot";

    public const string Has = "Has";
    public const string DoesNotHave = "Does Not Have";
    public const string HasNot = "Has Not";

    public const string Matches = "Matches";
    public const string DoesNotMatch = "Does Not Match";
    
    public const string Increase = "Increase";
    public const string Decrease = "Decrease";

    public const string EqualTo = "Equal To";
    public const string NotEqualTo = "Not Equal To";
    public const string LessThan = "Less Than";
    public const string LessThanOrEqualTo = "Less Than Or Equal To";
    public const string GreaterThan = "Greater Than";
    public const string GreaterThanOrEqualTo = "Greater Than Or Equal To";

    public const string Does = "Does";
    public const string DoesNot = "Does Not";

    public const string Enable = "Enable";
    public const string Disable = "Disable";

    public const string Weapon = "Weapon";
    public const string Amulet = "Amulet";
    public const string Armor = "Armor";
    public const string Boots = "Boots";
    public const string Ring1 = "Ring 1";
    public const string Ring2 = "Ring 2";

    public const string Play = "Play";
    public const string Stop = "Stop";
    public const string PlayOrRefresh = "Play Or Refresh";

    public const string X = "X";
    public const string Y = "Y";
    public const string Z = "Z";

    public const string CastPoint = "Cast Point";
    public const string Head = "Head";
    public const string Origin = "Origin";
    public const string Center = "Center";
    public const string LeftHand = "Left Hand";
    public const string RightHand = "Right Hand";
    public const string Custom1 = "Custom 1";
    public const string Custom2 = "Custom 2";
    public const string Custom3 = "Custom 3";
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class StringArg : Arg
{
    private static string[] isIsNot = new string[] { PresetStrings.Is, PresetStrings.IsNot };
    private static string[] isInIsNotIn = new string[] { PresetStrings.IsIn, PresetStrings.IsNotIn };
    private static string[] containsDoesNotContain = new string[] { PresetStrings.Contains, PresetStrings.DoesNotContain };
    private static string[] canCannot = new string[] { PresetStrings.Can, PresetStrings.Cannot };
    private static string[] doesDoesNotHave = new string[] { PresetStrings.Has, PresetStrings.DoesNotHave };
    private static string[] matchesDoesNotMatch = new string[] { PresetStrings.Matches, PresetStrings.DoesNotMatch };
    private static string[] hasHasNot = new string[] { PresetStrings.Has, PresetStrings.HasNot };
    private static string[] doesDoesNot = new string[] { PresetStrings.Does, PresetStrings.DoesNot };
    private static string[] attachPoints = new string[] { PresetStrings.CastPoint, PresetStrings.Head, PresetStrings.Origin, PresetStrings.Center, PresetStrings.LeftHand, PresetStrings.RightHand, PresetStrings.Custom1, PresetStrings.Custom2, PresetStrings.Custom3 };

    private static string[] increaseDecrease = new string[] { PresetStrings.Increase, PresetStrings.Decrease };
    private static string[] enableDisable = new string[] { PresetStrings.Enable, PresetStrings.Disable };
    private static string[] boolComparators = new string[] { PresetStrings.EqualTo, PresetStrings.NotEqualTo };
    private static string[] numberComparators = new string[] { PresetStrings.EqualTo, PresetStrings.NotEqualTo, PresetStrings.LessThan,
        PresetStrings.LessThanOrEqualTo, PresetStrings.GreaterThan, PresetStrings.GreaterThanOrEqualTo };
    private static string[] playOptions = new string[] { PresetStrings.Play, PresetStrings.Stop, PresetStrings.PlayOrRefresh };
    private static string[] vectorComponents = new string[] { PresetStrings.X, PresetStrings.Y, PresetStrings.Z };
    private static string[] itemSlots = new string[] { PresetStrings.Weapon, PresetStrings.Amulet, PresetStrings.Armor,
        PresetStrings.Boots, PresetStrings.Ring1, PresetStrings.Ring2 };
    private static string[] keybinds = new string[] { "A", "B", "C", "D", "E", "F", "G", "H", 
        "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", 
        "Z", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "Tab", 
        "Escape", "LeftShift", "LeftAlt", "LeftControl", "Space" };

    private static string[] buffs = null;
    private static string[] rarities = null;
    private static string[] factions = null;
    
    public static string[] GetOptions (PresetChoices choice)
    {
        switch (choice)
        {
            case PresetChoices.Buffs:
                if (buffs == null)
                {
                    Stat[] allStats = (Stat[])Enum.GetValues(typeof(Stat));
                    buffs = new string[allStats.Length];
                    for (int i = 0; i < buffs.Length; i++)
                    {
                        buffs[i] = allStats[i].Label();
                    }
                }
                return buffs;

            case PresetChoices.Rarities:
                if (rarities == null) rarities = Enum.GetNames(typeof(Item.ItemRarity));
                return rarities;

            case PresetChoices.Factions:
                if (factions == null) factions = Enum.GetNames(typeof(Unit.Faction));
                return factions;

            case PresetChoices.IncreaseDecrease: return increaseDecrease;
            case PresetChoices.BoolComparators: return boolComparators;
            case PresetChoices.NumberComparators: return numberComparators;
            case PresetChoices.PlayOptions: return playOptions;
            case PresetChoices.VectorComponents: return vectorComponents;
            case PresetChoices.Keybinds: return keybinds;
            case PresetChoices.IsInIsNotIn: return isInIsNotIn;
            case PresetChoices.IsIsNot: return isIsNot;
            case PresetChoices.ContainsDoesNotContain: return containsDoesNotContain;
            case PresetChoices.CanCanNot: return canCannot;
            case PresetChoices.HasDoesntHave: return doesDoesNotHave;
            case PresetChoices.MatchesDoesNotMatch: return matchesDoesNotMatch;
            case PresetChoices.HasHasNot: return hasHasNot;
            case PresetChoices.DoesDoesNot: return doesDoesNot;
            case PresetChoices.ItemSlots: return itemSlots;
            case PresetChoices.EnableDisable: return enableDisable;
            case PresetChoices.AttachPoints: return attachPoints;
            default: return new string[] { };
        }
    }

    public PresetChoices choicePreset;
    //public int selectedPresetOption = 0;

    public string defaultValue;
    public StringArg() { }
    public override Type GetStoredType() { return typeof(string); }
    public override object GetValue() { return defaultValue; }

}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class UnitArg : Arg
{
    public Unit defaultValue;
    public UnitArg() { tempLabel = "Unit"; }
    public override Type GetStoredType() { return typeof(Unit); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class InteractableArg : Arg
{
    public CustomInteractable defaultValue;
    public InteractableArg() { tempLabel = "Interactable"; }
    public override Type GetStoredType() { return typeof(CustomInteractable); }
    public override object GetValue() { return defaultValue; }
}


[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class UnitGroupArg : Arg
{
    public UnitGroup defaultValue;
    public UnitGroupArg() { tempLabel = "Unit(s)"; }
    public override Type GetStoredType() { return typeof(UnitGroup); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class AudioClipArg : Arg
{
    public AudioClip defaultValue;
    public AudioClipArg() { tempLabel = "Audio Clip"; }
    public override Type GetStoredType() { return typeof(AudioClip); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class VectorArg : Arg
{
    public Vector3 defaultValue;
    public VectorArg() { tempLabel = "Location"; }
    public override Type GetStoredType() { return typeof(Vector3); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class BoolArg : Arg
{
    public bool defaultValue;
    public BoolArg() { tempLabel = "Bool"; }
    public override Type GetStoredType() { return typeof(bool); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ItemArg : Arg
{
    public Item defaultValue;
    public ItemArg() { tempLabel = "Item"; }
    public override Type GetStoredType() { return typeof(Item); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ItemSetArg : Arg
{
    public ItemSet defaultValue;
    public ItemSetArg() { tempLabel = "Item Set"; }
    public override Type GetStoredType() { return typeof(ItemSet); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class BuffArg : Arg
{
    public Buff defaultValue;
    public BuffArg() { tempLabel = "Buff"; }
    public override Type GetStoredType() { return typeof(Buff); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ColorArg : Arg
{
    public Color defaultValue = Color.white;
    public ColorArg() { tempLabel = "Color"; }
    public override Type GetStoredType() { return typeof(Color); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class ProjectileArg : Arg
{
    public Projectile defaultValue;
    public ProjectileArg() { tempLabel = "Projectile"; }
    public override Type GetStoredType() { return typeof(Projectile); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class AbilityArg : Arg
{
    public Ability defaultValue;
    public AbilityArg() { tempLabel = "Ability"; }
    public override Type GetStoredType() { return typeof(Ability); }
    public override object GetValue() { return defaultValue; }
}

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
public class EffectArg : Arg
{
    public CustomVisualEffect defaultValue;
    public EffectArg() { tempLabel = "Effect"; }
    public override Type GetStoredType() { return typeof(CustomVisualEffect); }
    public override object GetValue() { return defaultValue; }
}

public enum ArgType
{
    Temp,
    Value,
    Preset,
    Function
}

public static class ArgTypeExtensions
{
    public static GeneralNode.ReturnType ToNodeReturnType(this ArgType sourceType)
    {
        return sourceType switch
        {
            ArgType.Temp => GeneralNode.ReturnType.Temp,
            ArgType.Value => GeneralNode.ReturnType.Value,
            ArgType.Preset => GeneralNode.ReturnType.Preset,
            ArgType.Function => GeneralNode.ReturnType.Function,
            _ => throw new ArgumentOutOfRangeException(nameof(sourceType), $"No mapping exists for {sourceType}")
        };
    }
}