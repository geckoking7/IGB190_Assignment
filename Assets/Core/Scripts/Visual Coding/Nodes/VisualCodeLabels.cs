using System.Collections.Generic;
using UnityEngine;

public static class VisualCodeLabels
{
    public static class Layers
    {
        public static string Wall = "Wall";
    }

    public static class FlowStrings
    {
        public const string CurrentLineID = "CurrentLineID";
        public const string ActionsArePaused = "ActionsArePaused";
        public const string Wait = "Wait";
        public const string DoActionsXTimes = "DoActionsXTimes";
        public const string DoActionsXTimesStoringVariable = "DoActionsXTimesStoringVariable";

        public const string ForEachUnitInGroup = "ForEachUnitInGroup";
        public const string DoActionsWhileBool = "DoActionsWhileBool";
        public const string DoActionsIfBool = "DoActionsIfBool";

    }

    public static class NodeTypes
    {
        public const string EVENT = "Event";
        public const string CONDITION = "Condition";
        public const string ACTION = "Action";
    }

    public static class Folders
    {
        public const string LOGIC_CONTAINERS = "General Scripts";
        public const string ABILITIES = "Abilities";
        public const string ITEMS = "Items";
        public const string BUFFS = "Buffs";
        public const string ASSETS = "Assets";
        public const string TEMPLATES = "Templates";
        public const string RESOURCES = "Assets/Resources";
    }

    public static class Events
    {
        // Projectile Events
        public const string EVENT_PROJECTILE_OWNED_COLLIDES_WITH_UNIT = "ProjectileMadeByThisCollidesWithUnit";
        public const string EVENT_PROJECTILE_COLLIDES_WITH_UNIT = "ProjectileCollidesWithUnit";
        public const string EVENT_PROJECTILE_REACHES_GOAL = "ProjectileReachesGoal";
        public const string EVENT_PROJECTILE_COLLIDES_WITH_TERRAIN = "ProjectileCollidesWithTerrain";
        public const string EVENT_PROJECTILE_TIMES_OUT = "ProjectileTimesOut";

        // Pickup Events
        public const string EVENT_ON_PICKUP_GOLD = "OnPickupGold";
        public const string EVENT_ON_PICKUP_HEALTH = "OnPickupHealth";

        public const string EVENT_SCRIPT_LOADED = "ScriptLoaded";
        public const string EVENT_SCRIPT_UNLOADED = "ScriptUnloaded";

        // Region Events
        public const string EVENT_REGION_ENTER = "UnitEntersRegion";
        public const string EVENT_REGION_EXIT = "UnitExitsRegion";

        // Timer Events
        public const string EVENT_TIMER_ONE_OFF_FINISHED = "OnOneOffTimerFinished";
        public const string EVENT_TIMER_CONTINUOUS_FINISHED = "OnTimerFinished";
        public const string EVENT_EVERY_FRAME = "EveryFrame";

        // Quest Events
        public const string EVENT_ON_QUEST_COMPLETED = "OnQuestCompleted";
        public const string EVENT_ON_QUEST_RECEIVED = "OnQuestReceived";

        // Item Events
        public const string EVENT_ITEM_PICKED_UP = "OnItemPickedUp";
        public const string EVENT_ITEM_SOLD = "OnItemSold";
        public const string EVENT_ITEM_BOUGHT = "OnItemBought";
        public const string EVENT_ITEM_EQUIPPED = "OnItemEquipped";
        public const string EVENT_ITEM_UNEQUIPPED = "OnItemUnequipped";

        // Interactable Events
        public const string EVENT_INTERACTION_STARTED = "OnInteractionStarted";
        public const string EVENT_INTERACTION_FINISHED = "OnInteractionFinished";

        // Item Set Events
        public const string EVENT_ITEM_SETITEM_EQUIPPED = "OnItemEquippedFromSet";
        public const string EVENT_ITEM_SETITEM_UNEQUIPPED = "OnItemUnequippedFromSet";

        // Unit Events
        public const string EVENT_UNIT_DAMAGED = "OnUnitDamaged";
        public const string EVENT_UNIT_KILLED = "WhenUnitIsKilled";
        public const string EVENT_UNIT_HEALED = "WhenUnitIsHealed";
        public const string EVENT_UNIT_CAST_STARTED = "UnitStartCast";
        public const string EVENT_UNIT_CAST_FINISHED = "UnitFinishCast";

