using MyUtilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VisualCodeScript
{
    public Unit GetOwner()
    {
        return LogicEngine.current.GetOwner();
    }

    public Unit GetPlayer()
    {
        return GameManager.player;
    }

    public Unit GetLastCreatedUnit()
    {
        return lastCreatedUnit;
    }

    public Unit GetTriggeringUnit()
    {
        return triggeringUnit;
    }

    public Unit GetDamagingUnit()
    {
        return damagingUnit;
    }

    public Unit GetKillingUnit()
    {
        return killingUnit;
    }

    public Unit GetHealingUnit()
    {
        return healingUnit;
    }

    public Unit GetBuffOwner ()
    {
        Error(LogicEngine.current.engineHandler is not Buff, VisualCodeLabels.Errors.NoBuffOwner);
        return ((Buff)LogicEngine.current.engineHandler).owner;
    }

    public float BuffStackCount ()
    {
        Error(LogicEngine.current.engineHandler is not Buff, VisualCodeLabels.Errors.NoBuffStackCount);
        Unit unit = GetBuffOwner();
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Buff buff = ((Buff)LogicEngine.current.engineHandler);
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Buff unitBuff = unit.buffs.GetUnitBuff(buff);
        Error(unitBuff == null, VisualCodeLabels.Errors.InvalidBuff);
        return unitBuff.buffCurrentStacks;
    }

    public Unit GetBuffApplier ()
    {
        Error(LogicEngine.current.engineHandler is not Buff, VisualCodeLabels.Errors.NoBuffApplier);
        return ((Buff)LogicEngine.current.engineHandler).applier;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Closest Unit Near Point",
        dynamicDescription = "Closest unit within $ distance of $")]
    [NumberArg(argType = ArgType.Temp, suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    public Unit ClosestUnitToPoint(float distance, Vector3 point)
    {
        return Utilities.GetClosest<Unit>(point, distance);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Furthest Unit Near Point",
        dynamicDescription = "Furthest unit within $ distance of $")]
    [NumberArg(argType = ArgType.Temp, suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    public Unit FurthestUnitToPoint(float distance, Vector3 point)
    {
        return Utilities.GetFurthest<Unit>(point, distance);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random Unit Near Point",
        dynamicDescription = "Random unit within $ distance of $")]
    [NumberArg(argType = ArgType.Temp, suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    public Unit RandomNearbyUnitToPoint(float distance, Vector3 point)
    {
        List<Unit> units = Utilities.GetAllWithinRange<Unit>(point, distance);
        if (units.Count == 0) return null;
        return units[Random.Range(0, units.Count)];
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit with Label",
        dynamicDescription = "Unit labeled $")]
    [StringArg(argType = ArgType.Temp)]
    public Unit GetUnitWithLabel(string label)
    {
        Monster[] monsters = GameObject.FindObjectsByType<Monster>(FindObjectsSortMode.None);
        foreach (Monster monster in monsters) if (monster.monsterLabel == label) return monster;
        return null;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit Variable",
        dynamicDescription = "Unit Variable: $")]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitVar")]
    public Unit GetUnitVariable(string name)
    {
        return LogicEngine.current.GetLocalVariable<Unit>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Unit Variable",
        dynamicDescription = "Global Unit: $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name")]
    public Unit GetGlobalUnitVariable(string name)
    {
        return LogicEngine.GetGlobalVariable<Unit>(name);
    }

    public Unit PopUnitFromUnitGroup(UnitGroup unitGroup)
    {
        Error(unitGroup == null, "The specified unit group is invalid.");
        return unitGroup.PopNextUnit();
    }
}
