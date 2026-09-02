using MyUtilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Math/Addition",
        dynamicDescription = "$ + $")]
    [NumberArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public float Addition(float num1, float num2)
    {
        return num1 + num2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Subtraction",
        dynamicDescription = "$ - $")]
    [NumberArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public float Subtraction(float num1, float num2)
    {
        return num1 - num2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Multiplication",
        dynamicDescription = "$ x $")]
    [NumberArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public float Multiplication(float num1, float num2)
    {
        return num1 * num2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Division",
        dynamicDescription = "$ / $")]
    [NumberArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public float Division(float num1, float num2)
    {
        return num1 / num2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Square Root",
        dynamicDescription = "Square Root of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float SquareRoot(float num)
    {
        return Mathf.Sqrt(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Log",
        dynamicDescription = "Log of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Log(float num)
    {
        return Mathf.Log10(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Sin",
        dynamicDescription = "Sin of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Sin(float num)
    {
        return Mathf.Sin(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Cos",
        dynamicDescription = "Cos of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Cos(float num)
    {
        return Mathf.Sin(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Tan",
        dynamicDescription = "Tan of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Tan(float num)
    {
        return Mathf.Tan(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Floor",
        dynamicDescription = "Floor of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Floor(float num)
    {
        return Mathf.Ceil(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Ceiling",
        dynamicDescription = "Ceiling of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Ceiling(float num)
    {
        return Mathf.Ceil(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Round",
        dynamicDescription = "$ Rounded to Nearest Whole Number")]
    [NumberArg(argType = ArgType.Temp)]
    public float Round(float num)
    {
        return Mathf.Round(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Frac",
        dynamicDescription = "Fractional Part of $")]
    [NumberArg(argType = ArgType.Temp)]
    public float Frac(float num)
    {
        return num - Mathf.Floor(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "One Input Math/Ping Pong",
        dynamicDescription = "Ping Pong $")]
    [NumberArg(argType = ArgType.Temp)]
    public float PingPong(float num)
    {
        return Mathf.PingPong(num, 1);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random/Random Number",
        dynamicDescription = "Random Number Between $ and $")]
    [NumberArg(argType = ArgType.Value, defaultValue = 0)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public float RandomNumberBetween(float num1, float num2)
    {
        return Random.Range(num1, num2);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Noise/Perlin Noise 1D",
        dynamicDescription = "Perlin Noise with Input $")]
    [NumberArg(argType = ArgType.Temp)]
    public float PerlinNoise(float num)
    {
        return Mathf.PerlinNoise1D(num);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Noise/Perlin Noise 2D",
        dynamicDescription = "Perlin Noise with Inputs $ and $")]
    [NumberArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Temp)]
    public float PerlinNoise(float num1, float num2)
    {
        return Mathf.PerlinNoise(num1, num2);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Time/Time Since Level Start",
        dynamicDescription = "Time Since Level Start")]
    public float TimeSinceLevelStart()
    {
        return Time.timeSinceLevelLoad;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Time/Delta Time",
        dynamicDescription = "Delta Time")]
    public float DeltaTime()
    {
        return Time.deltaTime;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Time/Fixed Delta Time",
        dynamicDescription = "Fixed Delta Time")]
    public float FixedDeltaTime()
    {
        return Time.fixedDeltaTime;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Stat",
        dynamicDescription = "$ $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Buffs, allowPreset = false, allowFunction = false)]
    public float UnitStat(Unit unit, string statName)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Stat stat = StatExtensions.LabelToStat(statName);
        return unit.stats[stat].GetValue();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Health",
        dynamicDescription = "$ Health")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float UnitHealth(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.health;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Max Health",
        dynamicDescription = "$ Max Health")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float UnitMaxHealth(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.stats.GetValue(Stat.MaxHealth);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Health Percent",
        dynamicDescription = "$ Health Percent")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float HealthPercent(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.GetCurrentHealthPercent();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Resource",
        dynamicDescription = "$ Resource")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float UnitResource(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.resource;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Max Resource",
        dynamicDescription = "$ Max Resource")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float UnitMaxResource(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.stats.GetValue(Stat.MaxResource);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Unit Resource Percent",
        dynamicDescription = "$ Resource Percent")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float ResourcePercent(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.GetCurrentResourcePercent();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Level",
        dynamicDescription = "Player Level")]
    public float GetPlayerLevel()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.currentLevel;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Experience",
        dynamicDescription = "Player Experience")]
    public float GetPlayerExperience()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.currentExperience;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Player Gold",
        dynamicDescription = "Player Gold")]
    public float GetPlayerGold()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.currentGold;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Items Equipped",
        dynamicDescription = "Count equipped items on player")]
    public float GetPlayerItemsEquipped()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.equipment.GetFilledSlots();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Inventory Item Count",
        dynamicDescription = "Inventory Item Count")]
    public float GetPlayerInventoryItemCount()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.inventory.GetFilledSlots();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Inventory Available Slots",
        dynamicDescription = "Inventory Available Slots")]
    public float GetPlayerInventoryAvailableSlots()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.inventory.GetEmptySlots();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Monster/Monster Gold Modifier",
        dynamicDescription = "Gold Modifier of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetMonsterGoldModifier(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit is not Monster, VisualCodeLabels.Errors.UnitMustBeMonster);
        return ((Monster)unit).goldModifier;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Monster/Monster Experience Modifier",
        dynamicDescription = "Experience Modifier of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetMonsterExperienceModifier(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit is not Monster, VisualCodeLabels.Errors.UnitMustBeMonster);
        return ((Monster)unit).experienceModifier;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Monster/Monster Corpse Duration",
        dynamicDescription = "Corpse Duration of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetMonsterCorpseDuration(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit is not Monster, VisualCodeLabels.Errors.UnitMustBeMonster);
        return ((Monster)unit).corpseDuration;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Distance Between Points",
        dynamicDescription = "Distance between $ and $")]
    [VectorArg(argType = ArgType.Temp)]
    [VectorArg(argType = ArgType.Temp)]
    public float DistanceBetweenPoints(Vector3 vec1, Vector3 vec2)
    {
        return Vector3.Distance(vec1, vec2);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Distance Between Units",
        dynamicDescription = "Distance between $ and $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float DistanceBetweenUnits(Unit unit1, Unit unit2)
    {
        Error(unit1 == null || unit2 == null, VisualCodeLabels.Errors.InvalidUnit);
        return Vector3.Distance(unit1.transform.position, unit2.transform.position);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Vector Component",
        dynamicDescription = "$ Component of $")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.VectorComponents, allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    public float VectorComponent(string component, Vector3 vector)
    {
        switch (component)
        {
            case PresetStrings.X:
                return vector.x;
            case PresetStrings.Y:
                return vector.y;
            case PresetStrings.Z:
                return vector.z;
            default:
                return 0;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Count Units in Unit Group",
        dynamicDescription = "Number of units in $")]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public float CountUnitsInUnitGroup(UnitGroup unitGroup)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        return unitGroup.Count();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Base Range",
        dynamicDescription = "Base Range of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public float GetAbilityBaseRange(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.range;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Base Cooldown",
        dynamicDescription = "Base Cooldown of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public float GetAbilityBaseCooldown(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityCooldown;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Base Cost",
        dynamicDescription = "Base Cost of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public float GetAbilityBaseCost(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.abilityCost;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Base Generation",
        dynamicDescription = "Base Resource Regeneration of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public float GetAbilityBaseRegeneration(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        if (ability.abilityGeneratesResource)
            return ability.abilityCost;
        else
            return 0;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Ability/Ability Cast Time",
        dynamicDescription = "Base Cast Time of $")]
    [AbilityArg(argType = ArgType.Temp)]
    public float GetAbilityBaseCastTime(Ability ability)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability.castTime;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Items Equipped From Set",
        dynamicDescription = "Items equipped from set $")]
    [ItemSetArg(argType = ArgType.Temp)]
    public float GetItemsEquippedFromSet(ItemSet itemSet)
    {
        Error(itemSet == null, VisualCodeLabels.Errors.InvalidItemSet);
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        return GameManager.player.CountItemsEquippedFromSet(itemSet);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Gold Cost",
        dynamicDescription = "Gold Cost of $")]
    [ItemArg(argType = ArgType.Temp)]
    public float GetGoldDropPercentChance(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.itemCost;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Random Stat Count",
        dynamicDescription = "Random Stat Count of $")]
    [ItemArg(argType = ArgType.Temp)]
    public float GetRandomStatCount(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.randomStatCount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Total Stat Count",
        dynamicDescription = "Total Stat Count of $")]
    [ItemArg(argType = ArgType.Temp)]
    public float GetTotalStatCount(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.randomStatCount + item.guaranteedStats.Count;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Item/Item Minimum Drop Level",
        dynamicDescription = "Minimum Drop Level of $")]
    [ItemArg(argType = ArgType.Temp)]
    public float GetMinimumDropLevel(Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        return item.minimumDropLevel;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Region/Region Count",
        dynamicDescription = "Number of regions named $")]
    [StringArg(argType = ArgType.Temp)]
    public float GetNumberOfRegions(string regionName)
    {
        return Region.GetRegionsWithName(regionName).Count;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Current Progress of Quest Requirement",
        dynamicDescription = "Current Increment of $ requirement on quest $")]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp)]
    public float GetCurrentQuestRequirementProgress(string requirementName, string questName)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        Quest.QuestItem requirement = quest.GetQuestRequirement(requirementName);
        Error(requirement == null, VisualCodeLabels.Errors.InvalidQuestRequirementName);
        return requirement.CurrentProgress;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quest/Maximum Progress of Quest Requirement",
        dynamicDescription = "Maximum Increment of $ requirement on quest $")]
    [StringArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp)]
    public float GetMaxQuestRequirementProgress(string requirementName, string questName)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        Quest.QuestItem requirement = quest.GetQuestRequirement(requirementName);
        Error(requirement == null, VisualCodeLabels.Errors.InvalidQuestRequirementName);
        return requirement.MaxProgress;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Stack Count on Unit",
        dynamicDescription = "Stack Count of $ on $")]
    [BuffArg(argType = ArgType.Temp)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetBuffStacks(Buff buff, Unit unit)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Buff unitBuff = unit.buffs.GetUnitBuff(buff);
        Error(unitBuff == null, VisualCodeLabels.Errors.InvalidBuff);
        return unitBuff.buffCurrentStacks;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Max Stack Count on Unit",
        dynamicDescription = "Max Stack Count of $ on $")]
    [BuffArg(argType = ArgType.Temp)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetBuffMaxStacks(Buff buff, Unit unit)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Buff unitBuff = unit.buffs.GetUnitBuff(buff);
        Error(unitBuff == null, VisualCodeLabels.Errors.InvalidBuff);
        return unitBuff.buffMaximumStacks;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Total Duration on Unit",
        dynamicDescription = "Total duration of $ on $")]
    [BuffArg(argType = ArgType.Temp)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetBuffMaxDuration(Buff buff, Unit unit)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Buff unitBuff = unit.buffs.GetUnitBuff(buff);
        Error(unitBuff == null, VisualCodeLabels.Errors.InvalidBuff);
        return unitBuff.buffMaxDuration;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Buff/Buff Current Duration on Unit",
        dynamicDescription = "Current duration of $ on $")]
    [BuffArg(argType = ArgType.Temp)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public float GetBuffCurrentDuration(Buff buff, Unit unit)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Buff unitBuff = unit.buffs.GetUnitBuff(buff);
        Error(unitBuff == null, VisualCodeLabels.Errors.InvalidBuff);
        return unitBuff.buffCurrentDuration;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Gold Drop Percent Chance",
        dynamicDescription = "Gold Drop Percent Chance")]
    public float GetGoldDropPercentChance()
    {
        return GameManager.monsterValues.goldDropChance;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Base Gold Drop Amount Minimum",
        dynamicDescription = "Base Gold Drop Amount")]
    public float GetBaseGoldDropAmountMinimum()
    {
        return GameManager.monsterValues.baseGoldDropAmountMinimum;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Base Gold Drop Amount Maximum",
        dynamicDescription = "Base Gold Drop Amount")]
    public float GetBaseGoldDropAmountMaximum()
    {
        return GameManager.monsterValues.baseGoldDropAmountMaximum;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Health Pickup Drop Chance",
        dynamicDescription = "Health Pickup Drop Chance")]
    public float GetHealthPickupDropChance()
    {
        return GameManager.healthGlobeValues.baseHealthGlobeChance;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Health Pickup Heal Amount",
        dynamicDescription = "Health Pickup Heal Amount")]
    public float GetHealthPickupHealAmount()
    {
        return GameManager.healthGlobeValues.healthGlobeHealthRestore;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Health Pickup Lifetime",
        dynamicDescription = "Health Pickup Lifetime")]
    public float GetHealthPickupLifetime()
    {
        return GameManager.healthGlobeValues.lifetime;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Item Sell Return Percentage",
        dynamicDescription = "Item Sell Return Percentage")]
    public float GetItemSellPercentage()
    {
        return GameManager.inventoryValues.sellItemReturnRate;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Base Monster XP",
        dynamicDescription = "Base Monster XP")]
    public float GetBaseMonsterXP()
    {
        return GameManager.playerExperienceValues.baseMonsterXP;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Global Spawn Density",
        dynamicDescription = "Global Spawn Density")]
    public float GetGlobalSpawnDensity()
    {
        return GameManager.spawner.spawnDensity;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variable/Number Variable",
        dynamicDescription = "Number Variable: $")]
    [StringArg(argType = ArgType.Value, defaultValue = "NumberVar", allowFunction = false, allowPreset = false)]
    public float GetNumberVariable(string name)
    {
        return LogicEngine.current.GetLocalVariable<float>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variable/Global Number Variable",
        dynamicDescription = "Global Number: $")]
    [StringArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    public float GetGlobalNumberVariable(string name)
    {
        return LogicEngine.GetGlobalVariable<float>(name);
    }
}
