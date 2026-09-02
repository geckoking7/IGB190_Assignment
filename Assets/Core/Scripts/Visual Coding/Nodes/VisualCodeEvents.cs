public partial class VisualCodeScript
{
    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Ability/When a unit begins casting this ability",
        dynamicDescription = "When a unit begins casting this ability")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Vectors.PRESET_TARGET_POSITION)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TARGET_UNIT)]
    public void WhenUnitBeginsCastingThisAbility() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Ability/When a unit finishes casting this ability",
        dynamicDescription = "When a unit finishes casting this ability")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Vectors.PRESET_TARGET_POSITION)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TARGET_UNIT)]
    public void WhenUnitFinishesCastingThisAbility() { }

    [VisualScriptingEvent(icon = timerIcon,
        dropdownDescription = "Time/On script loaded",
        dynamicDescription = "On script loaded")]
    public void ScriptLoaded() { }

    [VisualScriptingEvent(icon = timerIcon,
        dropdownDescription = "Time/On script unloaded",
        dynamicDescription = "On script unloaded")]
    public void ScriptUnloaded() { }

    [VisualScriptingEvent(icon = timerIcon,
        dropdownDescription = "Time/Do every frame",
        dynamicDescription = "Do actions every frame")]
    public void EveryFrame() { }

    [VisualScriptingEvent(icon = timerIcon,
        dropdownDescription = "Time/Do after X seconds",
        dynamicDescription = "After $ seconds")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5)]
    public void OnOneOffTimerFinished(float delay) { }

    [VisualScriptingEvent(icon = timerIcon,
        dropdownDescription = "Time/Do every X Seconds",
        dynamicDescription = "Every $ seconds")]
    [NumberArg(argType = ArgType.Value, defaultValue = 5)]
    public void OnTimerFinished(float period) { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Unit/Unit is killed",
        dynamicDescription = "When a unit is killed")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_KILLED_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_KILLING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Abilities.PRESET_KILLING_ABILITY)]
    [EventPreset(VisualCodeLabels.Presets.Events.Bools.PRESET_IS_CRITICAL)]
    public void WhenUnitIsKilled() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Unit/Unit is damaged",
        dynamicDescription = "When a unit is damaged")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_DAMAGED_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_DAMAGING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Abilities.PRESET_DAMAGING_ABILITY)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_DAMAGE_DEALT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Bools.PRESET_IS_CRITICAL)]
    public void OnUnitDamaged() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Unit/Unit is healed",
        dynamicDescription = "When a unit is healed")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_HEALED_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_HEALING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Abilities.PRESET_HEALING_ABILITY)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_AMOUNT_HEALED)]
    public void WhenUnitIsHealed() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Unit/Unit gains resource",
        dynamicDescription = "When a unit gains resource")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_RESOURCES_GAINED)]
    public void WhenUnitGainsResource() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Unit/Unit loses resource",
        dynamicDescription = "When a unit loses resource")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_RESOURCES_LOST)]
    public void WhenUnitLosesResource() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Ability/When a unit starts casting specific ability",
        dynamicDescription = "When Unit Starts Casting $")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Abilities.PRESET_ABILITY_CAST)]
    public void UnitStartCast() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Ability/When a unit finishes casting specific ability",
        dynamicDescription = "When Unit Finishes Casting $")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Abilities.PRESET_ABILITY_CAST)]
    public void UnitFinishCast() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Buff/Buff is applied to unit",
        dynamicDescription = "Buff is applied to unit")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_APPLIER)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET)]
    [EventPreset(VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT)]
    public void BuffApplied() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Buff/Buff is removed from unit",
        dynamicDescription = "Buff is removed from unit")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_REMOVER)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET)]
    [EventPreset(VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT)]
    public void BuffRemoved() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Buff/Buff stack count changes",
        dynamicDescription = "Buff stack count changes")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_APPLIER)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET)]
    [EventPreset(VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT_CHANGE)]
    public void BuffStacksChanged() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Player/Player gains experience",
        dynamicDescription = "When the player gains experience")]
    public void PlayerGainsExperience() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Player/Player gains a level",
        dynamicDescription = "When the player gains a level")]
    public void OnPlayerLevelUp() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Player/Player sells an item",
        dynamicDescription = "When the player sells an item")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    public void OnItemSold() { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "Player/Player buys an item",
        dynamicDescription = "When the player buys an item")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    public void OnItemBought() { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player equips an item",
        dynamicDescription = "When the player equips an item")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    public void OnItemEquipped() { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player unequips an item",
        dynamicDescription = "When the player unequips an item")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    public void OnItemUnequipped() { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player equips an item from set",
        dynamicDescription = "Player equips an item from the set $")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    [ItemSetArg(argType = ArgType.Temp)]
    public void OnItemEquippedFromSet(ItemSet itemSet) { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player unequips an item from set",
        dynamicDescription = "Player unequips an item from the set $")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    [ItemSetArg(argType = ArgType.Temp)]
    public void OnItemUnequippedFromSet(ItemSet itemSet) { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player picks up an item",
        dynamicDescription = "When the player picks up an item")]
    [EventPreset(VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM)]
    public void OnItemPickedUp() { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player picks up gold",
        dynamicDescription = "When the player picks up gold")]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_GOLD_PICKED_UP)]
    public void OnPickupGold() { }

    [VisualScriptingEvent(icon = pickupIcon,
        dropdownDescription = "Player/Player picks up a health globe",
        dynamicDescription = "When the player picks up a health globe")]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_HEALTH_PICKED_UP)]
    public void OnPickupHealth() { }

    [VisualScriptingEvent(icon = regionIcon,
        dropdownDescription = "Region/Unit enters region",
        dynamicDescription = "Unit enters region named $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_REGION_ID)]
    public void UnitEntersRegion(string regionName) { }

    [VisualScriptingEvent(icon = regionIcon,
        dropdownDescription = "Region/Unit leaves region",
        dynamicDescription = "Unit leaves region named $")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Region Name")]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_REGION_ID)]
    public void UnitExitsRegion(string regionName) { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Projectile/Projectile collides with an enemy",
        dynamicDescription = "Projectile from this object collides with an enemy")]
    [EventPreset(VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_COLLIDING_UNIT)]
    public void ProjectileMadeByThisCollidesWithUnit() { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Projectile/Projectile times out",
        dynamicDescription = "Projectile from this object times out")]
    [EventPreset(VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    public void ProjectileTimesOut() { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Projectile/Projectile reaches its goal",
        dynamicDescription = "Projectile from this object reaches its goal")]
    [EventPreset(VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    [EventPreset(VisualCodeLabels.Presets.Events.Vectors.PRESET_GOAL_POSITION)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_GOAL_UNIT)]
    public void ProjectileReachesGoal() { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Projectile/Projectile collides with terrain",
        dynamicDescription = "Projectile from this object collides with the terrain")]
    [EventPreset(VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT)]
    public void ProjectileCollidesWithTerrain() { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Quests/On quest completed",
        dynamicDescription = "When the quest named $ is completed")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    public void OnQuestCompleted(string questName) { }

    [VisualScriptingEvent(icon = projectileIcon,
        dropdownDescription = "Quests/On quest received",
        dynamicDescription = "When a quest named $ is received")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Quest Name")]
    public void OnQuestReceived(string questName) { }

    [VisualScriptingEvent(icon = inputIcon,
        dropdownDescription = "Interactions/On Interaction Started",
        dynamicDescription = "When an interaction is started")]
    [EventPreset(VisualCodeLabels.Presets.Events.Interactables.PRESET_INTERACTABLE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_INTERACTABLE_UNIQUE_ID)]
    [EventPreset(VisualCodeLabels.Presets.Events.Strings.PRESET_INTERACTABLE_LABEL)]
    [EventPreset(VisualCodeLabels.Presets.Events.Vectors.PRESET_INTERACTABLE_POSITION)]
    public void OnInteractionStarted() { }

    [VisualScriptingEvent(icon = inputIcon,
        dropdownDescription = "Interactions/On Interaction Finished",
        dynamicDescription = "When an interaction is finished")]
    [EventPreset(VisualCodeLabels.Presets.Events.Interactables.PRESET_INTERACTABLE)]
    [EventPreset(VisualCodeLabels.Presets.Events.Numbers.PRESET_INTERACTABLE_UNIQUE_ID)]
    [EventPreset(VisualCodeLabels.Presets.Events.Strings.PRESET_INTERACTABLE_LABEL)]
    [EventPreset(VisualCodeLabels.Presets.Events.Vectors.PRESET_INTERACTABLE_POSITION)]
    public void OnInteractionFinished() { }

    [VisualScriptingEvent(icon = inputIcon,
        dropdownDescription = "Input/On Key Down",
        dynamicDescription = "When the $ key is pressed down")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    public void OnKeyDown(string key) { }

    [VisualScriptingEvent(icon = inputIcon,
        dropdownDescription = "Input/On Key Up",
        dynamicDescription = "When the $ key is released")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    public void OnKeyUp(string key) { }

    [VisualScriptingEvent(icon = inputIcon,
        dropdownDescription = "Input/On Key Held",
        dynamicDescription = "While the $ key is held down")]
    [StringArg(argType = ArgType.Value, choicePreset = PresetChoices.Keybinds, allowFunction = false, allowPreset = false)]
    public void OnKeyHeld(string key) { }

    [VisualScriptingEvent(icon = eventIcon,
        dropdownDescription = "On Event Message Sent",
        dynamicDescription = "On Event Message $ Sent")]
    [StringArg(argType = ArgType.Temp, tempLabel = "Message")]
    public void OnEventMessageReceived(string message) { }
}
