using System;
using UnityEngine;  

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Check Preset",
        dynamicDescription = "$",
        icon = conditionIcon)]
    [BoolArg(argType = ArgType.Temp, allowFunction = false, allowValue = true)]
    public bool CheckPreset(bool bool1)
    {
        return bool1;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Unit Exists",
        dynamicDescription = "$ exists",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public bool UnitExists(Unit unit)
    {
        return unit != null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Item Exists",
        dynamicDescription = "$ exists",
        icon = conditionIcon)]
    [ItemArg(argType = ArgType.Temp)]
    public bool ItemExists(Item item)
    {
        return item != null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Item Exists",
        dynamicDescription = "$ exists",
        icon = conditionIcon)]
    [AbilityArg(argType = ArgType.Temp)]
    public bool AbilityExists(Ability ability)
    {
        return ability != null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Buff Exists",
        dynamicDescription = "$ exists",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    public bool BuffExists(Buff buff)
    {
        return buff != null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/String Exists",
        dynamicDescription = "$ exists and is not empty",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp)]
    public bool StringExists(string str)
    {
        return str != null && str.Length > 0;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Unit Group Exists",
        dynamicDescription = "$ exists and contains at least one unit",
        icon = conditionIcon)]
    [UnitGroupArg(argType = ArgType.Temp)]
    public bool UnitGroupExists(UnitGroup unitGroup)
    {
        return unitGroup != null && unitGroup.unitList.Count > 0;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Local Variable Exists",
        dynamicDescription = "Local variable named $ exists",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp)]
    public bool LocalVariableExists(string variableName)
    {
        return LogicEngine.current.LocalVariableExists(variableName);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Validity/Global Variable Exists",
        dynamicDescription = "Global variable named $ exists",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp)]
    public bool GlobalVariableExists(string variableName)
    {
        return LogicEngine.GlobalVariableExists(variableName);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Or Comparison",
        dynamicDescription = "$ or $",
        icon = conditionIcon)]
    [BoolArg(argType = ArgType.Temp)]
    [BoolArg(argType = ArgType.Temp)]
    public bool OrComparison(bool bool1, bool bool2)
    {
        return bool1 || bool2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/And Comparison",
        dynamicDescription = "$ and $",
        icon = conditionIcon)]
    [BoolArg(argType = ArgType.Temp)]
    [BoolArg(argType = ArgType.Temp)]
    public bool AndComparison(bool bool1, bool bool2)
    {
        return (bool1 && bool2);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Bool Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [BoolArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [BoolArg(argType = ArgType.Temp)]
    public bool BoolComparison(bool bool1, string comparator, bool bool2)
    {
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return bool1 == bool2;
            case PresetStrings.NotEqualTo:
                return bool1 != bool2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Number Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [NumberArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.NumberComparators, allowPreset = false, allowFunction = false)]
    [NumberArg(argType = ArgType.Temp)]
    public bool NumberComparison(float num1, string comparator, float num2)
    {
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return num1 == num2;
            case PresetStrings.NotEqualTo:
                return num1 != num2;
            case PresetStrings.LessThan:
                return num1 < num2;
            case PresetStrings.LessThanOrEqualTo:
                return num1 <= num2;
            case PresetStrings.GreaterThan:
                return num1 > num2;
            case PresetStrings.GreaterThanOrEqualTo:
                return num1 >= num2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Vector Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [VectorArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [VectorArg(argType = ArgType.Temp)]
    public bool VectorComparison(Vector3 vec1, string comparator, Vector3 vec2)
    {
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return vec1 == vec2;
            case PresetStrings.NotEqualTo:
                return vec1 != vec2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Ability Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [AbilityArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp)]
    public bool AbilityComparison(Ability ability1, string comparator, Ability ability2)
    {
        Error(ability1 == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(ability2 == null, VisualCodeLabels.Errors.InvalidAbility);
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return Ability.AbilitiesShareTemplate(ability1, ability2);
            case PresetStrings.NotEqualTo:
                return !Ability.AbilitiesShareTemplate(ability1, ability2);
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Item Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [ItemArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [ItemArg(argType = ArgType.Temp)]
    public bool ItemComparison(Item item1, string comparator, Item item2)
    {
        Error(item1 == null, VisualCodeLabels.Errors.InvalidItem);
        Error(item2 == null, VisualCodeLabels.Errors.InvalidItem);
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return item1.name == item2.name;
            case PresetStrings.NotEqualTo:
                return item1.name != item2.name;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/String Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp)]
    public bool StringComparison(string string1, string comparator, string string2)
    {
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return string1 == string2;
            case PresetStrings.NotEqualTo:
                return string1 != string2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Unit Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp)]
    public bool UnitComparison(Unit unit1, string comparator, Unit unit2)
    {
        Error(unit1 == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit2 == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return unit1 == unit2;
            case PresetStrings.NotEqualTo:
                return unit1 != unit2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Comparisons/Buff Comparison",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [BuffArg(argType = ArgType.Temp)]
    public bool BuffComparison(Buff buff1, string comparator, Buff buff2)
    {
        Error(buff1 == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(buff2 == null, VisualCodeLabels.Errors.InvalidBuff);
        switch (comparator)
        {
            case PresetStrings.EqualTo:
                return buff1 == buff2;
            case PresetStrings.NotEqualTo:
                return buff1 != buff2;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Region/Region Exists",
        dynamicDescription = "Region labeled $ $ exist",
        icon = regionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.DoesDoesNot, allowPreset = false, allowFunction = false)]
    public bool RegionExists(string regionName, string comparator)
    {
        Error(regionName == null || regionName.Length == 0, VisualCodeLabels.Errors.InvalidRegionName);
        switch (comparator)
        {
            case PresetStrings.Does:
                return Region.RegionExists(regionName);
            case PresetStrings.DoesNot:
                return !Region.RegionExists(regionName);
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Region/Unit Is In Region",
        dynamicDescription = "$ $ region named $",
        icon = regionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsInIsNotIn, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    public bool UnitIsInRegion(Unit unit, string comparator, string regionName)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.IsIn:
                return Region.UnitIsInRegion(unit, regionName);
            case PresetStrings.IsNotIn:
                return !Region.UnitIsInRegion(unit, regionName);
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Input/Key is Held",
        dynamicDescription = "$ $ Held",
        icon = inputIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool KeyIsHeld(string keyString, string comparator)
    {
        switch (comparator)
        {
            case PresetStrings.Is: return Input.GetKey(keyString);
            case PresetStrings.IsNot: return !Input.GetKey(keyString);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Input/Key Pressed This Frame",
        dynamicDescription = "$ $ Pressed This Frame",
        icon = inputIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool KeyPressedThisFrame(string keyString, string comparator)
    {
        switch (comparator)
        {
            case PresetStrings.Is: return Input.GetKeyDown(keyString);
            case PresetStrings.IsNot: return !Input.GetKeyDown(keyString);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Input/Key Released This Frame",
        dynamicDescription = "$ $ Released This Frame",
        icon = inputIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool KeyReleasedThisFrame(string keyString, string comparator)
    {
        switch (comparator)
        {
            case PresetStrings.Is: return Input.GetKeyUp(keyString);
            case PresetStrings.IsNot: return !Input.GetKeyUp(keyString);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit Group/Unit Group is Empty",
        dynamicDescription = "$ $ empty",
        icon = conditionIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false, tempLabel = "Unit Group")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool UnitGroupIsEmpty(UnitGroup unitGroup, string comparator)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        switch (comparator)
        {
            case PresetStrings.Is: return unitGroup.Count() == 0;
            case PresetStrings.IsNot: return unitGroup.Count() != 0;
            default: return true;
        }
    }

    /*
    [VisualScriptingFunction(
        dropdownDescription = "Unit Group/Unit Group is not Empty",
        dynamicDescription = "$ is not empty",
        icon = conditionIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public bool UnitGroupIsNotEmpty(UnitGroup unitGroup)
    {
        Error(unitGroup == null, "The specified unit group is invalid.");
        return (unitGroup.Count() > 0);
    }
    */

    [VisualScriptingFunction(
        dropdownDescription = "Unit Group/Unit is in Unit Group",
        dynamicDescription = "$ $ $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsInIsNotIn, allowPreset = false, allowFunction = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false, tempLabel = "Unit Group")]
    public bool UnitIsInUnitGroup(Unit unit, string comparator, UnitGroup unitGroup)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        switch (comparator)
        {
            case PresetStrings.IsIn: return unitGroup.Contains(unit);
            case PresetStrings.IsNotIn: return !unitGroup.Contains(unit);
            default: return false;
        }
    }

    /*
    [VisualScriptingFunction(
        dropdownDescription = "Unit Group/Unit is not in Unit Group",
        dynamicDescription = "$ is not in $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public bool UnitIsNotInUnitGroup(Unit unit, UnitGroup unitGroup)
    {
        Error(unit == null, "The specified unit is invalid.");
        Error(unitGroup == null, "The specified unit group is invalid.");
        return (!unitGroup.Contains(unit));
    }
    */

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Matches Template",
        dynamicDescription = "$ type $ $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.MatchesDoesNotMatch, allowPreset = false, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp)]
    public bool UnitTypeMatch(Unit unit1, string comparator, Unit unit2)
    {
        Error(unit1 == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit2 == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Matches: return unit1.unitName == unit2.unitName;
            case PresetStrings.DoesNotMatch: return unit1.unitName != unit2.unitName;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Is Moving",
        dynamicDescription = "$ $ moving",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool UnitIsMoving(Unit unit, string comparator)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Is: return unit.IsMoving();
            case PresetStrings.IsNot: return !unit.IsMoving();
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Is Casting",
        dynamicDescription = "$ $ casting",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool UnitIsCasting(Unit unit, string comparator)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Is: return !unit.IsMoving();
            case PresetStrings.IsNot: return unit.IsMoving();
            default: return false; 
        }
    } 

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Can Move",
        dynamicDescription = "$ $ move",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.CanCanNot, allowPreset = false, allowFunction = false)]
    public bool UnitCanMove(Unit unit, string comparator)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Can: return unit.CanMove();
            case PresetStrings.Cannot: return !unit.CanMove();
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Has Buff",
        dynamicDescription = "$ $ buff labeled $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Buff Name")]
    public bool UnitHasBuff(Unit unit, string comparator, string buff)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Has: return unit.HasBuff(buff);
            case PresetStrings.DoesNotHave: return !unit.HasBuff(buff);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit is Stunned",
        dynamicDescription = "$ $ stunned",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool UnitIsStunned(Unit unit, string comparator)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Is: return unit.IsStunned();
            case PresetStrings.IsNot: return !unit.IsStunned();
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Ability Is On Cooldown",
        dynamicDescription = "$ $ on cooldown for $",
        icon = conditionIcon)]
    [AbilityArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public bool UnitAbilityIsOnCooldown(Ability ability, string comparator, Unit unit)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Is: return ability.GetRemainingCooldown(unit) > 0;
            case PresetStrings.IsNot: return ability.GetRemainingCooldown(unit) <= 0;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Can Cast Ability",
        dynamicDescription = "$ $ cast $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.CanCanNot, allowPreset = false, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp)]
    public bool UnitCanCastAbility(Unit unit, string comparator, Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Can: return ability.IsValidToCast(unit, null, unit.transform.position);
            case PresetStrings.Cannot: return !ability.IsValidToCast(unit, null, unit.transform.position);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Monster/Monster Is Empowered",
        dynamicDescription = "$ $ empowered",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool UnitIsEmpowered(Unit unit, string comparator)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (unit is not Monster) return false;
        switch (comparator)
        {
            case PresetStrings.Is: return ((Monster)unit).isEmpowered;
            case PresetStrings.IsNot: return !((Monster)unit).isEmpowered;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Has Tag",
        dynamicDescription = "$ $ tag named $",
        icon = conditionIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false, tempLabel = "Tag")]
    public bool UnitHasTag(Unit unit, string comparator, string tag)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        switch (comparator)
        {
            case PresetStrings.Has: return unit.CompareTag(tag);
            case PresetStrings.DoesNotHave: return !unit.CompareTag(tag);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Has Item Equipped",
        dynamicDescription = "Player $ $ equipped",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    [ItemArg(argType = ArgType.Temp)]
    public bool PlayerHasItemEquipped(string comparator, Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        switch (comparator)
        {
            case PresetStrings.Has: return GameManager.player.HasItemEquipped(item);
            case PresetStrings.DoesNotHave: return !GameManager.player.HasItemEquipped(item);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Has Item In Inventory",
        dynamicDescription = "Player has $ in their inventory",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    [ItemArg(argType = ArgType.Temp)]
    public bool PlayerHasItemInInventory(string comparator, Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        switch (comparator)
        {
            case PresetStrings.Has: return GameManager.player.HasItemInInventory(item);
            case PresetStrings.DoesNotHave: return !GameManager.player.HasItemInInventory(item);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Is Dead",
        dynamicDescription = "Player $ dead",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool PlayerIsDead(string comparator)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        switch (comparator)
        {
            case PresetStrings.Is: return GameManager.player.isDead;
            case PresetStrings.IsNot: return !GameManager.player.isDead;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Is Active",
        dynamicDescription = "Quest named $ $ currently active",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool QuestIsActive(string label, string comparator)
    {
        Error(label.Length == 0, VisualCodeLabels.Errors.InvalidQuestName);
        switch (comparator)
        {
            case PresetStrings.Is: return GameManager.quests.QuestIsActive(label);
            case PresetStrings.IsNot: return !GameManager.quests.QuestIsActive(label);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Is Completed",
        dynamicDescription = "Quest named $ $ completed",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool QuestIsCompleted(string label, string comparator)
    {
        Error(label.Length == 0, VisualCodeLabels.Errors.InvalidQuestName);
        switch (comparator)
        {
            case PresetStrings.Is: return GameManager.quests.QuestIsCompleted(label);
            case PresetStrings.IsNot: return !GameManager.quests.QuestIsCompleted(label);
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Has Requirement",
        dynamicDescription = "Quest named $ $ requirement $",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Requirement Name")]
    public bool QuestHasRequirement(string label, string comparator, string requirement)
    {
        Quest quest = GameManager.quests.GetQuest(label);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        Quest.QuestItem item = quest.GetQuestRequirement(requirement);
        switch (comparator)
        {
            case PresetStrings.Has: return item != null;
            case PresetStrings.DoesNotHave: return item == null;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Quest Requirement is Completed",
        dynamicDescription = "Quest named $ $ completed requirement $",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasHasNot, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Requirement Name")]
    public bool QuestRequirementCompleted(string label, string comparator, string requirement)
    {
        Quest quest = GameManager.quests.GetQuest(label);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        Quest.QuestItem item = quest.GetQuestRequirement(requirement);
        Error(item == null, VisualCodeLabels.Errors.InvalidQuestRequirement);
        switch (comparator)
        {
            case PresetStrings.Has: return item.CurrentProgress == item.MaxProgress;
            case PresetStrings.HasNot: return item.CurrentProgress != item.MaxProgress;
            default: return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Rarity",
        dynamicDescription = "$ Rarity $ $",
        icon = conditionIcon)]
    [ItemArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.BoolComparators, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Rarities, allowFunction = false, allowPreset = false)]
    public bool ItemRarity(Item item, string comparison, string rarityText)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        Item.ItemRarity rarity = (Item.ItemRarity)Enum.Parse(typeof(Item.ItemRarity), rarityText);
        switch (comparison)
        {
            case PresetStrings.EqualTo:
                return item.itemRarity == rarity;
            case PresetStrings.NotEqualTo:
                return item.itemRarity != rarity;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff is Removed on Death",
        dynamicDescription = "Buff $ $ removed on death",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool IsBuffRemovedOnDeath (Buff buff, string comparison)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        switch (comparison)
        {
            case PresetStrings.Is:
                return buff.buffRemovedOnDeath;
            case PresetStrings.IsNot:
                return !buff.buffRemovedOnDeath;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Visible in UI",
        dynamicDescription = "Buff $ $ visible in UI",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IsIsNot, allowPreset = false, allowFunction = false)]
    public bool IsBuffVisibleInUI(Buff buff, string comparison)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        switch (comparison)
        {
            case PresetStrings.Is:
                return buff.buffVisibleInUI;
            case PresetStrings.IsNot:
                return !buff.buffVisibleInUI;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Has Duration",
        dynamicDescription = "Buff $ $ a duration",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.HasDoesntHave, allowPreset = false, allowFunction = false)]
    public bool IsBuffTimed(Buff buff, string comparison)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        switch (comparison)
        {
            case PresetStrings.Has:
                return buff.buffHasDuration;
            case PresetStrings.DoesNotHave:
                return !buff.buffHasDuration;
            default:
                return false;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Stacks Refresh Duration",
        dynamicDescription = "Adding stacks to $ $ refresh duration",
        icon = conditionIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.DoesDoesNot, allowPreset = false, allowFunction = false)]
    public bool BuffStacksRefreshDuration(Buff buff, string comparison)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        switch (comparison)
        {
            case PresetStrings.Does:
                return buff.addingStacksRefreshesDuration;
            case PresetStrings.DoesNot:
                return !buff.addingStacksRefreshesDuration;
            default:
                return false;
        }
    }


    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Items Can Drop",
        dynamicDescription = "Items Can Drop",
        icon = conditionIcon)]
    public bool ItemsCanDrop()
    {
        return GameManager.gameSettings.ItemsCanDrop;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Health Pickups Can Drop",
        dynamicDescription = "Health Pickups Can Drop",
        icon = conditionIcon)]
    public bool HealthPickupsCanDrop()
    {
        return GameManager.gameSettings.HealthPickupsCanDrop;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Gold Pickups Can Drop",
        dynamicDescription = "Gold Pickups Can Drop",
        icon = conditionIcon)]
    public bool GoldCanDrop()
    {
        return GameManager.gameSettings.GoldPickupsCanDrop;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variable/Bool Variable",
        dynamicDescription = "Bool Variable: $",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "BoolArg", allowFunction = false, allowPreset = false)]
    public bool GetBoolVariable(string name)
    {
        return LogicEngine.current.GetLocalVariable<bool>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variable/Global Bool Variable",
        dynamicDescription = "Global Bool: $",
        icon = conditionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    public bool GetGlobalBoolVariable(string name)
    {
        return LogicEngine.GetGlobalVariable<bool>(name);
    }
}
