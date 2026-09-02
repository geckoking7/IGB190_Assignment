using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System.Collections.Generic;
using UnityEditor.Playables;
using System;

public class BuffEditor : BaseEditor
{
    protected override string ListItemFolder => VisualCodeLabels.Folders.BUFFS;

    protected override IVisualCodeHandler[] ListData => Resources.LoadAll<Buff>(ListItemFolder);

    protected Buff buff => selectedItem == null ? null : (Buff)selectedItem.GetData();

    protected override Type ManagedType => typeof(Buff);
    protected override string GetListHeaderText => "Buffs";


    private static List<Buff.BuffStatBonus> toDelete = new List<Buff.BuffStatBonus>();

    private static List<string> effectNames = new List<string>();
    private static List<CustomVisualEffect> effects = new List<CustomVisualEffect>();

    private static string[] optionStrings;
    private static CustomVisualEffect[] optionValues;

    /// <summary>
    /// Draw a single item stat block for the item.
    /// </summary>
    private void DrawStatBlock(Buff buff, Buff.BuffStatBonus statBonus, Rect rect)
    {
        EditorGUI.DrawRect(rect, new Color(0.25f, 0.25f, 0.25f));

        // Draw the delete button.
        Rect deleteRect = new Rect(rect.x + rect.width - 21, rect.y - 1, rect.height - 1, rect.height - 1);
        if (GUI.Button(deleteRect, "\u00D7", LogicEngineEditor.windowStyle_AddButtonSmall))
            toDelete.Add(statBonus);

        // Draw the label.
        string label = statBonus.stat.Label();
        if (statBonus.stat.ShowAsPercent(statBonus.modifier.IsPercentage)) label += " %";
        GUI.Label(new Rect(rect.x + 5, rect.y, 140, rect.height), label);

        // Draw the minimum and maximum inputs.
        float mod = statBonus.stat.DisplayModifier(statBonus.modifier.IsPercentage);
        Rect maxRect = new Rect(rect.x + rect.width - 53, rect.y, 30, rect.height);

        EditorGUI.BeginChangeCheck();
        float previous = statBonus.modifier.Value;
        statBonus.modifier.Value = EditorGUI.FloatField(maxRect, statBonus.modifier.Value * mod, LogicEngineEditor.windowStyle_TextField) / mod;
        if (EditorGUI.EndChangeCheck())
        {
            EditorUtility.SetDirty(buff);
        }
    }