        // Player Events
        public const string EVENT_PLAYER_LEVEL_UP = "OnPlayerLevelUp";

        // Input Events
        public const string EVENT_KEY_DOWN = "OnKeyDown";
        public const string EVENT_KEY_UP = "OnKeyUp";
        public const string EVENT_KEY_HELD = "OnKeyHeld";

        // Buff Events
        public const string EVENT_BUFF_APPLIED = "BuffApplied";
        public const string EVENT_BUFF_REMOVED = "BuffRemoved";
        public const string EVENT_BUFF_STACKS_CHANGED = "BuffStacksChanged";

        // Event Messages
        public const string EVENT_MESSAGE_RECEIVED = "OnEventMessageReceived";
    }

    public static class Presets
    {
        public static class Events
        {
            public static class Strings
            {
                public const string PRESET_INTERACTABLE_LABEL = "Interactable Label";
                public static string[] STRING_PRESETS = new string[]
                {
                    PRESET_INTERACTABLE_LABEL,
                };
            }

            public static class Items
            {
                public const string PRESET_TRIGGERING_ITEM = "Triggering Item";
                public static string[] ITEM_PRESETS = new string[]
                {
                    PRESET_TRIGGERING_ITEM,
                };
            }

            public static class Buffs
            {
                public const string PRESET_TRIGGERING_BUFF = "Triggering Buff";
                public static string[] BUFF_PRESETS = new string[]
                {
                    PRESET_TRIGGERING_BUFF,
                };
            }

            public static class Projectiles
            {
                public const string PRESET_EVENT_PROJECTILE = "Event Projectile";
                public static string[] PROJECTILE_PRESETS = new string[]
                {
                    PRESET_EVENT_PROJECTILE
                };
            }

            public static class Bools
            {
                public const string PRESET_IS_CRITICAL = "Is Critical";
                public static string[] BOOL_PRESETS = new string[]
                {
                    PRESET_IS_CRITICAL
                };
            }

            public static class Units
            {
                public const string PRESET_TRIGGERING_UNIT = "Triggering Unit";
                public const string PRESET_CASTING_UNIT = "Casting Unit";
                public const string PRESET_TARGET_UNIT = "Target Unit";
                public const string PRESET_DAMAGING_UNIT = "Damaging Unit";
                public const string PRESET_DAMAGED_UNIT = "Damaged Unit";
                public const string PRESET_KILLING_UNIT = "Killing Unit";
                public const string PRESET_KILLED_UNIT = "Killed Unit";
                public const string PRESET_HEALING_UNIT = "Healing Unit";
                public const string PRESET_HEALED_UNIT = "Healed Unit";
                public const string PRESET_COLLIDING_UNIT = "Colliding Unit";
                public const string PRESET_GOAL_UNIT = "Goal Unit";
                public const string PRESET_BUFF_APPLIER = "Buff Applier";
                public const string PRESET_BUFF_REMOVER = "Buff Remover";
                public const string PRESET_BUFF_TARGET = "Buff Target";
                public static string[] UNIT_PRESETS = new string[]
                {
                    PRESET_TRIGGERING_UNIT,
                    PRESET_CASTING_UNIT,
                    PRESET_TARGET_UNIT,
                    PRESET_DAMAGING_UNIT,
                    PRESET_DAMAGED_UNIT,
                    PRESET_KILLING_UNIT,
                    PRESET_KILLED_UNIT,
                    PRESET_HEALING_UNIT,
                    PRESET_HEALED_UNIT,
                    PRESET_COLLIDING_UNIT,
                    PRESET_GOAL_UNIT,
                    PRESET_BUFF_APPLIER,
                    PRESET_BUFF_REMOVER,
                    PRESET_BUFF_TARGET
                };
            }

            public static class Interactables
            {
                public const string PRESET_INTERACTABLE = "Interactable";
                public static string[] INTERACTABLE_PRESETS = new string[]
                {
                    PRESET_INTERACTABLE,
                };
            }

