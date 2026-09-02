using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class VisualCodeManager : MonoBehaviour
{
    public bool showErrors = true;
    public bool stopScriptExecutionOnError = false;
    private List<LogicEngine> engines = new List<LogicEngine>();
    private Dictionary<string, List<LogicEngine>> eventLookup = new Dictionary<string, List<LogicEngine>>();

    private KeyCode[] keys;

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        keys = (KeyCode[])System.Enum.GetValues(typeof(KeyCode));
        Setup();
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        foreach (LogicEngine engine in engines)
            foreach (VisualCodeTimer timer in engine.activeTimers)
                timer.Update(engine);

        
        foreach (KeyCode key in keys)
        {
            if (Input.GetKeyDown(key))
            {
                TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_KEY_DOWN, key.ToString());
            }
            if (Input.GetKeyUp(key))
            {
                TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_KEY_UP, key.ToString());
            }
            if (Input.GetKey(key))
            {
                TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_KEY_HELD, key.ToString());
            }
        }
        

        TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_EVERY_FRAME);
    }

    /// <summary>
    /// Add this engine so that it is handled by the manager.
    /// </summary>
    public void AddEngine (LogicEngine engine)
    {
        engines.Add(engine);
        engine.Setup();

        // Cache event lookups for faster access later.
        foreach (VisualCodeScript script in engine.scripts)
        {
            foreach (GeneralNode node in script.eventNodes)
            {
                if (!eventLookup.ContainsKey(node.functionName))
                {
                    eventLookup.Add(node.functionName, new List<LogicEngine>());
                    
                }
                eventLookup[node.functionName].Add(engine);
            }
        }
        engine.TriggerEvent(null, VisualCodeLabels.Events.EVENT_SCRIPT_LOADED);
    }

    /// <summary>
    /// No longer manage the specified engine.
    /// </summary>
    public void RemoveEngine (LogicEngine engine)
    {
        engine.DisableTimers();
        engines.Remove(engine);
        foreach (List<LogicEngine> list in eventLookup.Values)
            list.RemoveAll(x => x.Equals(engine));
        engine.TriggerEvent(null, VisualCodeLabels.Events.EVENT_SCRIPT_UNLOADED);
    }

    /// <summary>
    /// Trigger the specified event on ALL listening engines (with any appropriate presets).
    /// </summary>
    public void TriggerEventOnAllEngines (Dictionary<string, object> presets, string eventCall, object reqs = null)
    {
        if (!eventLookup.ContainsKey(eventCall)) return;
        if (presets == null) presets = new Dictionary<string, object>();

        List<LogicEngine> copy = new List<LogicEngine>(engines);
        foreach (LogicEngine engine in copy)
        {
            foreach (VisualCodeScript script in engine.scripts)
            {
                try
                {
                    script.RunScript(presets, engine, eventCall, reqs);
                }
                catch { }
            }
        }
    }

    /// <summary>
    /// Trigger the specified event on the given engine (with any appropriate presets).
    /// </summary>
    public void TriggerEventOnEngine (LogicEngine engine, Dictionary<string, object> presets, string eventCall)
    {
        if (engine == null) return;
        if (presets == null) presets = new Dictionary<string, object>();
        foreach (VisualCodeScript script in engine.scripts)
        {
            script.RunScript(presets, engine, eventCall);
        }
    }

    /// <summary>
    /// Perform all basic setup for the engine manager. This links the game events to the visual scripting
    /// nodes so that they will be run when the appropriate game actions occur. If this link is not done,
    /// the event will never fire.
    /// </summary>
    private void Setup ()
    {
        GameManager.events.OnAbilityCastStarted.AddListener((eventInfo) => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, eventInfo.castingUnit },
                { VisualCodeLabels.Presets.Events.Abilities.PRESET_ABILITY_CAST, eventInfo.ability.template },
                { VisualCodeLabels.Presets.Events.Units.PRESET_TARGET_UNIT, eventInfo.targetUnit },
                { VisualCodeLabels.Presets.Events.Vectors.PRESET_TARGET_POSITION, eventInfo.targetPosition }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_UNIT_CAST_STARTED, eventInfo.ability.template);
        });

        GameManager.events.OnAbilityCastFinished.AddListener((eventInfo) => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, eventInfo.castingUnit },
                { VisualCodeLabels.Presets.Events.Abilities.PRESET_ABILITY_CAST, eventInfo.ability },
                { VisualCodeLabels.Presets.Events.Units.PRESET_TARGET_UNIT, eventInfo.targetUnit },
                { VisualCodeLabels.Presets.Events.Vectors.PRESET_TARGET_POSITION, eventInfo.targetPosition }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_UNIT_CAST_FINISHED, eventInfo.ability.template);
        });

        GameManager.events.OnUnitKilled.AddListener((eventInfo) => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_KILLED_UNIT, eventInfo.killedUnit },
                { VisualCodeLabels.Presets.Events.Units.PRESET_KILLING_UNIT, eventInfo.killingUnit },
                { VisualCodeLabels.Presets.Events.Abilities.PRESET_KILLING_ABILITY, (eventInfo.killingSource is Ability ? eventInfo.killingSource : null) },
                { VisualCodeLabels.Presets.Events.Bools.PRESET_IS_CRITICAL, eventInfo.isCrit },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_UNIT_KILLED);
        });

        GameManager.events.OnUnitHealed.AddListener((eventInfo) => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_HEALED_UNIT, eventInfo.healedUnit },
                { VisualCodeLabels.Presets.Events.Units.PRESET_HEALING_UNIT, eventInfo.healingUnit },
                { VisualCodeLabels.Presets.Events.Abilities.PRESET_HEALING_ABILITY, (eventInfo.healingSource is Ability ? eventInfo.healingSource : null) },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_AMOUNT_HEALED, eventInfo.healing },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_UNIT_HEALED);
        });

        GameManager.events.OnUnitDamaged.AddListener((args) => {
            Object data = args.damageSource == null ? null : args.damageSource.GetData();
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_DAMAGED_UNIT, args.damagedUnit },
                { VisualCodeLabels.Presets.Events.Units.PRESET_DAMAGING_UNIT, args.damagingUnit },
                { VisualCodeLabels.Presets.Events.Abilities.PRESET_DAMAGING_ABILITY, (data is Ability ? data : null) },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_DAMAGE_DEALT, args.damage },
                { VisualCodeLabels.Presets.Events.Bools.PRESET_IS_CRITICAL, args.isCritical }, 
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_UNIT_DAMAGED);
        });

        GameManager.events.OnPlayerEnteredRegion.AddListener(eventInfo =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT, eventInfo.enteringUnit },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_REGION_ID, (float)eventInfo.region.regionID },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_REGION_ENTER, eventInfo.regionName);
        });

        GameManager.events.OnPlayerExitedRegion.AddListener(eventInfo =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_TRIGGERING_UNIT, eventInfo.exitingUnit },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_REGION_ID, (float)eventInfo.region.regionID },

            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_REGION_EXIT, eventInfo.regionName);
        });

        GameManager.events.OnQuestCompleted.AddListener(quest => 
        {
            GameManager.logicEngine.TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_ON_QUEST_COMPLETED, quest.Label);
        });

        GameManager.events.OnQuestAdded.AddListener(quest =>
        {
            GameManager.logicEngine.TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_ON_QUEST_RECEIVED, quest.Label);
        });

        GameManager.events.OnItemPickedUp.AddListener(item => 
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM, item }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_PICKED_UP);
        });

        GameManager.events.OnBuffAdded.AddListener(buffAddedInfo =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_APPLIER, buffAddedInfo.unitApplyingBuff },
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET, buffAddedInfo.unitReceivingBuff },
                { VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF, buffAddedInfo.buff },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT, buffAddedInfo.stacks },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_BUFF_APPLIED);
        });

        GameManager.events.OnBuffRemoved.AddListener(buffRemovedInfo =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_APPLIER, buffRemovedInfo.unitRemovingBuff },
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET, buffRemovedInfo.unitLosingBuff },
                { VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF, buffRemovedInfo.buff },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT, buffRemovedInfo.stacks },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_BUFF_REMOVED);
        });

        GameManager.events.OnBuffStacksChanged.AddListener(buffStacksChangedInfo =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_APPLIER, buffStacksChangedInfo.buffApplier },
                { VisualCodeLabels.Presets.Events.Units.PRESET_BUFF_TARGET, buffStacksChangedInfo.buffTarget },
                { VisualCodeLabels.Presets.Events.Buffs.PRESET_TRIGGERING_BUFF, buffStacksChangedInfo.buff },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT, buffStacksChangedInfo.stackCount },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_BUFF_STACK_COUNT_CHANGE, buffStacksChangedInfo.stackChange },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_BUFF_APPLIED);
        });

        GameManager.events.OnItemSold.AddListener(item =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM, item.item },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_SELL_PRICE, item.sellPrice },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_SOLD);
        });

        GameManager.events.OnItemPurchased.AddListener(item =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM, item.item },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_PURCHASE_PRICE, item.purchasePrice },
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_BOUGHT);
        });

        GameManager.events.OnGoldPickedUp.AddListener(gold =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_GOLD_PICKED_UP, gold }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ON_PICKUP_GOLD);
        });

        GameManager.events.OnHealthPickedUp.AddListener(health =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_HEALTH_PICKED_UP, health }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ON_PICKUP_HEALTH);
        });


        GameManager.events.OnItemEquipped.AddListener(item =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM, item }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_EQUIPPED);
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_SETITEM_EQUIPPED, item.itemSet);
        });

        GameManager.events.OnItemUnequipped.AddListener(item =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Items.PRESET_TRIGGERING_ITEM, item }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_UNEQUIPPED);
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_ITEM_SETITEM_EQUIPPED, item.itemSet);
        });

        GameManager.events.OnInteractionStarted.AddListener(interactable =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Interactables.PRESET_INTERACTABLE, interactable },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_INTERACTABLE_UNIQUE_ID, interactable.uniqueID },
                { VisualCodeLabels.Presets.Events.Strings.PRESET_INTERACTABLE_LABEL, interactable.interactableLabel },
                { VisualCodeLabels.Presets.Events.Vectors.PRESET_INTERACTABLE_POSITION, interactable.transform.position }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_INTERACTION_STARTED);
        });

        GameManager.events.OnInteractionFinished.AddListener(interactable =>
        {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Interactables.PRESET_INTERACTABLE, interactable },
                { VisualCodeLabels.Presets.Events.Numbers.PRESET_INTERACTABLE_UNIQUE_ID, interactable.uniqueID },
                { VisualCodeLabels.Presets.Events.Strings.PRESET_INTERACTABLE_LABEL, interactable.interactableLabel },
                { VisualCodeLabels.Presets.Events.Vectors.PRESET_INTERACTABLE_POSITION, interactable.transform.position }
            };
            GameManager.logicEngine.TriggerEventOnAllEngines(presets, VisualCodeLabels.Events.EVENT_INTERACTION_FINISHED);
        });


        GameManager.events.OnPlayerLevelUp.AddListener(item =>
        {
            GameManager.logicEngine.TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_PLAYER_LEVEL_UP);
        });

        GameManager.events.OnEventMessageReceived.AddListener(message =>
        {
            GameManager.logicEngine.TriggerEventOnAllEngines(null, VisualCodeLabels.Events.EVENT_MESSAGE_RECEIVED, message.messageText);
        });

        GameManager.events.OnProjectileWallCollision.AddListener(info => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, info.castingUnit },
                { VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE, info.projectile }
            };
            info.engine.GetEngine().TriggerEvent(presets, VisualCodeLabels.Events.EVENT_PROJECTILE_COLLIDES_WITH_TERRAIN);
        });

        GameManager.events.OnProjectileCollision.AddListener(info => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, info.castingUnit },
                { VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE, info.projectile },
                { VisualCodeLabels.Presets.Events.Units.PRESET_COLLIDING_UNIT, info.collidingUnit },
            };
            info.engine.GetEngine().TriggerEvent(presets, VisualCodeLabels.Events.EVENT_PROJECTILE_OWNED_COLLIDES_WITH_UNIT);
        });

        GameManager.events.OnProjectileGoalReached.AddListener(info => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, info.castingUnit },
                { VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE, info.projectile },
                { VisualCodeLabels.Presets.Events.Units.PRESET_GOAL_UNIT, info.goalUnit },
                { VisualCodeLabels.Presets.Events.Vectors.PRESET_GOAL_POSITION, info.goalPosition }
            };
            info.engine.GetEngine().TriggerEvent(presets, VisualCodeLabels.Events.EVENT_PROJECTILE_REACHES_GOAL);
        });

        GameManager.events.OnProjectileTimeout.AddListener(info => {
            Dictionary<string, object> presets = new Dictionary<string, object>
            {
                { VisualCodeLabels.Presets.Events.Units.PRESET_CASTING_UNIT, info.castingUnit },
                { VisualCodeLabels.Presets.Events.Projectiles.PRESET_EVENT_PROJECTILE, info.projectile }
            };
            info.engine.GetEngine().TriggerEvent(presets, VisualCodeLabels.Events.EVENT_PROJECTILE_TIMES_OUT);
        });

        LogicContainer[] generalScripts = Resources.LoadAll<LogicContainer>(VisualCodeLabels.Folders.LOGIC_CONTAINERS);
        foreach (LogicContainer generalScript in generalScripts)
        {
            generalScript.engine.engineHandler = generalScript;
            AddEngine(generalScript.engine);
        }
    }

    private void OnDestroy()
    {
        LogicContainer[] generalScripts = Resources.LoadAll<LogicContainer>(VisualCodeLabels.Folders.LOGIC_CONTAINERS);
        foreach (LogicContainer generalScript in generalScripts)
            RemoveEngine(generalScript.engine);
    }
}
