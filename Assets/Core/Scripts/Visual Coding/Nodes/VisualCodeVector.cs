using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class VisualCodeScript
{
    [VisualScriptingFunction(
        dropdownDescription = "Math/Vector Addition",
        dynamicDescription = "$ + $")]
    [VectorArg(argType = ArgType.Temp)]
    [VectorArg(argType = ArgType.Temp)]
    public Vector3 VectorAddition(Vector3 vec1, Vector3 vec2)
    {
        return vec1 + vec2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Vector Subtraction",
        dynamicDescription = "$ - $")]
    [VectorArg(argType = ArgType.Temp)]
    [VectorArg(argType = ArgType.Temp)]
    public Vector3 VectorSubtraction(Vector3 vec1, Vector3 vec2)
    {
        return vec1 - vec2;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Vector Multiplication",
        dynamicDescription = "$ x $")]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Temp)]
    public Vector3 VectorMultiplication(Vector3 vec1, float value)
    {
        return vec1 * value;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Math/Vector Division",
        dynamicDescription = "$ / $")]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Temp)]
    public Vector3 VectorDivision(Vector3 vec1, float value)
    {
        return vec1 / value;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random/Random Point Near Unit",
        dynamicDescription = "Random point within $ of $")]
    [NumberArg(argType = ArgType.Temp, tempLabel = "Range", suffix = "m")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 RandomPointNearUnit(float distance, Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Vector3 offset = Random.insideUnitSphere * distance;
        offset.y = 0;
        return unit.transform.position + offset;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Random/Random Point Near Point",
        dynamicDescription = "Random point within $ of $")]
    [NumberArg(argType = ArgType.Temp, tempLabel = "Range", suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    public Vector3 RandomPointNearPoint(float distance, Vector3 point)
    {
        Vector3 offset = Random.insideUnitSphere * distance;
        offset.y = 0;
        return point + offset;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Position of Unit",
        dynamicDescription = "Position of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 PositionOfUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.transform.position;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Cast Point of Unit",
        dynamicDescription = "Cast Point of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 CastPointOfUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.GetCastPoint();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Attach Point on Unit",
        dynamicDescription = "$ of $")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.AttachPoints, allowPreset = false, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 AttachPointOfUnit(string attachPoint, Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (attachPoint == PresetStrings.CastPoint)
            return (unit.castPoint != null ? unit.castPoint.transform.position : unit.transform.position);
        if (attachPoint == PresetStrings.Head)
            return (unit.head != null ? unit.head.transform.position : unit.transform.position);
        if (attachPoint == PresetStrings.Center)
            return (unit.center != null ? unit.center.transform.position : unit.transform.position);
        if (attachPoint == PresetStrings.LeftHand)
            return (unit.leftHand != null ? unit.leftHand.transform.position : unit.transform.position);
        if (attachPoint == PresetStrings.RightHand)
            return (unit.rightHand != null ? unit.rightHand.transform.position : unit.transform.position);
        if (attachPoint == PresetStrings.Origin)
            return (unit.origin != null ? unit.origin.transform.position : unit.transform.position);
        return unit.transform.position;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Attack Point of Unit",
        dynamicDescription = "Attack Point of $")]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 AttackPointOfUnit(Unit unit)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        return unit.GetAttackPoint();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Position of Projectile",
        dynamicDescription = "Position of $")]
    [ProjectileArg(argType = ArgType.Temp, allowValue = false)]
    public Vector3 PositionOfProjectile(Projectile projectile)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        return projectile.transform.position;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Vector Variable",
        dynamicDescription = "Vector Variable: $")]
    [StringArg(argType = ArgType.Value, defaultValue = "VectorVar")]
    public Vector3 GetVectorVariable(string name)
    {
        return LogicEngine.current.GetLocalVariable<Vector3>(name);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Global Vector Variable",
        dynamicDescription = "Global Vector: $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name")]
    public Vector3 GetGlobalVectorVariable(string name)
    {
        return LogicEngine.GetGlobalVariable<Vector3>(name);
    }
}