            public static class Abilities
            {
                public const string PRESET_KILLING_ABILITY = "Killing Ability";
                public const string PRESET_DAMAGING_ABILITY = "Damaging Ability";
                public const string PRESET_HEALING_ABILITY = "Healing Ability";
                public const string PRESET_ABILITY_CAST = "Ability Cast";
                public static string[] ABILITY_PRESETS = new string[]
                {
                    PRESET_KILLING_ABILITY,
                    PRESET_DAMAGING_ABILITY,
                    PRESET_HEALING_ABILITY,
                    PRESET_ABILITY_CAST,
                };
            }

            public static class Vectors
            {
                public const string PRESET_TARGET_POSITION = "Ability Target Location";
                public const string PRESET_GOAL_POSITION = "Goal Position";
                public const string PRESET_INTERACTABLE_POSITION = "Interactable Position";
                public static string[] VECTOR_PRESETS = new string[]
                {
                    PRESET_TARGET_POSITION,
                    PRESET_GOAL_POSITION,
                    PRESET_INTERACTABLE_POSITION
                };
            }

            public static class Numbers
            {
                public const string PRESET_GOLD_PICKED_UP = "Gold Added";
                public const string PRESET_HEALTH_PICKED_UP = "Health Restored";
                public const string PRESET_RESOURCES_GAINED = "Resources Gained";
                public const string PRESET_RESOURCES_LOST = "Resources Lost";
                public const string PRESET_DAMAGE_DEALT = "Damage Dealt";
                public const string PRESET_BUFF_STACK_COUNT = "Buff Stacks";
                public const string PRESET_BUFF_STACK_COUNT_CHANGE = "Change";
                public const string PRESET_AMOUNT_HEALED = "Amount Healed";
                public const string PRESET_INTERACTABLE_UNIQUE_ID = "Interactable Unique ID";
                public const string PRESET_SELL_PRICE = "Sell Price";
                public const string PRESET_PURCHASE_PRICE = "Purchase Price";
                public const string PRESET_REGION_ID = "Unique Region ID";
                public static string[] NUMBER_PRESETS = new string[]
                {
                    PRESET_GOLD_PICKED_UP,
                    PRESET_HEALTH_PICKED_UP,
                    PRESET_RESOURCES_GAINED,
                    PRESET_RESOURCES_LOST,
                    PRESET_DAMAGE_DEALT,
                    PRESET_BUFF_STACK_COUNT,
                    PRESET_BUFF_STACK_COUNT_CHANGE,
                    PRESET_AMOUNT_HEALED,
                    PRESET_INTERACTABLE_UNIQUE_ID,
                    PRESET_SELL_PRICE,
                    PRESET_PURCHASE_PRICE,
                    PRESET_REGION_ID
                };
            }

