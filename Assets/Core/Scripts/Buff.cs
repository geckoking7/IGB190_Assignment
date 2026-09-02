using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Buff", menuName = "Data/Buff", order = 1)]
public class Buff : ScriptableObject, IVisualCodeHandler
{
    // Buff properties
    public string buffDescription = "";
    public string buffFlavourText = "";
    public string buffTag = "";
    public Sprite buffIcon = null;
    public BuffType buffType = BuffType.Buff;

    public bool buffRemovedOnDeath = true;
    public bool buffVisibleInUI = true;
    public bool buffHasDuration = true;
    public bool addingStacksRefreshesDuration = true;

    public float buffCurrentDuration;
    public float buffMaxDuration = 10;

    public int buffCurrentStacks = 1;
    public int buffMaximumStacks = 1;

    public CustomVisualEffect buffEffectOnTarget;
    public string buffAttachPoint;
    public float buffScale;

    public List<BuffStatBonus> buffStatBonuses = new List<BuffStatBonus>();

    public CustomVisualEffect visualEffect = null;

    public enum BuffType
    {
        Buff,
        Debuff
    }

    [System.Serializable]
    public class BuffStatBonus
    {
        public Stat stat;
        public StatModifier.Modifier modifier;

        public BuffStatBonus (Stat stat, float amount, bool isPercent)
        {
            this.stat = stat;
            modifier = new StatModifier.Modifier(amount, 0, isPercent); 
        }

        public BuffStatBonus Copy ()
        {
            return new BuffStatBonus(stat, modifier.Value, modifier.IsPercentage);
        }
    }

    // Keep track of the raw buff template object.
    private Buff _template;
    public Buff template
    {
        get
        {
            if (_template == null)
                _template = this;
            return _template;
        }
        set
        {
            _template = value;
        }
    }

    // Keep track of who "owns" the buff (e.g., who it is affecting).
    public Unit owner;

    // Keep track of who "applied" the buff to the target.
    public Unit applier;

    // Handles all logic for the item from the visual engine.
    public LogicEngine engine = new LogicEngine();

    public IVisualCodeHandler CopyGeneral(string copyName)
    {
        Buff buff = Copy();
        return buff;
    }

    /// <summary>
    /// Creates a deep copy of the buff.
    /// </summary>
    public Buff Copy()
    {
        Buff buff = CreateInstance<Buff>();
        buff.buffDescription = buffDescription;
        buff.buffFlavourText = buffFlavourText;
        buff.buffType = buffType;
        buff.buffIcon = buffIcon;
        buff.buffTag = buffTag;

        buff.buffRemovedOnDeath = buffRemovedOnDeath;
        buff.buffVisibleInUI = buffVisibleInUI;
        buff.buffHasDuration = buffHasDuration;
        buff.addingStacksRefreshesDuration = addingStacksRefreshesDuration;

        buff.buffMaximumStacks = buffMaximumStacks;
        
        buff.buffMaxDuration = buffMaxDuration;
        buff.buffCurrentDuration = buffCurrentDuration;

        foreach (BuffStatBonus bonus in buffStatBonuses)
        { 
            buff.buffStatBonuses.Add(bonus.Copy());
        }

        buff.engine = engine.Copy();
        return buff;
    }

    /// <summary>
    /// Creates a shallow copy of the buff.
    /// </summary>
    public Buff ShallowCopy()
    {
        Buff buff = Instantiate(this);
        buff.name = name;
        buff.engine = engine.ShallowCopy(buff);
        buff.buffStatBonuses = buffStatBonuses;
        buff.buffCurrentDuration = buff.buffMaxDuration;
        if (template != null)
            buff.template = template;
        else
            buff.template = this;
        return buff;
    }

    /// <summary>
    /// Returns the current owner of the item, or null if the item is not currently owned.
    /// </summary>
    public Unit GetOwner() 
    {
        return owner;
    }

    /// <summary>
    /// Sets the owner of this item to the specified unit.
    /// </summary>
    public void SetOwner(Unit owner)
    {
        this.owner = owner;
    }

    /// <summary>
    /// Returns the generic object associated with this item. This is needed as part of the visual
    /// scripting interface.
    /// </summary>
    public UnityEngine.Object GetData()
    {
        return this;
    }

    /// <summary>
    /// Returns the script engine associated with this item (used for the visual scripting interface).
    /// </summary>
    public LogicEngine GetEngine()
    {
        return engine;
    }

    public string GetTag()
    {
        return buffTag;
    }

    public void SetName(string newName)
    {

    }

    public string GetName() => name;
    public Texture2D GetIcon() => buffIcon == null ? null : buffIcon.texture;
}