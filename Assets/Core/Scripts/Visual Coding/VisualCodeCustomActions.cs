using MyUtilities;
using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Set Unit(s) Targetable",
        dynamicDescription = "Set $ targetting status to $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    public void SetTargetable(UnitGroup units, bool targetable)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SetTargetableStatus(targetable);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random/Random Unit",
        dynamicDescription = "Random Spawnable Unit")]
    public Unit RandomSpawnableUnit ()
    {
        return GameManager.spawner.monstersToSpawn[UnityEngine.Random.Range(0, GameManager.spawner.monstersToSpawn.Count)];
    }
}