            public class Dynamic
            {
                public const string PRESET_UNIT_PLAYER = "Player";
                public const string PRESET_UNIT_LAST_CREATED = "Last Spawned Unit";
                public const string PRESET_PROJECTILE_LAST_CREATED = "Last Created Projectile";
                public const string PRESET_ABILITY_OWNER = "Ability Owner";
                public const string PRESET_ITEM_OWNER = "Item Owner";
                public const string PRESET_ABILITY_THIS = "This Ability";
                public const string PRESET_ITEM_THIS = "This Item";
                public const string PRESET_TIME_SINCE_START = "Time Since Level Start";
                public const string PRESET_PLAYER_LEVEL = "Player Level";
                public const string PRESET_ALL_UNITS = "All Units";
                public const string PRESET_ALL_ALLIES = "All Allies";
                public const string PRESET_ALL_NONPLAYER_ALLIES = "All Non-Player Allies";
                public const string PRESET_ALL_ENEMIES = "All Enemies";
                public const string PRESET_ALL_MONSTERS = "All Monsters";
                public const string PRESET_BUFF_THIS = "This Buff";
                public const string PRESET_UNIT_BUFF_OWNER = "Buff Owner";
                public const string PRESET_UNIT_BUFF_APPLIER = "Buff Applier";
                public const string PRESET_UNIT_BUFF_STACKS = "Buff Stacks";
                public static Dictionary<string, string> DYNAMIC_PRESETS = new Dictionary<string, string>()
                {
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_UNIT_PLAYER, "GetPlayer" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_UNIT_LAST_CREATED, "GetLastCreatedUnit" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_PROJECTILE_LAST_CREATED, "LastCreatedProjectile" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_OWNER, "GetOwner" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ITEM_OWNER, "GetOwner" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ABILITY_THIS, "ThisAbility" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ITEM_THIS, "ThisItem" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_UNITS, "AllUnits" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ALLIES, "AllAllies" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_NONPLAYER_ALLIES, "AllNonPlayerAllies" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_ENEMIES, "AllEnemies" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_ALL_MONSTERS, "AllMonsters" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_BUFF_THIS, "ThisBuff" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_UNIT_BUFF_STACKS, "BuffStackCount" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_UNIT_BUFF_OWNER, "GetBuffOwner" },
                    { VisualCodeLabels.Presets.Events.Dynamic.PRESET_UNIT_BUFF_APPLIER, "GetBuffApplier" },
                };
            }
        }
    }

    public static class EventPresets
    {

    }

    public static class Errors
    {
        public const string InvalidUnitGroup = "The unit group given was invalid.";
        public const string InvalidUnit = "The unit specified doesn't exist.";
        public const string InvalidItem = "The item specified doesn't exist.";
        public const string InvalidItemSet = "The item set specified doesn't exist.";
        public const string InvalidBuff = "The buff specified doesn't exist.";
        public const string InvalidAbility = "The ability specified doesn't exist.";
        public const string NoAbilities = "The specified unit has no abilities.";
        public const string InvalidEffect = "The effect specified doesn't exist.";
        public const string InvalidInteractable = "The interactable specified doesn't exist.";
        public const string InvalidChild = "The child object specified doesn't exist.";
        public const string NotYetImplemented = "The specified action doesn't yet exist (the logic has not been implemented).";
        public const string InvalidAudioClip = "The specified audio clip doesn't exist.";
        public const string InvalidPlayer = "No player exists. Is there a GameObject with an active Player component in the scene?";
        public const string InvalidVariable = "The variable name was invalid. Variable names cannot be empty or undefined.";
        public const string InvalidText = "The text was invalid. Text cannot be empty or undefined.";

        public const string UnitMustBeMonster = "The specified unit must be a monster.";

        public const string InvalidRegionName = "Invalid region name. Region names cannot be empty.";
        public const string InvalidQuestName = "Invalid quest name. Quest names cannot be empty.";
        public const string InvalidQuestReward = "Invalid quest reward. Quest rewards cannot be empty.";
        public const string InvalidQuestAlreadyInProgress = "Invalid quest. Cannot add this quest to the player as it is already in progress.";
        public const string InvalidProjectile = "The specified projectile doesn't exist.";
        public const string InvalidQuestRequirementName = "Invalid quest requirement name. Requirement names cannot be empty.";
        public const string InvalidQuestRequirement = "The specified quest does not have the specified requirement.";
        public const string InvalidQuest = "Invalid Quest. The player does not currently have a quest with this name.";

        public const string CouldNotShakeScreen = "Error trying to shake the camera.";

        public const string NoBuffOwner = "The 'Buff Owner' only resolves in the Buff Editor.";
        public const string NoBuffStackCount = "The 'Buff Stack Count' only resolves in the Buff Editor.";
        public const string NoBuffApplier = "The 'Buff Applier' only resolves in the Buff Editor.";
    }

    public static class Editor
    {
        public const string ABILITY_TARGETS_TOOLTIP = "The targeting mode of the ability determines if the ability is valid to cast.\n\n" +
            "<b>None</b>: The ability doesn't have targeting requirements.\n\n" +
            "<b>Unit in Melee</b>: The caster must have a target (e.g., be hovering of a monster) that is in melee range.\n\n" +
            "<b>Unit At Ranged</b>: The caster must have a target (e.g., be hovering of a monster) that is in range.\n\n" +
            "<b>Point in Melee</b>: The caster must have a valid target position (e.g., be hovering over a point in melee).\n\n" +
            "<b>Point At Ranged</b>: The caster must have a valid target position (e.g., be hovering over a point at range).";
        public static GUIContent ABILITY_TARGETS_CONTENT = new GUIContent("Ability Targets", ABILITY_TARGETS_TOOLTIP);

        public const string ABILITY_ANIMATION_TOOLTIP = "The animation that the unit will play when the ability is cast. If the ability shouldn't play an animation, select 'None' instead.\n\n" +
            "To add a custom animation, open the 'Player Animator' and attach your custom animations to any of the Custom1-5 slots. To add more animations to the list, go to the 'Unit' script and update the 'animations' array.";
        public static GUIContent ABILITY_ANIMATION_CONTENT = new GUIContent("Cast Animation", ABILITY_ANIMATION_TOOLTIP);

        public const string ABILITY_DESCRIPTION_TOOLTIP = "The ability description will be shown when the user hovers over the ability in the UI. It should concisely describe information about the ability. The cooldown, cost, and name are auto-generated and do not need to be listed.";
        public static GUIContent ABILITY_DESCRIPTION_CONTENT = new GUIContent("Ability Description", ABILITY_DESCRIPTION_TOOLTIP);

        public const string ABILITY_FLAVOUR_TOOLTIP = "The ability flavour text will be shown to the user when they hover over the ability at the bottom of the panel. This should not describe technical details about the ability. If no flavour text is listed, the bottom panel will not appear.";
        public static GUIContent ABILITY_FLAVOUR_CONTENT = new GUIContent("Ability Flavour Text", ABILITY_FLAVOUR_TOOLTIP);

        public const string ABILITY_TAG_TOOLTIP = "The ability tag is <b>optional</b> and can be used to categorise a group of abilities. E.g., if you want to trigger an effect whenever a 'Fire' ability is cast, you could tag all of those abilities with the 'Fire' tag and then check for whether the ability being cast has that tag.";
        public static GUIContent ABILITY_TAG_CONTENT = new GUIContent("Tag", ABILITY_TAG_TOOLTIP);

        public const string ABILITY_CAST_WHILE_MOVING_TOOLTIP = "If enabled, the casting unit can continue to move at full speed and change direction while the cast is in progress.";
        public static GUIContent ABILITY_CAST_WHILE_MOVING_CONTENT = new GUIContent(" Can Cast While Moving", ABILITY_CAST_WHILE_MOVING_TOOLTIP);

        public const string ABILITY_HAS_SPECIFIC_CAST_TIME_TOOLTIP = "If enabled, you can enter an exact cast time for the ability. If disabled, the cast time will be automatically calculated (and usually set to one second).";
        public static GUIContent ABILITY_HAS_SPECIFIC_CAST_TIME_CONTENT = new GUIContent(" Has Specific Cast Time", ABILITY_HAS_SPECIFIC_CAST_TIME_TOOLTIP);

        public const string ABILITY_REQUIRES_LINE_OF_SIGHT_TOOLTIP = "If enabled, the target position of the ability will be recalculated so that it falls at the furthest point towards the target location that is still in line of sight.";
        public static GUIContent ABILITY_REQUIRES_LINE_OF_SIGHT_CONTENT = new GUIContent(" Requires Line of Sight", ABILITY_REQUIRES_LINE_OF_SIGHT_TOOLTIP);

        public const string ABILITY_UPDATE_TARGET_WHILE_CASTING_TOOLTIP = "If enabled, the target location of the ability will keep updating after the ability starts casting. It is recommended that you enable this for melee attacks, but <b>disable</b> it for ranged attacks so that the player can prepare to move.";
        public static GUIContent ABILITY_UPDATE_TARGET_WHILE_CASTING_CONTENT = new GUIContent(" Update Target While Casting", ABILITY_UPDATE_TARGET_WHILE_CASTING_TOOLTIP);

        public const string ABILITY_COOLDOWN_IS_ATTACK_SPEED_TOOLTIP = "If enabled, the cooldown of the ability will be set to the exact attack speed of the unit. This will ensure that an increase in attack speed changes how fast the player attacks with this ability. If this isn't enabled, attack speed will not affect the ability at all!";
        public static GUIContent ABILITY_COOLDOWN_IS_ATTACK_SPEED_CONTENT = new GUIContent(" Cooldown is Attack Speed", ABILITY_COOLDOWN_IS_ATTACK_SPEED_TOOLTIP);

        public const string ABILITY_GENERATES_RESOURCES_TOOLTIP = "If enabled, this ability will <b>generate</b> resources when cast, rather than consume them. You set the exact value below in the 'Resources Gained' field.";
        public static GUIContent ABILITY_GENERATES_RESOURCES_CONTENT = new GUIContent(" Ability Generates Resources", ABILITY_GENERATES_RESOURCES_TOOLTIP);

        public const string ABILITY_RANGE_TOOLTIP = "The range of the ability. If the player tries to cast an ability and they are out of range, it will cast at the closest valid position.";
        public static GUIContent ABILITY_RANGE_CONTENT = new GUIContent("Range", ABILITY_RANGE_TOOLTIP);

        public const string ABILITY_RESOURCE_GAIN_TOOLTIP = "This determines the amount of resources gained when the ability is cast.\n\nIf you want the ability to cost resources, change the 'Ability Generates Tooltip' checkbox above.";
        public static GUIContent ABILITY_RESOURCE_GAIN_CONTENT = new GUIContent("Resource Gain", ABILITY_RESOURCE_GAIN_TOOLTIP);

        public const string ABILITY_RESOURCE_COST_TOOLTIP = "This determines the amount of resources lost when the ability is cast. If the unit does not have enough resources, the ability will not be cast.\n\nIf you want the ability to gain resources, change the 'Ability Generates Tooltip' checkbox above.";
        public static GUIContent ABILITY_RESOURCE_COST_CONTENT = new GUIContent("Resource Cost", ABILITY_RESOURCE_COST_TOOLTIP);

        public const string ABILITY_COOLDOWN_TOOLTIP = "The cooldown determines how long the unit must wait before casting the ability again.";
        public static GUIContent ABILITY_COOLDOWN_CONTENT = new GUIContent("Cooldown", ABILITY_COOLDOWN_TOOLTIP);

        public const string ABILITY_CAST_TIME_TOOLTIP = "The cast time determines how long the ability takes to cast. To set a specific value here, enable the 'Has Specific Cast Time' toggle above.";
        public static GUIContent ABILITY_CAST_TIME_CONTENT = new GUIContent("Cast Time", ABILITY_CAST_TIME_TOOLTIP);

        public const string ABILITY_SOUND_EFFECT_TOOLTIP = "The sound effect that will play when the ability is cast. If this is left empty, no sound will play.";
        public static GUIContent ABILITY_SOUND_EFFECT_CONTENT = new GUIContent("Ability Sound Effect", ABILITY_SOUND_EFFECT_TOOLTIP);

        public const string ABILITY_SOUND_EFFECT_VOLUME_TOOLTIP = "The volume of the above sound effect. If you need finer control over the sound (e.g., playing a different sound if more units are hit), you should trigger the sound effect in the visual code instead.";
        public static GUIContent ABILITY_SOUND_EFFECT_VOLUME_CONTENT = new GUIContent("Volume", ABILITY_SOUND_EFFECT_VOLUME_TOOLTIP);

        public const string ABILITY_TRIGGER_POINT_TOOLTIP = "This determines <b>when</b> in the animation that the ability should finish casting. By default, if the cast time is set to automatic, it will use this slider (a value of 0.5 will trigger 50% through the animation). Choosing a lower value will make the ability feel more responsive.";
        public static GUIContent ABILITY_TRIGGER_POINT_CONTENT = new GUIContent("Animation Trigger Point", ABILITY_TRIGGER_POINT_TOOLTIP);

        public const string ITEM_ITEM_SLOT_TOOLTIP = "This option controls which slot the item can be equipped in. If the item shouldn't be equippable (e.g., it is a special key that the player will use), you can use the <b>Other</b> slot.";
        public static GUIContent ITEM_ITEM_SLOT_CONTENT = new GUIContent("Item Slot", ITEM_ITEM_SLOT_TOOLTIP);

        public const string ITEM_ITEM_RARITY_TOOLTIP = "This option determines the rarity of the item. The rarity controls the chance the item will drop, and will visibly change the color and description on the item.";
        public static GUIContent ITEM_ITEM_RARITY_CONTENT = new GUIContent("Item Rarity", ITEM_ITEM_RARITY_TOOLTIP);

        public const string ITEM_ITEM_SET_TOOLTIP = "This option determines which set this item is associated with. To create a new set, right click in the project and select <b>Create > Item Set</b>. This allows you to specify the bonuses which will be given at each threshold.";
        public static GUIContent ITEM_ITEM_SET_CONTENT = new GUIContent("Item Set", ITEM_ITEM_SET_TOOLTIP);

        public const string ITEM_TOOLTIP_TOOLTIP = "The item description will be shown when the user hovers over the ability in the UI. It should concisely describe information about the item. The stats, cost, and name are auto-generated and do not need to be listed.";
        public static GUIContent ITEM_TOOLTIP_CONTENT = new GUIContent("Item Tooltip", ITEM_TOOLTIP_TOOLTIP);

        public const string ITEM_FLAVOUR_TEXT_TOOLTIP = "The item flavour text will be shown to the user when they hover over the item at the bottom of the panel. This should not describe technical details about the item. If no flavour text is listed, the bottom panel will not appear.";
        public static GUIContent ITEM_FLAVOUR_TEXT_CONTENT = new GUIContent("Item Flavour Text", ITEM_FLAVOUR_TEXT_TOOLTIP);

        public const string ITEM_TAG_TOOLTIP = "The item tag is <b>optional</b> and can be used to categorise a group of items. E.g., if you want to trigger an effect if the player has a 'Cursed' item, you could tag all of the cursed items with the 'Cursed' tag and then check for whether the player has an item with that tag.";
        public static GUIContent ITEM_TAG_CONTENT = new GUIContent("Tag", ITEM_TAG_TOOLTIP);

        public const string ITEM_CLASS_REQUIRED_TOOLTIP = "The item class is an <b>optional</b> property. If no class is entered, the item will be available to all player classes. Otherwise, it will only drop for the player with the specified name (make sure that the name is spelled correctly!).";
        public static GUIContent ITEM_CLASS_REQUIRED_CONTENT = new GUIContent("Class Required", ITEM_CLASS_REQUIRED_TOOLTIP);

        public const string ITEM_MIN_DROP_LEVEL_TOOLTIP = "Determines the <b>minimum</b> player level at which the item can drop from monsters. If you want the item to be able to drop at any level, enter a value of <b>0</b>.";
        public static GUIContent ITEM_MIN_DROP_LEVEL_CONTENT = new GUIContent("Min Drop Level", ITEM_MIN_DROP_LEVEL_TOOLTIP);

        public const string ITEM_CAN_PURCHASE_TOOLTIP = "If checked, the item will appear in the shop. While it cannot be purchased, it may still be able to drop from monsters (depending on the settings below).";
        public static GUIContent ITEM_CAN_PURCHASE_CONTENT = new GUIContent(" Can Purchase in Shop", ITEM_CAN_PURCHASE_TOOLTIP);

        public const string ITEM_CAN_DROP_TOOLTIP = "If checked, the item can drop from monsters if all other conditions are met (minimum level, required class etc). You may want to use this setting if the item should only be obtainable from a specific event or action in the game.";
        public static GUIContent ITEM_CAN_DROP_CONTENT = new GUIContent(" Can Drop From Monster", ITEM_CAN_DROP_TOOLTIP);

        public const string ITEM_RANDOM_STATS_TOOLTIP = "Determines the number of random stats that the item will roll. For example, if you list 10 possible random stats, but have a value of 3 in this field, it will randomly choose three stats from that list of ten.";
        public static GUIContent ITEM_RANDOM_STATS_CONTENT = new GUIContent("Random Stats", ITEM_RANDOM_STATS_TOOLTIP);

        public const string ITEM_PURCHASE_COST_TOOLTIP = "The purchase cost of the item. This should be set even if the item cannot be purchased, because the sell price is based on the purchase value.";
        public static GUIContent ITEM_PURCHASE_COST_CONTENT = new GUIContent("Purchase Cost", ITEM_PURCHASE_COST_TOOLTIP);
    }
}
