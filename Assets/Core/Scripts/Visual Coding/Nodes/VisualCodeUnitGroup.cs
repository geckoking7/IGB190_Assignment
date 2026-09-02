using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Burst.Intrinsics;
using UnityEngine;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Empty Unit Group",
        dynamicDescription = "Empty Unit Group")]
    public UnitGroup EmptyUnitGroup()
    {
        return UnitGroup.Empty();
    }

    public UnitGroup AllUnits()
    {
        return new UnitGroup(GameObject.FindObjectsByType<Unit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None));
    }

    public UnitGroup AllEnemies()
    {
        Unit.Faction ownerFaction = Unit.Faction.Player;
        if (LogicEngine.current != null && LogicEngine.current.GetOwner() != null)
            ownerFaction = LogicEngine.current.GetOwner().GetFaction();

        List<Unit> enemies = new List<Unit>();
        Unit[] units = GameObject.FindObjectsByType<Unit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Unit unit in units)
            if (unit.GetFaction() != ownerFaction)
                enemies.Add(unit);

        return new UnitGroup(enemies);
    }

    public UnitGroup AllAllies()
    {
        Unit.Faction ownerFaction = Unit.Faction.Player;
        if (LogicEngine.current != null && LogicEngine.current.GetOwner() != null)
            ownerFaction = LogicEngine.current.GetOwner().GetFaction();

        List<Unit> allies = new List<Unit>();
        Unit[] units = GameObject.FindObjectsByType<Unit>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Unit unit in units)
            if (unit.GetFaction() == ownerFaction)
                allies.Add(unit);

        return new UnitGroup(allies);
    }

    public UnitGroup AllNonPlayerAllies()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        UnitGroup allies = AllAllies();
        allies.RemoveUnit(GameManager.player);
        return allies;
    }

    public UnitGroup AllMonsters()
    {
        Unit[] units = GameObject.FindObjectsByType<Monster>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        return new UnitGroup(units);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Units Near Point",
        dynamicDescription = "$ within $ of $")]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    public UnitGroup FilterUnitsWithinRangeOfPoint(UnitGroup unitGroup, float distance, Vector3 point)
    {
        if (unitGroup == null || unitGroup.unitList.Count == 0)
            return new UnitGroup();

        List<Unit> finalUnits = new List<Unit>();
        foreach (Unit unit in unitGroup)
            if (Vector3.Distance(unit.transform.position, point) <= distance)
                finalUnits.Add(unit);

        return new UnitGroup(finalUnits);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Units with Tag",
        dynamicDescription = "$ with tag $")]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    [StringArg(argType = ArgType.Temp)]
    public UnitGroup FilterUnitsWithTag(UnitGroup unitGroup, string tag)
    {
        List<Unit> finalUnits = new List<Unit>();
        foreach (Unit unit in unitGroup)
            if (unit.CompareTag(tag))
                finalUnits.Add(unit);

        return new UnitGroup(finalUnits);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Units with Buff",
        dynamicDescription = "$ with buff $")]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    [StringArg(argType = ArgType.Temp)]
    public UnitGroup FilterUnitsWithBuff(UnitGroup unitGroup, string buff)
    {
        List<Unit> finalUnits = new List<Unit>();
        foreach (Unit unit in unitGroup)
            if (unit.HasBuff(buff))
                finalUnits.Add(unit);

        return new UnitGroup(finalUnits);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Units In Arc from Unit",
        dynamicDescription = "$ in $ arc from $ extending $")]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    [NumberArg(argType = ArgType.Value, defaultValue = 90, suffix = "\u00BA")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "m")]
    public UnitGroup FilterUnitsInArcFromUnit(UnitGroup unitGroup, float arc, Unit unit, float length)
    {
        Error(unit == null, "The specified unit is invalid.");
        return FilterUnitsInArcBetweenPoints(
            unitGroup,
            arc,
            unit.transform.position,
            unit.transform.position + unit.transform.forward * length);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Units In Arc Between Points",
        dynamicDescription = "$ in $ arc from $ to $")]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    [NumberArg(argType = ArgType.Value, defaultValue = 90, suffix = "u00BA")]
    [VectorArg(argType = ArgType.Temp)]
    [VectorArg(argType = ArgType.Temp)]
    public UnitGroup FilterUnitsInArcBetweenPoints(UnitGroup unitGroup, float arc, Vector3 from, Vector3 to)
    {
        float distance = Vector3.Distance(from, to);
        List<Unit> matches = new List<Unit>();
        Vector3 compare = (to - from).normalized;
        float threshold = (arc - 180) / -180;
        foreach (Unit unit in unitGroup)
        {
            float dot = Vector3.Dot(compare, (unit.transform.position - from).normalized);
            if (dot > threshold && Vector3.Distance(from, unit.transform.position) < distance)
            {
                matches.Add(unit);
            }
        }
        return new UnitGroup(matches);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random X Units",
        dynamicDescription = "Random $ units in $")]
    [NumberArg(argType = ArgType.Value, defaultValue = 3)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public UnitGroup RandomXUnits(float unitCount, UnitGroup unitGroup)
    {
        if (unitGroup == null || unitGroup.unitList.Count == 0)
            return new UnitGroup();

        return new UnitGroup(unitGroup.unitList
            .OrderBy(u => Random.value)
            .Take((int)unitCount)
            .ToList());
    }

    [VisualScriptingFunction(
        dropdownDescription = "Furthest X Units from Point",
        dynamicDescription = "Furthest $ units from $ in $")]
    [NumberArg(argType = ArgType.Value, defaultValue = 3)]
    [VectorArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    public UnitGroup FurthestXUnitsFromPoint(float unitCount, Vector3 point, UnitGroup unitGroup)
    {
        if (unitGroup == null || unitGroup.unitList.Count == 0)
            return new UnitGroup();

        List<Unit> unitsSorted = unitGroup.unitList
            .OrderBy(u => Vector3.SqrMagnitude(u.transform.position - point))
            .Take((int)unitCount)
            .ToList();

        return new UnitGroup(unitsSorted);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Closest X Units from Point",
        dynamicDescription = "Closest $ units from $ in $")]
    [NumberArg(argType = ArgType.Value, defaultValue = 3)]
    [VectorArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Preset, allowValue = false, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES)]
    public UnitGroup ClosestXUnitsFromPoint(float unitCount, Vector3 point, UnitGroup unitGroup)
    {
        if (unitGroup == null || unitGroup.unitList.Count == 0)
            return new UnitGroup();

        List<Unit> unitsSorted = unitGroup.unitList
            .OrderBy(u => -Vector3.SqrMagnitude(u.transform.position - point))
            .Take((int)unitCount)
            .ToList();

        return new UnitGroup(unitsSorted);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit Group Variable",
        dynamicDescription = "Unit Group: $")]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitGroupVar")]
    public UnitGroup GetUnitGroupVariable(string name)
    {
        if (!LogicEngine.current.LocalVariableExists(name)) return UnitGroup.Empty(); 
        return LogicEngine.current.GetLocalVariable<UnitGroup>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Unit Group Variable",
        dynamicDescription = "Global Unit Group: $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name")]
    public UnitGroup GetGlobalUnitGroupVariable(string name)
    {
        if (!LogicEngine.GlobalVariableExists(name)) return UnitGroup.Empty();
        return LogicEngine.GetGlobalVariable<UnitGroup>(name);
    }
}
