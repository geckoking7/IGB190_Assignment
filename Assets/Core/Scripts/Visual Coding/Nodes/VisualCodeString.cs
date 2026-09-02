using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Convert/Combine Strings",
        dynamicDescription = "$ + $")]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp)]
    public string CombineStrings(string str1, string str2)
    {
        return str1 + str2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Substring",
        dynamicDescription = "First $ characters of $")]
    [NumberArg(argType = ArgType.Value, defaultValue = 3)]
    [StringArg(argType = ArgType.Temp)]
    public string Substring(float count, string str)
    {
        int chars = (int)count;
        return str.Substring(0, chars);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/String Replace",
        dynamicDescription = "$ replacing $ with $")]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp)]
    public string ReplaceString(string str1, string str2, string str3)
    {
        return str1.Replace(str2, str3);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Number to String",
        dynamicDescription = "To String: $")]
    [NumberArg(argType = ArgType.Temp)]
    public string NumberToString(float number)
    {
        return number.ToString();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Position to String",
        dynamicDescription = "To String: $")]
    [VectorArg(argType = ArgType.Temp)]
    public string VectorToString(Vector3 vector)
    {
        return vector.ToString();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Color to String",
        dynamicDescription = "To String: $")]
    [ColorArg(argType = ArgType.Temp)]
    public string ColorToString(Color color)
    {
        return "#" + ColorUtility.ToHtmlStringRGB(color);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Color to String",
        dynamicDescription = "To String: $")]
    [BoolArg(argType = ArgType.Temp)]
    public string BoolToString(bool boolean)
    {
        return boolean.ToString();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Convert/Projectile to String",
        dynamicDescription = "To String: $")]
    [ProjectileArg(argType = ArgType.Temp)]
    public string ProjectileToString(Projectile projectile)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        return projectile.name;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Name",
        dynamicDescription = "Name of $")]
    [UnitArg(argType = ArgType.Temp)]
    public string NameOfUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.unitName;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Tag",
        dynamicDescription = "Tag of $")]
    [UnitArg(argType = ArgType.Temp)]
    public string TagOfUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.tag;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Resource Name",
        dynamicDescription = "Player Resource Name")]
    public string PlayerResourceName()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.resourceName;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Name",
        dynamicDescription = "Name of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public string NameOfAbility(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.name;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Tag",
        dynamicDescription = "Tag of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public string TagOfAbility(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityTag;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Description",
        dynamicDescription = "Description of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public string DescriptionOfAbility(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityDescription;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Flavour Text",
        dynamicDescription = "Flavour Text of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public string FlavourTextOfAbility(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityFlavourText;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Animation Name",
        dynamicDescription = "Animation Name of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public string AnimationOfAbility(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityAnimation;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Name",
        dynamicDescription = "Name of $")]
    [ItemArg(argType = ArgType.Temp)]
    public string NameOfItem(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.name;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Tag",
        dynamicDescription = "Tag of $")]
    [ItemArg(argType = ArgType.Temp)]
    public string TagOfItem(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.itemTag;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Description",
        dynamicDescription = "Description of $")]
    [ItemArg(argType = ArgType.Temp)]
    public string DescriptionOfItem(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.itemDescription;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Flavour Text",
        dynamicDescription = "Flavour Text of $")]
    [ItemArg(argType = ArgType.Temp)]
    public string FlavourTextOfItem(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.itemFlavourText;
    }


    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Reward Text",
        dynamicDescription = "Reward of quest named $")]
    [StringArg(argType = ArgType.Temp)]
    public string GetQuestRewardText(string questName)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        return quest.Reward;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Requirement Text",
        dynamicDescription = "Requirement Text of quest named $")]
    [StringArg(argType = ArgType.Temp)]
    public string GetQuestRequirementText(string questName)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        return quest.GetMainRequirementText();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Name",
        dynamicDescription = "Name of $")]
    [BuffArg(argType = ArgType.Temp)]
    public string GetBuffName(Buff buff)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        return buff.name;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Description",
        dynamicDescription = "Description of $")]
    [BuffArg(argType = ArgType.Temp)]
    public string GetBuffDescription(Buff buff)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        return buff.buffDescription;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Flavour Text",
        dynamicDescription = "Flavour Text of $")]
    [BuffArg(argType = ArgType.Temp)]
    public string GetBuffFlavourText(Buff buff)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        return buff.buffFlavourText;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/String Variable",
        dynamicDescription = "String Variable: $")]
    [StringArg(argType = ArgType.Value, defaultValue = "StringVar")]
    public string GetStringVariable(string name)
    {
        return LogicEngine.current.GetLocalVariable<string>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Global String Variable",
        dynamicDescription = "Global String: $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name")]
    public string GetGlobalStringVariable(string name)
    {
        return LogicEngine.GetGlobalVariable<string>(name);
    }


}
