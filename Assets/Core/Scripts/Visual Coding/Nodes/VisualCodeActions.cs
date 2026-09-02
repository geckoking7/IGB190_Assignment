using MyUtilities;
using System;
using System.Collections;
using Unity.VisualScripting; 
using UnityEngine;
using UnityEngine.SceneManagement;
using Color = UnityEngine.Color;

public partial class VisualCodeScript
{
    #region Flow Actions

    [VisualScriptingFunction(
        dropdownDescription = "Flow/Wait",
        dynamicDescription = "Wait for $ second(s)",
        icon = waitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void Wait(float duration)
    {
        // This is a special action. It is processed seperately as part of the core visual scripting execution logic.
        // See the RunAllActions method in the VisualCodeScript.cs class for details.
    } 

    [VisualScriptingFunction(
        dropdownDescription = "Flow/If Statement",
        dynamicDescription = "Do actions if $",
        allowsChildren = true,
        icon = conditionIcon)]
    [BoolArg(argType = ArgType.Temp)]
    public void DoActionsIfBool(bool condition)
    {
        // This is a special action. It is processed seperately as part of the core visual scripting execution logic.
        // See the RunAllActions method in the VisualCodeScript.cs class for details.
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/While Loop",
        dynamicDescription = "Do actions while $",
        allowsChildren = true,
        icon = loopIcon)]
    [BoolArg(argType = ArgType.Temp)]
    public void DoActionsWhileBool(bool condition)
    {
        // This is a special action. It is processed seperately as part of the core visual scripting execution logic.
        // See the RunAllActions method in the VisualCodeScript.cs class for details.
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/For Loop",
        dynamicDescription = "Do actions $ times (Variable Storing Current Iteration: $)",
        allowsChildren = true,
        icon = loopIcon)]
    [NumberArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, defaultValue = "Loop ID")]
    public void DoActionsXTimesStoringVariable(float times, string variable)
    {
        // This is a special action. It is processed seperately as part of the core visual scripting execution logic.
        // See the RunAllActions method in the VisualCodeScript.cs class for details.
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/For Each Unit in Unit Group",
        dynamicDescription = "For Each Unit in $ (Variable Storing Current Unit: $)",
        allowsChildren = true,
        icon = loopIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Value, defaultValue = "Unit")]
    public void ForEachUnitInGroup(UnitGroup group, string variable)
    {
        // This is a special action. It is processed seperately as part of the core visual scripting execution logic.
        // See the RunAllActions method in the VisualCodeScript.cs class for details.
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/Disable This Script",
        dynamicDescription = "Disable this script",
        icon = cancelIcon)]
    public void DisableScript()
    {
        LogicEngine.current.disabledScripts[this] = float.MaxValue;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/Disable This Script for Duration",
        dynamicDescription = "Disable this script for $",
        icon = cancelIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void DisableScriptForDuration(float duration)
    {
        Error(duration <= 0, "The duration to disable the script must be greater than zero.");
        float enabledAt = Time.time + duration;
        if (!LogicEngine.current.disabledScripts.ContainsKey(this) ||
            LogicEngine.current.disabledScripts[this] < enabledAt)
            LogicEngine.current.disabledScripts[this] = enabledAt;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Flow/Send Event Message",
        dynamicDescription = "Send event message with label $",
        icon = eventIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Label")]
    public void SendEventMessage(string message)
    {
        Error(message == null || message.Length == 0, VisualCodeLabels.Errors.InvalidText);
        GameManager.SendEventMessage(message);
    }

    #endregion 

    #region Unit Actions

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Visuals/Spin Unit(s)",
        dynamicDescription = "Spin $ by $ for $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 360, suffix = "\u00BA/s")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void SpinUnits(UnitGroup units, float speed, float duration)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.StartSpin(speed, duration);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Spawn/Spawn Unit(s)",
        dynamicDescription = "Spawn $ $ for $ faction at $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    [UnitArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Factions, allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SpawnUnits(float count, Unit unit, string faction, Vector3 position)
    {
        const int MAX_SPAWNS = 100; // It would be bad if you spawned more than this...
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        int unitCount = Mathf.Clamp((int)count, 0, MAX_SPAWNS); 
        Unit.Faction fact = (Unit.Faction)Enum.Parse(typeof(Unit.Faction), faction);
        for (int i = 0; i < unitCount; i++)
        {
            GameManager.spawner.SpawnMonster((Monster)unit, position, fact, false, true);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Spawn/Spawn Empowered Unit(s)",
        dynamicDescription = "Spawn $ empowered $ for $ faction at $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    [UnitArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Factions, allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SpawnEmpoweredUnits(float count, Unit unit, string faction, Vector3 position)
    {
        const int MAX_SPAWNS = 100; // It would be bad if you spawned more than this...
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        int unitCount = Mathf.Clamp((int)count, 0, MAX_SPAWNS);
        Unit.Faction fact = (Unit.Faction)Enum.Parse(typeof(Unit.Faction), faction);
        for (int i = 0; i < unitCount; i++)
        {
            GameManager.spawner.SpawnMonster((Monster)unit, position, fact, true, true);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Resources/Add Health to Unit(s)",
        dynamicDescription = "Add $ health to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    public void AddHealthToUnits(float amount, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.AddHealth(amount);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Resources/Remove Health from Unit(s)",
        dynamicDescription = "Remove $ health from $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    public void RemoveHealthFromUnits(float amount, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.RemoveHealth(amount);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Resources/Add Resource to Unit(s)",
        dynamicDescription = "Add $ resource to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    public void AddResourceToUnits(float amount, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.AddResource(amount);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Resources/Remove Resource from Unit(s)",
        dynamicDescription = "Remove $ resource from $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    public void RemoveResourcesfromUnits(float amount, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.RemoveResource(amount);
        }
    }

    public void SpawnUnitWithEffect(Unit unit, Vector3 position, UnitSpawnEffect spawnEffect, bool isEmpowered = false, Unit.Faction faction = Unit.Faction.Enemy)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(spawnEffect == null, VisualCodeLabels.Errors.InvalidEffect);
        GameManager.instance.StartCoroutine(SpawnUnitCoroutine(unit, position, spawnEffect, isEmpowered, faction));
    }

    private IEnumerator SpawnUnitCoroutine(Unit unit, Vector3 position, UnitSpawnEffect spawnEffect, bool isEmpowered = false, Unit.Faction faction = Unit.Faction.Enemy)
    {
        float duration = spawnEffect.GetComponent<UnitSpawnEffect>().effectDuration;
        ObjectPooler.InstantiatePooled(spawnEffect.gameObject, position, Quaternion.identity);
        yield return new WaitForSeconds(duration);
        Unit u = GameObject.Instantiate(unit, position, Quaternion.identity);
        if (isEmpowered && u is Monster monster) monster.Empower();
        u.SetFaction(faction);
        IVisualCodeHandler engine = LogicEngine.current.engineHandler;
        Unit spawningUnit = engine.GetOwner();
        GameManager.events.OnUnitSpawned.Invoke(new GameEvents.OnUnitSpawnedInfo(u, spawningUnit));
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Kill Unit(s)",
        dynamicDescription = "Kill $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void KillUnits(UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        Unit killingUnit = LogicEngine.current.engineHandler.GetOwner();
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.Kill(killingUnit, LogicEngine.current.engineHandler);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Visuals/Play Animation on Unit(s)",
        dynamicDescription = "Play $ animation on $",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Animation Name")]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void PlayAnimationOnUnits(string animation, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        Error(animation == null || animation.Length == 0, VisualCodeLabels.Errors.InvalidText);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.PlayAnimation(animation);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Deal Damage to Unit(s)",
        dynamicDescription = "Deal $ attack damage to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100, suffix = "%")]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void DamageUnits(float amount, UnitGroup unitGroup)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        IVisualCodeHandler engine = LogicEngine.current.engineHandler;
        Unit owner = engine.GetOwner();
        amount /= 100.0f;
        if (amount <= 0) return;
        foreach (Unit unit in unitGroup)
        {
            if (unit == null) continue;
            if (owner == null)
                unit.TakeDamage(amount, false, owner, engine);
            else
                owner.DamageOtherUnit(unit, amount, engine);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Have Unit Deal Damage to Unit(s)",
        dynamicDescription = "Have $ Deal $ attack damage to $",
        icon = unitIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100, suffix = "%")]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void HaveUnitDamageUnits(Unit damager, float amount, UnitGroup unitGroup)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        IVisualCodeHandler engine = LogicEngine.current.engineHandler;
        amount /= 100.0f;
        if (amount <= 0) return;
        foreach (Unit unit in unitGroup)
        {
            if (unit == null) continue;
            if (damager == null)
                unit.TakeDamage(amount, false, damager, engine);
            else
                damager.DamageOtherUnit(unit, amount, engine);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Stun Unit(s)",
        dynamicDescription = "Stun $ for $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    public void StunUnits(UnitGroup unitGroup, float duration)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        IVisualCodeHandler engine = LogicEngine.current.engineHandler;
        Unit stunningUnit = engine == null ? null : engine.GetOwner();
        foreach (Unit unit in unitGroup)
        {
            if (unit == null) continue;
            unit.Stun(duration, stunningUnit, engine);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Heal Unit(s)",
        dynamicDescription = "Restore $ health to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void HealUnits(float amount, UnitGroup unitGroup)
    {
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        IVisualCodeHandler engine = LogicEngine.current.engineHandler;
        Unit owner = engine.GetOwner();
        foreach (Unit unit in unitGroup)
        {
            if (unit == null) continue;
            if (owner == null)
                unit.AddHealth(amount, owner, engine);
            else
                owner.HealOtherUnit(unit, amount, engine);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Movement/Teleport Unit(s)",
        dynamicDescription = "Teleport $ to $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void TeleportUnits(UnitGroup units, Vector3 newPosition)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.Teleport(newPosition);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Movement/Move Unit(s) Over Time",
        dynamicDescription = "Move $ to $ over $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void MoveUnitsOverTime(UnitGroup units, Vector3 newPosition, float duration)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.MoveOverTime(newPosition, duration);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Movement/Force Unit(s) to Move",
        dynamicDescription = "Force $ to walk to $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void ForceMoveUnit(UnitGroup units, Vector3 position)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.ForceMove(position);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Movement/Push Unit(s) Away from Point",
        dynamicDescription = "Push $ $ away from $ over $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "m")]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void PushUnitsAwayFromPoint(UnitGroup units, float distance, Vector3 position, float duration)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            Vector3 direction = (unit.transform.position - position).normalized;
            unit.MoveOverTime(unit.transform.position + direction * distance, duration);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Modify Stat on Unit(s) by Percent",
        dynamicDescription = "$ $ of $ by $ for $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowPreset = false, allowFunction = false)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Buffs, allowPreset = false, allowFunction = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Temp, tempLabel = "Percent", suffix = "%")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "s")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ModifyStatPercent(string increaseDecrease, string modifier, UnitGroup units, float mod, float duration, string buff, float maxStacks)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        Stat stat = StatExtensions.LabelToStat(modifier);

        bool increase = false;
        if (increaseDecrease == PresetStrings.Increase) increase = true;

        if (increase)
            mod = Mathf.Max(0, mod / 100.0f);
        else
            mod = Mathf.Min(0, - mod / 100f);

        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.stats[stat].AddTimedPercentageModifier(mod, duration, buff, (int)maxStacks);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Combat/Remove Buff from Unit(s)",
        dynamicDescription = "Remove buff named $ from $",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void RemoveBuff(string buffName, UnitGroup units)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.stats.RemoveBuffWithLabel(buffName);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Cast Ability",
        dynamicDescription = "Have $ Cast $ (Cast Time: $ | Apply Restrictions: $)",
        icon = unitIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    public void CastAbility(Unit unit, Ability ability, bool payCost, bool castTime)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Ability abilityCopy = ability.ShallowCopy();
        abilityCopy.SetOwner(unit);
        Debug.Log($"{payCost} | {castTime}");
        unit.CastAbility(abilityCopy, null, unit.transform.position, payCost, castTime);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Cast Ability at Position",
        dynamicDescription = "Have $ Cast $ at $ (Cast Time: $ | Apply Restrictions: $)",
        icon = unitIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    public void CastAbilityAtPosition(Unit unit, Ability ability, Vector3 position, bool payCost, bool castTime)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Ability abilityCopy = ability.ShallowCopy();
        abilityCopy.SetOwner(unit);
        unit.CastAbility(abilityCopy, null, unit.transform.position, payCost, castTime);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Cast Ability on Unit",
        dynamicDescription = "Have $ Cast $ on $ (Cast Time: $ | Apply Restrictions: $)",
        icon = unitIcon)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = false)]
    public void CastAbilityOnUnit(Unit unit, Ability ability, Unit target, bool payCost, bool castTime)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Ability abilityCopy = ability.ShallowCopy();
        abilityCopy.SetOwner(unit);
        unit.CastAbility(abilityCopy, target, unit.transform.position, payCost, castTime);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Enable Ability on Unit",
        dynamicDescription = "Enable $ on $ (Show UI Message: $)",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = true)]
    public void EnableAbility(Ability ability, Unit unit, bool showUIMessage)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        for (int i = 0; i < unit.abilities.Count; i++)
        {
            if (unit.abilities[i].name == ability.name)
            {
                bool needsReminderFlash = unit == GameManager.player && showUIMessage && !unit.abilities[i].isUnlocked;

                if (needsReminderFlash) GameManager.ui.NotificationWindow.DisplayMessage("Ability Unlocked", ability.name, ability.abilityIcon);
                unit.abilities[i].Unlock(needsReminderFlash);
            }
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Disable Ability on Unit",
        dynamicDescription = "Disable $ on $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false, allowPreset = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void DisableAbility(Ability ability, Unit unit)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        for (int i = 0; i < unit.abilities.Count; i++)
        {
            if (unit.abilities[i].name == ability.name)
            {
                unit.abilities[i].Lock();
            }
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Reduce Current Ability Cooldown",
        dynamicDescription = "Reduce current cooldown of $ on $ by $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void ReduceAbilityCooldown(Ability ability, Unit unit, float amount)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        unit.ReduceAbilityCooldown(ability, amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Ability",
        dynamicDescription = "Add $ to $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void AddAbilityToUnit(Ability ability, Unit unit)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        unit.AddAbility(ability);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Remove Ability",
        dynamicDescription = "Remove $ from $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void RemoveAbilityFromUnit(Ability ability, Unit unit)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        unit.RemoveAbility(ability);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Replace Ability",
        dynamicDescription = "Replace $ with $ on $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void ReplaceAbilityOnUnit(Ability oldAbility, Ability newAbility, Unit unit)
    {
        Error(oldAbility == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(newAbility == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        unit.ReplaceAbility(oldAbility, newAbility);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Cost Modifier to Ability",
        dynamicDescription = "$ cost of $ on $ by $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddAbilityCostModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddAbilityCostModifier(ability, modifier, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Cooldown Modifier to Ability",
        dynamicDescription = "$ cooldown of $ on $ by $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddAbilityCooldownModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddAbilityCooldownModifier(ability, modifier, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Damage Modifier to Ability",
        dynamicDescription = "$ damage of $ on $ by $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddAbilityDamageModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddAbilityDamageModifier(ability, modifier / 100.0f, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Timed Cost Modifier to Ability",
        dynamicDescription = "$ cost of $ on $ by $ for $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "s")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddTimedAbilityCostModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, float duration, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddTimedAbilityCostModifier(ability, modifier, duration, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Timed Cooldown Modifier to Ability",
        dynamicDescription = "$ cooldown of $ on $ by $ for $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "s")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddTimedAbilityCooldownModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, float duration, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddTimedAbilityCooldownModifier(ability, modifier, duration, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Add Timed Damage Modifier to Ability",
        dynamicDescription = "$ damage of $ on $ by $ for $ (Buff Name: $ | Max Stacks: $)",
        icon = unitIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.IncreaseDecrease, allowFunction = false)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5, suffix = "s")]
    [StringArg(argType = ArgType.Value, defaultValue = "None")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddTimedAbilityDamageModifierToUnit(string increaseDecrease, Ability ability, Unit unit, float modifier, float duration, string buffName = "None", float maxStacks = 99)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (increaseDecrease == "Decrease") modifier *= -1;
        unit.AddTimedAbilityDamageModifier(ability, modifier / 100.0f, duration, buffName, (int)maxStacks);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Ability/Remove Ability Modifier",
        dynamicDescription = "Remove buffs for $ on $ named $",
        icon = unitIcon)]
    [AbilityArg(argType = ArgType.Temp, allowFunction = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Temp)]
    public void RemoveAbilityModifier(Ability ability, Unit unit, string buffName)
    {
        Error(ability == null, VisualCodeLabels.Errors.InvalidAbility);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        unit.RemoveAbilityBuffModifiers(ability, buffName);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Apply Buff Stacks to Unit(s)",
        dynamicDescription = "Apply $ stack(s) of $ to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void ApplyXStacksOfBuffToUnits(float stacks, Buff buff, UnitGroup units)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        if (stacks <= 0) return; // Do not allow a non-positive amount of stacks to be added.
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.AddBuff(buff, GetOwner(), (int)stacks);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Remove Buff Stacks from Unit(s)",
        dynamicDescription = "Remove $ stack(s) of $ from $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void RemoveXStacksOfBuffToUnits(float stacks, Buff buff, UnitGroup units)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        if (stacks <= 0) return; // Do not allow a non-positive amount of stacks to be removed.
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.RemoveStacks(buff, (int)stacks);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Set Buff Stacks on Unit(s)",
        dynamicDescription = "Set $ stacks on $ to $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetStacksOfBuffOnUnits(Buff buff, UnitGroup units, float stacks)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        if (stacks < 0) return; // Do not allow buff stacks to go negative.
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.SetStacks(buff, (int)stacks);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Set Max Buff Stacks on Unit(s)",
        dynamicDescription = "Set $ max stacks on $ to $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetMaxStacksOfBuffOnUnit(Buff buff, UnitGroup units, float stacks)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        if (stacks <= 0) return;
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.SetMaxStacks(buff, (int)stacks);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Set Buff Duration on Unit(s)",
        dynamicDescription = "Set current duration of $ on $ to $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetBuffDurationOnUnit(Buff buff, UnitGroup units, float duration)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.SetBuffDuration(buff, duration);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Modify Buff Duration on Unit(s)",
        dynamicDescription = "Modify current duration of $ on $ by $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ModifyBuffDurationOnUnit(Buff buff, UnitGroup units, float change)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units.unitList)
        {
            unit.buffs.ModifyBuffDuration(buff, change);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Enable Buff Timer on Unit(s)",
        dynamicDescription = "Enable the timer on $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    public void EnableBuffDuration(Buff buff)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        buff.buffHasDuration = true;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/Buff/Disable Buff Timer on Unit(s)",
        dynamicDescription = "Disable the timer on $",
        icon = unitIcon)]
    [BuffArg(argType = ArgType.Temp)]
    public void DisableBuffDuration(Buff buff)
    {
        Error(buff == null, VisualCodeLabels.Errors.InvalidBuff);
        buff.buffHasDuration = false;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - No Parameter",
        dynamicDescription = "Run C# Method on $ named $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    public void RunCSharpMethodOnUnitNoArg(UnitGroup units, string methodName)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Float Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void RunCSharpMethodOnUnitFloatArg(UnitGroup units, string methodName, float parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - String Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [StringArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitStringArg(UnitGroup units, string methodName, string parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Vector Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [VectorArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitVectorArg(UnitGroup units, string methodName, Vector3 parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Unit Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [UnitArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitUnitArg(UnitGroup units, string methodName, Unit parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Ability Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [AbilityArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitAbilityArg(UnitGroup units, string methodName, Ability parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Buff Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [BuffArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitBuffArg(UnitGroup units, string methodName, Buff parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Unit/C#/Run C# Method on Unit(s) - Item Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = unitIcon)]
    [UnitGroupArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [ItemArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitItemArg(UnitGroup units, string methodName, Item parameter)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            unit.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion

    #region Player Actions

    [VisualScriptingFunction(
        dropdownDescription = "Player/Add Gold",
        dynamicDescription = "Add $ gold to the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void AddGold(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.AddGold(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Remove Gold",
        dynamicDescription = "Remove $ gold to the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void RemoveGold(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.RemoveGold(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Add Experience",
        dynamicDescription = "Add $ experience to the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void AddExperience(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.AddExperience(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Remove Experience",
        dynamicDescription = "Remove $ experience to the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void RemoveExperience(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.RemoveExperience(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Set Experience",
        dynamicDescription = "Set the current experience of the player to $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Temp)]
    public void SetExperience(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.SetExperience(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Add Levels",
        dynamicDescription = "Add $ level(s) to the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddLevels(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.AddLevels((int)amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Remove Levels",
        dynamicDescription = "Remove $ level(s) from the player",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void RemoveLevels(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.RemoveLevels((int)amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Set Level",
        dynamicDescription = "Set the player to level $",
        icon = unitIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetLevel(float amount)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.SetLevel((int)amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Add Item",
        dynamicDescription = "Add $ to the player",
        icon = unitIcon)]
    [ItemArg(argType = ArgType.Temp)]
    public void AddItem(Item item)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        GameManager.player.inventory.AddItem(item.RollItem());
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Remove Item",
        dynamicDescription = "Remove $ to the player",
        icon = unitIcon)]
    [ItemArg(argType = ArgType.Temp)]
    public void RemoveItem(Item item)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        GameManager.player.inventory.RemoveItem(item);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Remove All Equipment",
        dynamicDescription = "Remove all equipment on the player",
        icon = unitIcon)]
    public void RemoveEquipment()
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        GameManager.player.equipment.Clear();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Player/Equip Item",
        dynamicDescription = "Have the player equip $",
        icon = unitIcon)]
    [ItemArg(argType = ArgType.Temp)]
    public void EquipItem(Item item)
    {
        Error(GameManager.player == null, VisualCodeLabels.Errors.InvalidPlayer);
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        item = item.RollItem();
        if (item.itemType == Item.ItemType.Weapon)
            GameManager.player.equipment.AddItemAtID(item, 0);

        else if (item.itemType == Item.ItemType.Amulet)
            GameManager.player.equipment.AddItemAtID(item, 1);

        else if (item.itemType == Item.ItemType.Armor)
            GameManager.player.equipment.AddItemAtID(item, 2);

        else if (item.itemType == Item.ItemType.Boots)
            GameManager.player.equipment.AddItemAtID(item, 3);

        else if (item.itemType == Item.ItemType.Ring && GameManager.player.equipment.GetItemAtID(4) == null)
            GameManager.player.equipment.AddItemAtID(item, 4);

        else if (item.itemType == Item.ItemType.Ring && GameManager.player.equipment.GetItemAtID(5) == null)
            GameManager.player.equipment.AddItemAtID(item, 5);

        else if (item.itemType == Item.ItemType.Ring)
            GameManager.player.equipment.AddItemAtID(item, 4);
    }

    #endregion

    #region Sound Actions

    [VisualScriptingFunction(
        dropdownDescription = "Audio/Play Sound",
        dynamicDescription = "Play $ at $ volume",
        icon = soundIcon)]
    [AudioClipArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100, suffix = "%")]
    public void Play2DSound(AudioClip clip, float volume)
    {
        Error(clip == null, VisualCodeLabels.Errors.InvalidAudioClip);
        volume /= 100.0f;
        GameManager.music.PlaySound(clip, volume);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Shake Screen",
        dynamicDescription = "Shake the screen with a strength of $",
        icon = effectIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ShakeScreen(float strength)
    {
        ScreenShakeEffect effect = Camera.main.GetOrAddComponent<ScreenShakeEffect>();
        Error(effect == null, VisualCodeLabels.Errors.CouldNotShakeScreen);
        effect.shakeStrength += (strength * 0.15f);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Audio/Play Music",
        dynamicDescription = "Change Game Music to $",
        icon = soundIcon)]
    [AudioClipArg(argType = ArgType.Temp)]
    public void ChangeGameMusic (AudioClip clip)
    {
        Error(clip == null, VisualCodeLabels.Errors.InvalidAudioClip);
        GameManager.music.FadeIntoNewClip(clip);
    }

    #endregion

    #region Feedback Actions

    // No longer in use. Will be removed in a future version.
    public void PlayFeedbackAtPoint(GameFeedback feedback, Vector3 point)
    {
        Error(feedback == null, "The specified feedback was invalid.");
        feedback.ActivateFeedback(null, null, point);
    }

    // No longer in use. Will be removed in a future version.
    public void PlayFeedbackOnUnit(GameFeedback feedback, Unit unit)
    {
        Error(feedback == null, "The specified feedback was invalid.");
        feedback.ActivateFeedback(unit.gameObject, unit.gameObject, unit.transform.position);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Create Circular Effect Guide",
        dynamicDescription = "Create a circular guide at $ with radius $ for $",
        icon = unitIcon)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "m")]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    public void CreateCircleGuide(Vector3 location, float radius, float duration)
    {
        CircleEffectGuide.Spawn(location, radius, duration);
    }

    public void CreateLineGuide(Vector3 location1, Vector3 location2, float width, float duration)
    {
        LineEffectGuide.Spawn(location1, location2, width, duration);
    }

    public void CreateLineGuide2(Unit unit, float width, float length, float duration)
    {
        LineEffectGuide.Spawn(unit, width, length, duration);
    }

    public void CreateArcGuide(float arc, Unit unit, float radius, float duration)
    {
        ArcEffectGuide.Spawn(arc, unit, radius, duration);
    }

    public void PlayFeedbackOnUnits(GameFeedback feedback, UnitGroup unitGroup)
    {
        Error(unitGroup == null, "The specified unit list was invalid.");
        foreach (Unit unit in unitGroup)
        {
            if (unit != null)
            {
                feedback.ActivateFeedback(unit.gameObject, unit.gameObject, unit.transform.position);
            }
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Play Effect at Location",
        dynamicDescription = "Play $ effect at $ for $ at $ scale",
        icon = effectIcon)]
    [EffectArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false)]
    [VectorArg(argType = ArgType.Temp, allowValue = true)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1.0f, suffix = "x")]
    public void SpawnEffectAtLocation(CustomVisualEffect effect, Vector3 position, float duration, float scale)
    {
        Error(effect == null, VisualCodeLabels.Errors.InvalidEffect);
        GameObject obj = ObjectPooler.InstantiatePooled(effect.gameObject, position, Quaternion.identity);
        obj.transform.localScale = effect.transform.localScale * scale;
        if (duration > 0) obj.GetComponent<CustomVisualEffect>().DestroyAfter(duration);
    }

    // USED BY OTHER METHODS.
    private void SpawnEffectOnUnit(CustomVisualEffect effect, Unit unit, float duration, float scale)
    {
        Error(effect == null, VisualCodeLabels.Errors.InvalidEffect);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        GameObject obj = ObjectPooler.InstantiatePooled(effect.gameObject, unit.transform.position, unit.transform.rotation);
        obj.name = effect.name;
        obj.transform.localScale = effect.transform.localScale * scale;
        if (duration > 0) obj.GetComponent<CustomVisualEffect>().DestroyAfter(duration);
        obj.transform.SetParent(unit.transform);
    }

    /*
    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Play Effect on Unit",
        dynamicDescription = "$ $ effect on $ for $ at $ scale",
        icon = effectIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.PlayOptions, allowFunction = false, allowPreset = false)]
    [EffectArg(argType = ArgType.Temp)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1.0f, suffix = "x")]
    */
    public void SpawnEffectOnUnit(string action, CustomVisualEffect effect, Unit unit, float duration, float scale)
    {
        Error(effect == null, VisualCodeLabels.Errors.InvalidEffect);
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (action == PresetStrings.Play)
        {
            SpawnEffectOnUnit(effect, unit, duration, scale);
        }
        else if (action == PresetStrings.Stop)
        {
            Transform existing = unit.transform.Find(effect.name);
            if (existing != null)
            {
                ObjectPooler.DestroyPooled(unit.transform.Find(effect.name).gameObject);
            }
        }
        else if (action == PresetStrings.PlayOrRefresh)
        {
            Transform existing = unit.transform.Find(effect.name);
            if (existing != null)
            {
                existing.GetComponent<CustomVisualEffect>().DestroyAfter(duration);
            }
            else
            {
                SpawnEffectOnUnit(effect, unit, duration, scale);
            }
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Play Effect on Unit(s)",
        dynamicDescription = "$ $ effect on $ for $ at $ scale",
        icon = effectIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.PlayOptions, allowFunction = false, allowPreset = false)]
    [EffectArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1.0f, suffix = "x")]
    public void SpawnEffectOnUnitGroup(string action, CustomVisualEffect effect, UnitGroup unitGroup, float duration, float scale)
    {
        Error(effect == null, VisualCodeLabels.Errors.InvalidEffect);
        Error(unitGroup == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        foreach (Unit unit in unitGroup)
        {
            if (unit == null) continue;
            SpawnEffectOnUnit(action, effect, unit, duration, scale);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Feedback/Flash Color on Unit(s)",
        dynamicDescription = "Flash $ on $ for $",
        icon = effectIcon)]
    [ColorArg(argType = ArgType.Temp)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "s")]
    public void FlashColorOnUnits(Color color, UnitGroup units, float time)
    {
        Error(units == null, VisualCodeLabels.Errors.InvalidUnitGroup);

        foreach (Unit unit in units)
        {
            if (unit == null) continue;
            if (unit.GetComponent<FlashTextureEffect>() != null) return;
            unit.AddComponent<FlashTextureEffect>().Setup(color, time);
        }
    }

    #endregion

    #region Projectile Actions

    public static Projectile lastCreatedProjectile = null;

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Spawn Projectile",
        dynamicDescription = "Spawn $ at $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Temp, allowPreset = false, allowFunction = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SpawnProjectile(Projectile projectile, Vector3 position)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        if (projectile == null) return;

        // Default orientation of the projectile matches the casting unit.
        Quaternion rotation = Quaternion.identity;
        Unit unit = LogicEngine.current.GetOwner();
        if (unit != null) rotation = unit.transform.rotation;

        GameObject obj = ObjectPooler.InstantiatePooled(projectile.gameObject, position, rotation);
        Projectile p = obj.GetComponent<Projectile>();
        p.Setup(LogicEngine.current.engineHandler);
        lastCreatedProjectile = p;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Move Projectile Forwards",
        dynamicDescription = "Move $ forwards at $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 3, suffix = "m/s")]
    public void MoveForwardAtSpeed(Projectile projectile, float speed)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.MoveProjectileForwards(speed);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Rotate Projectile",
        dynamicDescription = "Rotate $ by $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 30, suffix = "\u00BA")]
    public void RotateProjectile(Projectile projectile, float amount)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.Rotate(amount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Face Projectile Towards Point",
        dynamicDescription = "Face $ towards $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void FaceProjectileTowardsPoint(Projectile projectile, Vector3 point)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.FaceProjectileTowardsPoint(point);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Set Lifetime",
        dynamicDescription = "Set max lifetime of $ to $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "s")]
    public void SetProjectileLifetime(Projectile projectile, float lifetime)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SetLifetime(lifetime);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Destroy Projectile",
        dynamicDescription = "Destroy $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Temp, allowValue = false)]
    public void DestroyProjectile(Projectile projectile)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        if (projectile != null)
            projectile.DestroyProjectile();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Move Projectile Towards Point",
        dynamicDescription = "Move $ towards $ at $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 3, suffix = "m/s")]
    public void MoveTowardsPointAtSpeed(Projectile projectile, Vector3 point, float speed)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.MoveProjectileTowardsPoint(point, speed);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/Move Projectile Towards Point in Arc",
        dynamicDescription = "Move $ towards $ in $ with $ arc",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 3, suffix = "s")]
    [NumberArg(argType = ArgType.Value, defaultValue = 2, suffix = "m")]
    public void MoveTowardsPointInArc(Projectile projectile, Vector3 point, float time, float arcHeight)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.MoveProjectileInArcTowardsPoint(point, time, arcHeight);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - No Parameter",
        dynamicDescription = "Run C# Method on $ named $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    public void RunCSharpMethodOnProjectileNoArg(Projectile projectile, string methodName)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Float Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void RunCSharpMethodOnProjectileFloatArg(Projectile projectile, string methodName, float parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - String Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [StringArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnProjectileStringArg(Projectile projectile, string methodName, string parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Vector Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [VectorArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnProjectileVectorArg(Projectile projectile, string methodName, Vector3 parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Unit Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [UnitArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnProjectileUnitArg(Projectile projectile, string methodName, Unit parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Ability Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [AbilityArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnProjectileAbilityArg(Projectile projectile, string methodName, Ability parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Buff Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [BuffArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnProjectileBuffArg(Projectile projectile, string methodName, Buff parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Projectile/C#/Run C# Method on Projectile - Item Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = projectileIcon)]
    [ProjectileArg(argType = ArgType.Preset, preset = VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, allowValue = false)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [ItemArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnUnitProjectileArg(Projectile projectile, string methodName, Item parameter)
    {
        Error(projectile == null, VisualCodeLabels.Errors.InvalidProjectile);
        projectile.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    #endregion

    #region Region Actions

    [VisualScriptingFunction(
        dropdownDescription = "Region/Create Square Region",
        dynamicDescription = "Create a square region at $ with side length $ named $",
        icon = regionIcon)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "m")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    public void CreateSquareRegion(Vector3 position, float size, string regionName)
    {
        Error(regionName == null || regionName.Length == 0, VisualCodeLabels.Errors.InvalidRegionName);
        Error(GameManager.assets.squareRegion == null, "The square region prefab has not been assigned (GameManager > Assets > Square Region)");
        Region region = GameObject.Instantiate(GameManager.assets.squareRegion, position, Quaternion.identity);
        region.regionName = regionName;
        region.transform.localScale = new Vector3(size, size, size);
    } 

    [VisualScriptingFunction(
        dropdownDescription = "Region/Create Spherical Region",
        dynamicDescription = "Create a spherical region at $ with radius $ named $",
        icon = regionIcon)]
    [VectorArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1, suffix = "m")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    public void CreateCircularRegion(Vector3 position, float size, string regionName)
    {
        Error(regionName == null || regionName.Length == 0, VisualCodeLabels.Errors.InvalidRegionName);
        Error(GameManager.assets.squareRegion == null, "The spherical region prefab has not been assigned (GameManager > Assets > Spherical Region)");
        Region region = GameObject.Instantiate(GameManager.assets.sphericalRegion, position, Quaternion.identity);
        region.regionName = regionName;
        region.transform.localScale = new Vector3(size, size, size);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Region/Destroy Regions with Name",
        dynamicDescription = "Destroy regions named $",
        icon = regionIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    public void DestroyRegions(string regionName)
    {
        Error(regionName.Length == 0, VisualCodeLabels.Errors.InvalidRegionName);
        Region.DestroyAllRegionsWithName(regionName);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Region/Destroy Regions with ID",
        dynamicDescription = "Destroy region with ID $",
        icon = regionIcon)]
    [NumberArg(argType = ArgType.Temp, tempLabel = "Region ID")]
    public void DestroyRegionWithID(float id)
    {
        Region.DestroyRegionWithID((int)id);
    }

    #endregion

    #region Quest Actions

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Create Quest",
        dynamicDescription = "Give the player a quest named $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    public void CreateQuest2(string questName)
    {
        Error(questName.Length == 0, VisualCodeLabels.Errors.InvalidQuestName);
        if (GameManager.quests.QuestIsActive(questName))
        {
            Error(true, VisualCodeLabels.Errors.InvalidQuestAlreadyInProgress);
        }
        else
        {
            Quest quest = new Quest(questName, questName);
            GameManager.quests.AddQuest(quest);
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Add Quest Requirement",
        dynamicDescription = "Add $ requirement to $ with $ progress increments",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Requirement")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void AddQuestRequirement2(string requirement, string questName, float increments)
    {
        Error(questName.Length == 0, VisualCodeLabels.Errors.InvalidQuestName);
        Error(requirement.Length == 0, VisualCodeLabels.Errors.InvalidQuestRequirementName);
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        quest.AddCompletionRequirement(requirement, requirement, (int)increments);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Add Quest Reward",
        dynamicDescription = "Add reward labeled $ to quest $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Reward")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    public void AddQuestReward2(string reward, string questName)
    {
        Error(reward.Length == 0, VisualCodeLabels.Errors.InvalidQuestReward);
        Error(questName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        GameManager.quests.GetQuest(questName).SetReward(reward);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Set Quest Progress",
        dynamicDescription = "Set quest progress of $ to $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetQuestRequirementProgress2(string questName, float progress)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        Error(progress < 0, "Invalid quest progress. Quest progress cannot be negative.");
        quest.SetProgress(string.Empty, (int)progress);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Modify Quest Progress",
        dynamicDescription = "Modify quest progress of $ by $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ModifyQuestRequirementProgress2(string questName, float progress)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        quest.IncrementProgress(string.Empty, (int)progress);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Set Requirement Progress",
        dynamicDescription = "Set quest progress of $ on $ to $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Requirement Name")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetSpecificQuestRequirementProgress2(string requirement, string questName, float progress)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        quest.SetProgress(requirement, (int)progress);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Modify Requirement Progress",
        dynamicDescription = "Modify quest progress of $ on $ by $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Requirement Name")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ModifySpecificQuestRequirementProgress2(string requirement, string questName, float progress)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        quest.IncrementProgress(requirement, (int)progress);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Quests/Complete Quest",
        dynamicDescription = "Complete quest named $",
        icon = questIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    public void CompleteQuest(string questName)
    {
        Quest quest = GameManager.quests.GetQuest(questName);
        Error(quest == null, VisualCodeLabels.Errors.InvalidQuest);
        quest.Complete();
    }

    #endregion

    #region Pickup Actions

    [VisualScriptingFunction(
        dropdownDescription = "Pickups/Spawn Item Pickup",
        dynamicDescription = "Spawn an item pickup at $ containing $",
        icon = questIcon)]
    [VectorArg(argType = ArgType.Temp, tempLabel = "Location")]
    [ItemArg(argType = ArgType.Temp)]
    public void SpawnItemDrop(Vector3 location, Item item)
    {
        Error(item == null, VisualCodeLabels.Errors.InvalidItem);
        ItemPickup.Spawn(location, item);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Pickups/Spawn Gold Pickup",
        dynamicDescription = "Spawn a gold pickup at $ containing $ gold",
        icon = questIcon)]
    [VectorArg(argType = ArgType.Temp, tempLabel = "Location")]
    [NumberArg(argType = ArgType.Temp)]
    public void SpawnGoldDrop(Vector3 location, float goldAmount)
    {
        GoldPickup.Spawn(location, (int)goldAmount);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Pickups/Spawn Health Pickup",
        dynamicDescription = "Spawn a health pickup at $",
        icon = questIcon)]
    [VectorArg(argType = ArgType.Temp, tempLabel = "Location")]
    public void SpawnHealthDrop(Vector3 location)
    {
        HealthPickup.Spawn(location);
    }

    #endregion

    #region Game State Actions

    [VisualScriptingFunction(
        dropdownDescription = "Game State/Win Game",
        dynamicDescription = "Have the player win the game",
        icon = gameIcon)]
    public void WinGame()
    {
        GameManager.instance.WinGame();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game State/Restart Level",
        dynamicDescription = "Restart the current level",
        icon = gameIcon)]
    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game State/Set Checkpoint",
        dynamicDescription = "Set the player checkpoint to $",
        icon = gameIcon)]
    [VectorArg(argType = ArgType.Temp)]
    public void SetPlayerCheckpoint(Vector3 position)
    {
        GameManager.SetCheckpoint(position);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Disable Item Drops",
        dynamicDescription = "Disable Item Drops",
        icon = gameIcon)]
    public void DisableItemDrops ()
    {
        GameManager.gameSettings.ItemsCanDrop = false;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Enable Item Drops",
        dynamicDescription = "Enable Item Drops",
        icon = gameIcon)]
    public void EnableItemDrops ()
    {
        GameManager.gameSettings.ItemsCanDrop = true;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Disable Health Pickups",
        dynamicDescription = "Disable Health Pickups",
        icon = gameIcon)]
    public void DisableHealthPickups ()
    {
        GameManager.gameSettings.HealthPickupsCanDrop = false;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Enable Health Pickups",
        dynamicDescription = "Enable Health Pickups",
        icon = gameIcon)]
    public void EnableHealthPickups()
    {
        GameManager.gameSettings.HealthPickupsCanDrop = true;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Disable Gold Pickups",
        dynamicDescription = "Disable Gold Pickups",
        icon = gameIcon)]
    public void DisableGoldPickups()
    {
        GameManager.gameSettings.GoldPickupsCanDrop = false;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Enable Gold Pickups",
        dynamicDescription = "Enable Gold Pickups",
        icon = gameIcon)]
    public void EnableGoldPickups()
    {
        GameManager.gameSettings.GoldPickupsCanDrop = true;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Disable Shop Access",
        dynamicDescription = "Disable Shop Access",
        icon = gameIcon)]
    public void DisableShop()
    {
        GameManager.gameSettings.CanAccessShop = false;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Enable Shop Access",
        dynamicDescription = "Enable Shop Access",
        icon = gameIcon)]
    public void EnableShop()
    {
        GameManager.gameSettings.CanAccessShop = true;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Gold Drop Chance",
        dynamicDescription = "Set Gold Drop Chance to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    public void SetGoldDropChance(float amount)
    {
        amount /= 100.0f;
        GameManager.monsterValues.goldDropChance = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Base Gold Drop Amount",
        dynamicDescription = "Set the base gold from amount to between $ and $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 20)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50)]
    public void SetBaseGoldDropAmount(float minimum, float maximum)
    {
        Error(minimum > maximum, "Minimum gold drop amount must be less than or equal to the maximum gold drop amount.");
        GameManager.monsterValues.baseGoldDropAmountMinimum = minimum;
        GameManager.monsterValues.baseGoldDropAmountMinimum = maximum;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Health Pickup Drop Chance",
        dynamicDescription = "Set health pickup drop chance to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 50, suffix = "%")]
    public void SetHealthPickupDropChance(float amount)
    {
        amount /= 100.0f;
        GameManager.healthGlobeValues.baseHealthGlobeChance = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Health Pickup Heal Amount",
        dynamicDescription = "Set health pickup heal amount to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void SetHealthPickupHealAmount(float amount)
    {
        GameManager.healthGlobeValues.healthGlobeHealthRestore = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Health Pickup Lifetime",
        dynamicDescription = "Set health pickup lifetime to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 5)]
    public void SetHealthPickupLifetime(float amount)
    {
        GameManager.healthGlobeValues.lifetime = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Item Return Rate",
        dynamicDescription = "Set the item sell percentage to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 25, suffix = "%")]
    public void SetItemSellPercentage(float amount)
    {
        amount /= 100.0f;
        GameManager.inventoryValues.sellItemReturnRate = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Base Monster XP",
        dynamicDescription = "Set the base experience for a monster kill to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 10)]
    public void SetBaseMonsterXP(float amount)
    {
        GameManager.playerExperienceValues.baseMonsterXP = amount;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Game Settings/Set Global Spawn Density",
        dynamicDescription = "Set the global spawn density to $",
        icon = gameIcon)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void SetGlobalSpawnDensity(float amount)
    {
        GameManager.spawner.spawnDensity = amount;
    }

    #endregion

    #region UI Actions

    [VisualScriptingFunction(
        dropdownDescription = "UI Actions/Show In-World Status Message",
        dynamicDescription = "Show a status message at $ with color $ printing $",
        icon = uiIcon)]
    [VectorArg(argType = ArgType.Temp)]
    [ColorArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Message")]
    public void ShowStatusMessage(Vector3 position, Color color, string message)
    {
        Error(message.Length == 0, VisualCodeLabels.Errors.InvalidText);
        StatusMessageUI.Spawn(position, message, color);
    }

    [VisualScriptingFunction(
        dropdownDescription = "UI Actions/Show Tutorial Message",
        dynamicDescription = "Show a tutorial message printing $ for $ seconds",
        icon = uiIcon)]
    [StringArg(argType = ArgType.Temp)]
    [NumberArg(argType = ArgType.Value, defaultValue = 6)]
    public void ShowTutorialMessage(string message, float duration = 6.0f)
    {
        Error(message.Length == 0, VisualCodeLabels.Errors.InvalidText);
        GameManager.ui.MessageWindow.DisplayMessage(message, duration);
    }

    [VisualScriptingFunction(
        dropdownDescription = "UI Actions/Open Shop",
        dynamicDescription = "Open the Shop",
        icon = uiIcon)]
    public void OpenShop()
    {
        GameManager.ui.CharacterWindow.Show();
        GameManager.ui.ShopWindow.Show();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Debug/Show Debug Message",
        dynamicDescription = "Show a debug message printing $",
        icon = uiIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Message")]
    public void ShowDebugMessage(string message)
    {
        Debug.Log(message);
    }

    #endregion

    #region Interactables Actions

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Create",
        dynamicDescription = "Destroy $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = true, allowPreset = false, allowFunction = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void Create(CustomInteractable interactable, Vector3 position)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        GameObject.Instantiate(interactable.gameObject, position, Quaternion.identity);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Destroy",
        dynamicDescription = "Destroy $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void Destroy(CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        GameObject.Destroy(interactable.gameObject);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Interact",
        dynamicDescription = "Start the interaction on $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void Interact(CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.ForceInteract();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Force Player Interaction",
        dynamicDescription = "Force the player to interact with $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void ForceInteraction(CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.ForceTriggerInteract();
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Enable or Disable Interactions",
        dynamicDescription = "$ interactions on $",
        icon = pickupIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.EnableDisable, allowPreset = false, allowFunction = false)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void EnableOrDisableInteractions(string mode, CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.allowInteractions = (mode == PresetStrings.Enable);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Enable or Disable Object",
        dynamicDescription = "$ $",
        icon = pickupIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.EnableDisable, allowPreset = false, allowFunction = false)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void EnableOrDisable (string mode, CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.gameObject.SetActive(mode == PresetStrings.Enable);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Enable or Disable Object Child",
        dynamicDescription = "$ child of $ named $",
        icon = pickupIcon)]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.EnableDisable, allowPreset = false, allowFunction = false)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [StringArg(argType = ArgType.Temp)]
    public void EnableOrDisableChild(string mode, CustomInteractable interactable, string child)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Transform transform = interactable.transform.Find(child);
        Error(transform == null, VisualCodeLabels.Errors.InvalidChild);
        transform.gameObject.SetActive(mode == PresetStrings.Enable);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Set Position",
        dynamicDescription = "Set the position of $ to $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SetPosition(CustomInteractable interactable, Vector3 position)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.transform.position = position;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Set Rotation",
        dynamicDescription = "Set the rotation of $ to $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 0)]
    public void SetRotation(CustomInteractable interactable, float rotation)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.transform.rotation = Quaternion.Euler(0, rotation, 0);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Set Scale",
        dynamicDescription = "Set the scale of $ to $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SetScale (CustomInteractable interactable, Vector3 scale)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.transform.localScale = scale;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Modify Scale",
        dynamicDescription = "Set the scale of $ to $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 2)]
    public void ModifyScale(CustomInteractable interactable, float modifier)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        interactable.transform.localScale *= modifier;
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Look At",
        dynamicDescription = "Have $ look at $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void LookAt (CustomInteractable interactable, Vector3 position)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        position.y = interactable.transform.position.y;
        interactable.transform.LookAt(position);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Play Animation",
        dynamicDescription = "Play $ animation of $ or one of its children",
        icon = pickupIcon)]
    [StringArg(argType = ArgType.Temp)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    public void PlayAnimation(string animation, CustomInteractable interactable)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        int hash = Animator.StringToHash(animation);
        Animator[] animators = interactable.GetComponents<Animator>();
        foreach (Animator animator in animators)
        {
            if (animator.HasState(0, hash))
            {
                animator.Play(hash);
            }
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/Set Color",
        dynamicDescription = "Set color of $ to $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp, allowValue = false)]
    [ColorArg(argType = ArgType.Temp)]
    public void SetColor(CustomInteractable interactable, Color color)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Renderer[] renderers = interactable.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = color;
        }
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - No Parameter",
        dynamicDescription = "Run C# Method on $ named $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    public void RunCSharpMethodOnInteractableNoArg(CustomInteractable interactable, string methodName)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Float Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [NumberArg(argType = ArgType.Value, defaultValue = 100)]
    public void RunCSharpMethodOnInteractableFloatArg(CustomInteractable interactable, string methodName, float parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - String Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [StringArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableStringArg(CustomInteractable interactable, string methodName, string parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Vector Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [VectorArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableVectorArg(CustomInteractable interactable, string methodName, Vector3 parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Unit Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [UnitArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableUnitArg(CustomInteractable interactable, string methodName, Unit parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Ability Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [AbilityArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableAbilityArg(CustomInteractable interactable, string methodName, Ability parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Buff Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [BuffArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableBuffArg(CustomInteractable interactable, string methodName, Buff parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Interactables/C#/Run C# Method on Interactable - Item Parameter",
        dynamicDescription = "Run C# Method on $ named $ with value $",
        icon = pickupIcon)]
    [InteractableArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Method Name")]
    [ItemArg(argType = ArgType.Temp)]
    public void RunCSharpMethodOnInteractableProjectileArg(CustomInteractable interactable, string methodName, Item parameter)
    {
        Error(interactable == null, VisualCodeLabels.Errors.InvalidInteractable);
        Error(methodName.Length == 0, VisualCodeLabels.Errors.InvalidText);
        interactable.SendMessage(methodName, parameter, SendMessageOptions.DontRequireReceiver);
    }

    #endregion

    #region Variable Actions

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Modify Number Variable",
        dynamicDescription = "Modify script number variable named $ by $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "NumberVar", allowFunction = false, allowPreset = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 1)]
    public void ModifyNumberVariable(string name, float value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidText);
        float currentValue = LogicEngine.current.GetLocalVariable<float>(name);
        LogicEngine.current.SetLocalVariable(name, currentValue + value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set Number Variable",
        dynamicDescription = "Set script number variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "NumberVar", allowFunction = false, allowPreset = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 0)]
    public void SetNumberVariable(string name, float value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.current.SetLocalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set Unit Group Variable",
        dynamicDescription = "Set Unit Group named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitGroupVar", allowFunction = false, allowPreset = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void SetUnitGroupVariable(string name, UnitGroup value)
    {
        Error(value == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        LogicEngine.current.SetLocalVariable(name, value.Copy());
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Add Unit to Unit Group Variable",
        dynamicDescription = "Add $ to Unit Group Named $",
        icon = variableIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitGroupVar", allowFunction = false, allowPreset = false)]
    public void AddToUnitGroup(Unit unit, string name)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        if (!LogicEngine.current.localVariables.ContainsKey(name))
            LogicEngine.current.localVariables.Add(name, new UnitGroup());
        ((UnitGroup)LogicEngine.current.localVariables[name.ToUpper()]).AddUnit(unit);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Remove Unit from Unit Group Variable",
        dynamicDescription = "Remove $ from Unit Group Named $",
        icon = variableIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitGroupVar", allowFunction = false, allowPreset = false)]
    public void RemoveFromUnitGroup(Unit unit, string name)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        if (!LogicEngine.current.localVariables.ContainsKey(name)) return;
        ((UnitGroup)LogicEngine.current.localVariables[name.ToUpper()]).RemoveUnit(unit);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Add Unit to Global Unit Group Variable",
        dynamicDescription = "Add $ to Global Unit Group Named $",
        icon = variableIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    public void AddToGlobalUnitGroup(Unit unit, string name)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        if (!LogicEngine.globalVariables.ContainsKey(name))
            LogicEngine.globalVariables.Add(name, new UnitGroup());
        ((UnitGroup)LogicEngine.globalVariables[name.ToUpper()]).AddUnit(unit);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Remove Unit from Global Unit Group Variable",
        dynamicDescription = "Remove $ from Global Unit Group Named $",
        icon = variableIcon)]
    [UnitArg(argType = ArgType.Temp)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    public void RemoveFromGlobalUnitGroup(Unit unit, string name)
    {
        Error(unit == null, VisualCodeLabels.Errors.InvalidUnit);
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        if (!LogicEngine.globalVariables.ContainsKey(name)) return;
        ((UnitGroup)LogicEngine.globalVariables[name.ToUpper()]).RemoveUnit(unit);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set Bool Variable",
        dynamicDescription = "Set script bool variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "BoolVar", allowFunction = false, allowPreset = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = true)]
    public void SetBoolVariable(string name, bool value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.current.SetLocalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set Unit Variable",
        dynamicDescription = "Set script unit variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "UnitVar", allowFunction = false, allowPreset = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void SetUnitVariable(string name, Unit value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.current.SetLocalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set Vector Variable",
        dynamicDescription = "Set script vector variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "VectorVar", allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SetVectorVariable(string name, Vector3 value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.current.SetLocalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Variables/Set String Variable",
        dynamicDescription = "Set script string variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Value, defaultValue = "StringVar", allowFunction = false, allowPreset = false)]
    [StringArg(argType = ArgType.Temp)]
    public void SetStringVariable(string name, string value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.current.SetLocalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Modify Global Number Variable",
        dynamicDescription = "Modify global number variable named $ by $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 0)]
    public void ModifyGlobalNumberVariable(string name, float value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        float currentValue = LogicEngine.GetGlobalVariable<float>(name);
        LogicEngine.SetGlobalVariable(name, currentValue + value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global Number Variable",
        dynamicDescription = "Set global number variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [NumberArg(argType = ArgType.Value, defaultValue = 0)]
    public void SetGlobalNumberVariable(string name, float value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.SetGlobalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global Unit Group Variable",
        dynamicDescription = "Set Global Unit Group named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [UnitGroupArg(argType = ArgType.Temp, allowValue = false)]
    public void SetGlobalUnitGroupVariable(string name, UnitGroup value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        Error(value == null, VisualCodeLabels.Errors.InvalidUnitGroup);
        LogicEngine.SetGlobalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global Bool Variable",
        dynamicDescription = "Set global bool variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [BoolArg(argType = ArgType.Value, defaultValue = true)]
    public void SetGlobalBoolVariable(string name, bool value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.SetGlobalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global Unit Variable",
        dynamicDescription = "Set global unit variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [UnitArg(argType = ArgType.Temp, allowValue = false)]
    public void SetGlobalUnitVariable(string name, Unit value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.SetGlobalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global Vector Variable",
        dynamicDescription = "Set global vector variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [VectorArg(argType = ArgType.Temp)]
    public void SetGlobalVectorVariable(string name, Vector3 value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.SetGlobalVariable(name, value);
    }

    [VisualScriptingFunction(
        dropdownDescription = "Global Variables/Set Global String Variable",
        dynamicDescription = "Set global string variable named $ to $",
        icon = variableIcon)]
    [StringArg(argType = ArgType.Temp, tempLabel = "Variable Name", allowFunction = false, allowPreset = false)]
    [StringArg(argType = ArgType.Temp)]
    public void SetGlobalStringVariable(string name, string value)
    {
        Error(name.Length == 0, VisualCodeLabels.Errors.InvalidVariable);
        LogicEngine.SetGlobalVariable(name, value);
    }

    #endregion
}