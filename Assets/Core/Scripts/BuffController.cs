using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BuffController
{
    public List<Buff> buffs = new List<Buff>();

    Dictionary<Buff, Buff> TemplateToUnitSpecificBuff = new Dictionary<Buff, Buff>();

    public Unit unit;

    public BuffController (Unit unit)
    {
        this.unit = unit;
    }

    public void Update ()
    {
        List<Buff> toRemove = new List<Buff>();

        foreach (Buff buff in buffs)
        {
            buff.buffCurrentDuration -= Time.deltaTime;
            if (buff.buffCurrentDuration < 0)
            {
                toRemove.Add(buff);
            }
        }

        foreach (Buff buff in toRemove)
        {
            RemoveBuff(buff);
        }
    }

    public bool HasBuff(Buff buff)
    {
        return GetUnitBuff(buff) != null;
    }

    public Buff GetUnitBuff (Buff buff)
    {
        if (TemplateToUnitSpecificBuff.ContainsKey(buff.template))
            return TemplateToUnitSpecificBuff[buff.template];
        else
            return null;
    }

    public void AddBuff (Buff buff, Unit unitApplyingBuff = null, int stacks = 1)
    {
        if (HasBuff(buff))
        {
            AddStacks(buff, stacks);
        }
        else
        { 
            Buff buffCopy = buff.ShallowCopy();
            TemplateToUnitSpecificBuff[buffCopy.template] = buffCopy;
            buffCopy.buffCurrentStacks = Mathf.Min(stacks, buffCopy.buffMaximumStacks);
            buffs.Add(buffCopy);

            foreach (var buffStatBonus in buffCopy.buffStatBonuses)
            {
                if (buffStatBonus.modifier.IsPercentage)
                {
                    unit.stats[buffStatBonus.stat].AddPercentageModifier(buffStatBonus.modifier.Value, buffCopy.name, stacks);
                }
                else
                    unit.stats[buffStatBonus.stat].AddValueModifier(buffStatBonus.modifier.Value, buffCopy.name, stacks);
            }

            GameManager.events.OnBuffAdded.Invoke((new GameEvents.OnBuffAddedInfo(buffCopy, unitApplyingBuff, unit, stacks)));
            GameManager.logicEngine.AddEngine(buffCopy.engine);
        }
    }

    public void RemoveBuff (Buff buff, Unit unitRemovingBuff = null)
    {
        if (TemplateToUnitSpecificBuff.ContainsKey(buff.template))
        {
            Buff unitBuff = TemplateToUnitSpecificBuff[buff.template];
            TemplateToUnitSpecificBuff.Remove(buff.template);
            buffs.Remove(unitBuff);

            foreach (var buffStatBonus in buff.buffStatBonuses)
            {
                unit.stats[buffStatBonus.stat].RemoveModifiersWithLabel(buff.name);
            }

            GameManager.events.OnBuffRemoved.Invoke((new GameEvents.OnBuffRemovedInfo(buff, unitRemovingBuff, unit, buff.buffCurrentStacks)));
            GameManager.logicEngine.RemoveEngine(buff.engine);

            if (unitBuff.visualEffect != null)
            {
                unit.RemoveVisualEffect(unitBuff.visualEffect);
            }
        }
    }

    public void AddStacks (Buff buff, int stacks)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentStacks += stacks;
        if (unitBuff.addingStacksRefreshesDuration) unitBuff.buffCurrentDuration = unitBuff.buffMaxDuration; 
        foreach (var buffStatBonus in buff.buffStatBonuses)
        {
            unit.stats[buffStatBonus.stat].SetStackCount(buff.name, buff.buffCurrentStacks);
            unit.stats[buffStatBonus.stat].SetDuration(buff.name, buff.buffCurrentDuration);
        }

        if (buff.visualEffect != null)
        {
            unit.SpawnOrRefreshEffect(buff.visualEffect, 9999f, 1.0f, Vector3.zero);
        }
    }

    public void RemoveStacks (Buff buff, int stacks)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentStacks -= stacks;
        foreach (var buffStatBonus in buff.buffStatBonuses)
        {
            unit.stats[buffStatBonus.stat].SetStackCount(buff.name, buff.buffCurrentStacks);
        }
    }

    public void SetStacks (Buff buff, int stacks)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentStacks = stacks;
        foreach (var buffStatBonus in buff.buffStatBonuses)
        {
            unit.stats[buffStatBonus.stat].SetStackCount(buff.name, buff.buffCurrentStacks);
        }
    }

    public void SetMaxStacks (Buff buff, int stacks)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffMaximumStacks = stacks;
    }

    public void ModifyBuffDuration (Buff buff, float change)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentDuration += change;
    }

    public void SetBuffDuration (Buff buff, float duration)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentDuration = duration;
    }

    public void SetBuffMaxDuration (Buff buff, float duration)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffMaxDuration = duration;
    }

    public void RefreshDuration (Buff buff)
    {
        Buff unitBuff = GetUnitBuff(buff);
        if (unitBuff == null) return;
        unitBuff.buffCurrentDuration = unitBuff.buffMaxDuration;
    }




    

    public Buff GetUnitBuffMatchingTemplate (Buff buffTemplate)
    {
        if (buffTemplate.template != null) buffTemplate = buffTemplate.template;
        if (TemplateToUnitSpecificBuff.ContainsKey(buffTemplate))
            return TemplateToUnitSpecificBuff[buffTemplate];
        else
            return null;
    }

    public float GetBuffCurrentDuration (Buff buff)
    {
        Buff unitBuff = GetUnitBuffMatchingTemplate (buff);
        if (unitBuff == null) return 0;
        return unitBuff.buffCurrentDuration;
    }

    public float GetBuffMaxDuration (Buff buff)
    {
        Buff unitBuff = GetUnitBuffMatchingTemplate(buff);
        if (unitBuff == null) return 0;
        return unitBuff.buffMaxDuration;
    }

    public int GetBuffCurrentStacks (Buff buff)
    {
        Buff unitBuff = GetUnitBuffMatchingTemplate(buff);
        if (unitBuff == null) return 0;
        return unitBuff.buffCurrentStacks;
    }

    public int GetBuffMaxStacks (Buff buff)
    {
        Buff unitBuff = GetUnitBuffMatchingTemplate(buff);
        if (unitBuff == null) return 0; 
        return unitBuff.buffMaximumStacks;
    }
}
