using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VisualCodeScript
{
    public Ability LastAbilityCast ()
    {
        return lastAbilityCast; 
    }

    public Ability ThisAbility()
    {
        return (Ability)LogicEngine.current.engineHandler;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random Ability on Unit",
        dynamicDescription = "Random Ability by $")]
    [UnitArg(argType = ArgType.Temp)]
    public Ability RandomAbilityOnUnit (Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit.abilities.Count == 0, VisualCodeLabels.Errors.NoAbilities);
        Ability ability = unit.abilities[Random.Range(0, unit.abilities.Count)];
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        return ability;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Last Ability Cast by Unit",
        dynamicDescription = "Last Ability Cast by $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Ability LastAbilityCastByUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.lastAbilityCast;
    }
}