    /// <summary>
    /// Build a context menu which shows all possible stats that can be added to the item.
    /// If the item already has the stat, it cannot be added again.
    /// </summary>
    private GenericMenu BuildAddStatMenu(List<Buff.BuffStatBonus> statBonus)
    {
        GenericMenu menu = new GenericMenu();
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (stat.IsBasicStat(false) && !statBonus.Exists(x => x.stat == stat && !x.modifier.IsPercentage))
            {
                menu.AddItem(new GUIContent(stat.Label() + (stat.ShowAsPercent(false) ? " (%)" : "")), false, () =>
                {
                    statBonus.Add(new Buff.BuffStatBonus(stat, 0, false));
                    EditorUtility.SetDirty(buff);
                });
            }
            if (stat.IsBasicStat(true) && !statBonus.Exists(x => x.stat == stat && x.modifier.IsPercentage))
            {
                menu.AddItem(new GUIContent(stat.Label() + (stat.ShowAsPercent(true) ? " (%)" : "")), false, () =>
                {
                    statBonus.Add(new Buff.BuffStatBonus(stat, 0, true));
                    EditorUtility.SetDirty(buff);
                });
            }
        }
        return menu;
    }

    private static void BuildVisualEffectList ()
    {
        if (effectNames != null && effectNames.Count > 0) return;
        effects = new List<CustomVisualEffect>();
        effectNames = new List<string>();

        effects.Add(null);
        effectNames.Add("None");
        string[] interactionGUIDs = AssetDatabase.FindAssets("t:prefab", new[] { "Assets" });
        foreach (string guid in interactionGUIDs)
        {
            GameObject asset = AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as GameObject;
            CustomVisualEffect component = asset.GetComponent<CustomVisualEffect>();
            if (component != null && !component.isTemplate)
            {
                effects.Add(component);
                effectNames.Add(component.subGroup + "/" + component.gameObject.name);
            }
        }
        optionStrings = effectNames.ToArray();
        optionValues = effects.ToArray();
    }

    protected override void DrawItemInspector(Rect rect, IVisualCodeHandler item)
    {
        base.DrawItemInspector(rect, item);

        if (buff == null) return;

        float boxPadding = 5;
        Color boxColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        float minorSpacer = 5;
        float majorSpacer = 10;
        float width = rect.width - majorSpacer * 2;
        float posY = rect.y;
        float posX = rect.x + majorSpacer;
        float iconSize = 70;
        float shortWidth = width - iconSize - majorSpacer;
        float shortStart = posX + iconSize + majorSpacer;
        float height = 0;

        // Draw the background panel.
        EditorGUI.DrawRect(rect, panelColor);

        // Draw the header.
        EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, headerHeight), headerColor);
        EditorGUI.LabelField(new Rect(rect.x + 8, posY, rect.width, headerHeight), buff.name, LogicEngineEditor.windowStyle_HeaderText);
        posY += headerHeight + majorSpacer;

        // UI Control: Draw the icon for the item.
        buff.buffIcon = (Sprite)EditorGUI.ObjectField(new Rect(posX, posY, iconSize, iconSize), buff.buffIcon, typeof(Sprite), false);

        // UI Control: Specify the slot of the item.
        GUI.Label(new Rect(shortStart, posY, 100, height = 16), "Buff Type", EditorStyles.boldLabel);
        posY += height;
        buff.buffType = (Buff.BuffType)EditorGUI.EnumPopup(new Rect(shortStart, posY, shortWidth, height = 20), buff.buffType);
        posY += height;

        // UI Control: Specify the rarity of the item. 
        
        posY += height;
        BuildVisualEffectList();
        //buff.visualEffect = (CustomVisualEffect)EditorGUI.ObjectField(new Rect(shortStart, posY, shortWidth, height = 20), buff.visualEffect, typeof(CustomVisualEffect), false);
        //item.itemRarity = (Item.ItemRarity)EditorGUI.EnumPopup(new Rect(shortStart, posY, shortWidth, height = 20), item.itemRarity);

        

        //posY += height;

        // UI Control: Item Set Dropdown
        //GUI.Label(new Rect(posX, posY, width, height = 20), "Item Set", EditorStyles.boldLabel);
        posY += height;
        

        // UI Control: Specify the tooltip of the item.
        GUI.Label(new Rect(posX, posY, width, height = 20), "Buff Tooltip", EditorStyles.boldLabel);
        posY += height;
        buff.buffDescription = GUI.TextArea(new Rect(posX, posY, width, height = 50), buff.buffDescription);
        posY += height;

        // Specify the flavour text of the item.
        GUI.Label(new Rect(posX, posY, width, height = 20), "Buff Flavour Text", EditorStyles.boldLabel);
        posY += height;
        buff.buffFlavourText = GUI.TextArea(new Rect(posX, posY, width, height = 50), buff.buffFlavourText);
        posY += height;

        GUI.Label(new Rect(posX, posY, width, height = 20), "Visual Effect", EditorStyles.boldLabel);
        posY += height;
        if (optionStrings != null)
        {
            int index = Mathf.Max(0, Array.IndexOf(optionValues, buff.visualEffect));
            EditorGUI.BeginChangeCheck();
            int newID = EditorGUI.Popup(new Rect(posX, posY, width, height = 20), index, optionStrings);
            buff.visualEffect = optionValues[newID];
        }
        posY += height;

        GUI.Label(new Rect(posX, posY, width, height = 20), "Effect Attach Point", EditorStyles.boldLabel);
        posY += height;
        if (buff.visualEffect == null) GUI.enabled = false;
        if (optionStrings != null)
        {
            int index = Mathf.Max(0, Array.IndexOf(StringArg.GetOptions(PresetChoices.AttachPoints), buff.buffAttachPoint));
            EditorGUI.BeginChangeCheck();
            int newID = EditorGUI.Popup(new Rect(posX, posY, width, height = 20), index, StringArg.GetOptions(PresetChoices.AttachPoints));
            buff.buffAttachPoint = StringArg.GetOptions(PresetChoices.AttachPoints)[newID];
        }
        GUI.enabled = true;
        posY += height;

        posY += 5;
        EditorGUI.DrawRect(new Rect(posX, posY, width, 149), boxColor);
        posY += boxPadding;
        buff.buffRemovedOnDeath = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), buff.buffRemovedOnDeath, " Buff Removed on Death");
        posY += 20;
        buff.buffVisibleInUI = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), buff.buffVisibleInUI, " Buff Visible in UI");
        posY += 20;
        buff.addingStacksRefreshesDuration = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), buff.addingStacksRefreshesDuration, " Stacks Refresh Duration");
        posY += 20;
        buff.buffHasDuration = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), buff.buffHasDuration, " Buff Has Duration");
        posY += 30;
        //EditorGUI.DrawRect(new Rect(posX, posY, width, 94), boxColor);

        EditorGUI.DrawRect(new Rect(posX, posY, width, 1), panelColor);
        posY += 7;


        GUI.Label(new Rect(posX + boxPadding, posY, width, height = 20), "Duration");
        if (buff.buffHasDuration)
        {
            buff.buffMaxDuration = EditorGUI.FloatField(new Rect(posX + 100, posY, width - 100 - boxPadding, height = 20), buff.buffMaxDuration);
        }
        else
        {
            GUI.enabled = false;
            EditorGUI.TextField(new Rect(posX + 100, posY, width - 100 - boxPadding, height = 20), "None");
        }
        GUI.enabled = true;
        posY += height + 2;

        GUI.Label(new Rect(posX + boxPadding, posY, width, height), "Max Stacks");
        buff.buffMaximumStacks = EditorGUI.IntField(new Rect(posX + 100, posY, width - 100 - boxPadding, 20), buff.buffMaximumStacks);
        posY += height + 15;

        EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.1f, 0.1f, 0.1f));
        GUI.Label(new Rect(rect.x + minorSpacer, posY, 120, 20), "Buff Stat Bonuses", EditorStyles.boldLabel);
        //GUI.Label(new Rect(posX + 167, posY, 30, 20), "Max", LogicEngineEditor.windowStyle_SmallCenteredText);
        if (GUI.Button(new Rect(rect.x + rect.width - 21, posY - 1, 21, 21), "+", LogicEngineEditor.windowStyle_AddButtonSmall))
        {
            BuildAddStatMenu(buff.buffStatBonuses).ShowAsContext();
        }
        posY += 23;

        // Draw in the stat bonuses!

        foreach (Buff.BuffStatBonus buffStatBonus in buff.buffStatBonuses)
        {
            DrawStatBlock(buff, buffStatBonus, new Rect(rect.x, posY, rect.width, 22));
            posY += 23;
        }
        if (buff.buffStatBonuses.Count == 0)
        {
            EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.25f, 0.25f, 0.25f));
            GUI.Label(new Rect(rect.x, posY, rect.width, 22), "No Stat Modifiers", LogicEngineEditor.windowStyle_SmallCenteredText);
            posY += 23;
        }

        // Delete all stats marked for deletion.
        if (toDelete.Count > 0)
        {
            foreach (var statBlock in toDelete)
            {
                buff.buffStatBonuses.Remove(statBlock);
                EditorUtility.SetDirty(buff);
            }
            toDelete = new List<Buff.BuffStatBonus>(); 
        }
    }
}
