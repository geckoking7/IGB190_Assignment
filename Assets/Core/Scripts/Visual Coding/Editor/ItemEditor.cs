using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;
using ColorUtility = UnityEngine.ColorUtility;

public class ItemEditor : BaseEditor
{
    private static List<Item.StatBlock> toDelete = new List<Item.StatBlock>();

    protected override string ListItemFolder => VisualCodeLabels.Folders.ITEMS;

    protected override IVisualCodeHandler[] ListData {
        get
        {
            Item[] items = Resources.LoadAll<Item>(ListItemFolder);
            items = items.OrderBy(e => -(int)e.itemRarity).ThenBy(e => e.name).ToArray();
            return items;
        }
    }

    protected Item item => selectedItem == null ? null : (Item)selectedItem.GetData();

    protected override Type ManagedType => typeof(Item);
    protected override string GetListHeaderText => "Items";
    protected override string GetStyledName(IVisualCodeHandler item)
    {
        if (item == null || item is not Item) return "";
        Item i = (Item)item;
        return SetStringColor(i.name, i.GetItemColor());
    }

    /// <summary>
    /// Build a richtext string, turning the text into the specified color.
    /// e.g. "My String" and Red would become "<color=#FF0000>My String</color>".
    /// </summary>
    private string SetStringColor (string text, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{text}</color>";
    }

    /// <summary>
    /// Build a context menu which shows all possible stats that can be added to the item.
    /// If the item already has the stat, it cannot be added again.
    /// </summary>
    private GenericMenu BuildAddStatMenu (List<Item.StatBlock> statBlock)
    {
        GenericMenu menu = new GenericMenu();
        foreach (Stat stat in Enum.GetValues(typeof(Stat)))
        {
            if (stat.IsBasicStat(false) && !statBlock.Exists(x => x.stat == stat && !x.isPercent))
            {
                menu.AddItem(new GUIContent(stat.Label() + (stat.ShowAsPercent(false) ? " (%)" : "")), false, () =>
                {
                    statBlock.Add(new Item.StatBlock(stat, false));
                    EditorUtility.SetDirty(item);
                });
            }
            if (stat.IsBasicStat(true) && !statBlock.Exists(x => x.stat == stat && x.isPercent))
            {
                menu.AddItem(new GUIContent(stat.Label() + (stat.ShowAsPercent(true) ? " (%)" : "")), false, () =>
                {
                    statBlock.Add(new Item.StatBlock(stat, true));
                    EditorUtility.SetDirty(item);
                });
            }
        }
        return menu;
    }

    protected override void DrawItemInspector(Rect rect, IVisualCodeHandler itemDrawn)
    {
        base.DrawItemInspector(rect, itemDrawn);

        if (item == null) return;

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
        EditorGUI.LabelField(new Rect(rect.x + 8, posY, rect.width, headerHeight), GetStyledName(item), LogicEngineEditor.windowStyle_HeaderText);
        posY += headerHeight + majorSpacer;

        // UI Control: Draw the icon for the item.
        item.itemIcon = (Sprite)EditorGUI.ObjectField(new Rect(posX, posY, iconSize, iconSize), item.itemIcon, typeof(Sprite), false);

        // UI Control: Specify the slot of the item.
        GUI.Label(new Rect(shortStart, posY, 100, height = 16), VisualCodeLabels.Editor.ITEM_ITEM_SLOT_CONTENT, EditorStyles.boldLabel);
        posY += height;
        item.itemType = (Item.ItemType)EditorGUI.EnumPopup(new Rect(shortStart, posY, shortWidth, height = 20), item.itemType);
        posY += height;

        // UI Control: Specify the rarity of the item. 
        GUI.Label(new Rect(shortStart, posY, 100, height = 16), VisualCodeLabels.Editor.ITEM_ITEM_RARITY_CONTENT, EditorStyles.boldLabel);
        posY += height;
        item.itemRarity = (Item.ItemRarity)EditorGUI.EnumPopup(new Rect(shortStart, posY, shortWidth, height = 20), item.itemRarity);
        posY += height;

        // UI Control: Item Set Dropdown
        GUI.Label(new Rect(posX, posY, width, height = 20), VisualCodeLabels.Editor.ITEM_ITEM_SET_CONTENT, EditorStyles.boldLabel);
        posY += height;

        List<ItemSet> itemSets = EditorUtilityExtensions.FindAllAssetsOfType<ItemSet>();
        itemSets.Insert(0, null);
        int selectedId = 0;

        List<string> itemSetNames = new List<string>();
        itemSetNames.Add("Item Not in Set");
        for (int i = 1; i < itemSets.Count; i++)
        {
            itemSetNames.Add(itemSets[i].name);
            if (item.itemSet == itemSets[i])
            {
                selectedId = i;
            }
        }

        int newId = EditorGUI.Popup(new Rect(posX, posY, width, height = 20), selectedId, itemSetNames.ToArray());
        item.itemSet = itemSets[newId];
        posY += height;


        // UI Control: Specify the tooltip of the item.
        GUI.Label(new Rect(posX, posY, width, height = 20), VisualCodeLabels.Editor.ITEM_TOOLTIP_CONTENT, EditorStyles.boldLabel);
        posY += height;
        item.itemDescription = GUI.TextArea(new Rect(posX, posY, width, height = 50), item.itemDescription);
        posY += height;

        // Specify the flavour text of the item.
        GUI.Label(new Rect(posX, posY, width, height = 20), VisualCodeLabels.Editor.ITEM_FLAVOUR_TEXT_CONTENT, EditorStyles.boldLabel);
        posY += height;
        item.itemFlavourText = GUI.TextArea(new Rect(posX, posY, width, height = 30), item.itemFlavourText);
        posY += height;







        posY += 5;
        GUI.Label(new Rect(posX, posY, width, 20), VisualCodeLabels.Editor.ITEM_TAG_CONTENT, EditorStyles.boldLabel);
        item.itemTag = GUI.TextField(new Rect(posX + 120, posY, width - 120, 20), item.itemTag);
        posY += 24;
        GUI.Label(new Rect(posX, posY, width, 20), VisualCodeLabels.Editor.ITEM_CLASS_REQUIRED_CONTENT, EditorStyles.boldLabel);
        item.classRequirement = GUI.TextField(new Rect(posX + 120, posY, width - 120, 20), item.classRequirement);
        posY += 24;
        GUI.Label(new Rect(posX, posY, width, 20), VisualCodeLabels.Editor.ITEM_MIN_DROP_LEVEL_CONTENT, EditorStyles.boldLabel);
        item.minimumDropLevel = EditorGUI.IntField(new Rect(posX + 120, posY, width - 120, 20), item.minimumDropLevel);
        posY += 24;






        float boxPadding = 5;
        Color boxColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
        posY += boxPadding;


        EditorGUI.DrawRect(new Rect(posX, posY, width, 50), boxColor);

        posY += boxPadding;
        item.canPurchaseInShop = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), item.canPurchaseInShop, VisualCodeLabels.Editor.ITEM_CAN_PURCHASE_CONTENT);
        posY += 20;
        item.canDropOffMonster = GUI.Toggle(new Rect(posX + boxPadding, posY, width, 22), item.canDropOffMonster, VisualCodeLabels.Editor.ITEM_CAN_DROP_CONTENT);
        posY += 34;

        // UI Control: Specify the number of random stats the item should roll.
        GUI.Label(new Rect(posX, posY, 100, height = 20), VisualCodeLabels.Editor.ITEM_RANDOM_STATS_CONTENT, EditorStyles.boldLabel);
        item.randomStatCount = EditorGUI.IntField(new Rect(posX, posY + height, 100, height), item.randomStatCount);

        // UI Control: Specify the purchase cost of the item.
        GUI.Label(new Rect(posX + 110, posY, 100, height), VisualCodeLabels.Editor.ITEM_PURCHASE_COST_CONTENT, EditorStyles.boldLabel);
        item.itemCost = EditorGUI.IntField(new Rect(posX + 110, posY + height, 100, height), item.itemCost);
        posY += 2 * height + minorSpacer;

        // UI Control: Draw the header for the guaranteed stats.
        EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.1f, 0.1f, 0.1f));
        GUI.Label(new Rect(rect.x + minorSpacer, posY, 120, 20), "Guaranteed Stats", EditorStyles.boldLabel);
        if (item.guaranteedStats.Count > 0) GUI.Label(new Rect(posX + 136, posY, 30, 20), "Min", LogicEngineEditor.windowStyle_SmallCenteredText);
        if (item.guaranteedStats.Count > 0) GUI.Label(new Rect(posX + 167, posY, 30, 20), "Max", LogicEngineEditor.windowStyle_SmallCenteredText);
        if (GUI.Button(new Rect(rect.x + rect.width - 21, posY - 1, 21, 21), "+", LogicEngineEditor.windowStyle_AddButtonSmall))
        {
            BuildAddStatMenu(item.guaranteedStats).ShowAsContext();
        }
        posY += 23;

        // UI Control: Draw each guaranteed stat.
        foreach (Item.StatBlock statBlock in item.guaranteedStats)
        {
            DrawStatBlock(item, statBlock, new Rect(rect.x, posY, rect.width, 22));
            posY += 23;
        }
        if (item.guaranteedStats.Count == 0)
        {
            EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.25f, 0.25f, 0.25f));
            GUI.Label(new Rect(rect.x, posY, rect.width, 22), "No Guaranteed Stats", LogicEngineEditor.windowStyle_SmallCenteredText);
            posY += 23;
        }
        posY += minorSpacer;

        // UI Control: Draw the header for the random stats.
        EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.1f, 0.1f, 0.1f));
        GUI.Label(new Rect(rect.x + minorSpacer, posY, 150, 20), "Randomisable Stats", EditorStyles.boldLabel);
        if (item.randomisableStats.Count > 0) GUI.Label(new Rect(posX + 136, posY, 30, 20), "Min", LogicEngineEditor.windowStyle_SmallCenteredText);
        if (item.randomisableStats.Count > 0) GUI.Label(new Rect(posX + 167, posY, 30, 20), "Max", LogicEngineEditor.windowStyle_SmallCenteredText);
        if (GUI.Button(new Rect(rect.x + rect.width - 21, posY - 1, 21, 21), "+", LogicEngineEditor.windowStyle_AddButtonSmall))
        {
            BuildAddStatMenu(item.randomisableStats).ShowAsContext();
        }
        posY += 23;

        // UI Control: Draw each randomisable stat.
        foreach (Item.StatBlock statBlock in item.randomisableStats)
        {
            DrawStatBlock(item, statBlock, new Rect(rect.x, posY, rect.width, 22));
            posY += 23;
        }
        if (item.randomisableStats.Count == 0)
        {
            EditorGUI.DrawRect(new Rect(rect.x, posY, rect.width, 22), new Color(0.25f, 0.25f, 0.25f));
            GUI.Label(new Rect(rect.x, posY, rect.width, 22), "No Random Stats", LogicEngineEditor.windowStyle_SmallCenteredText);
            posY += 23;
        }

        // Delete all stats marked for deletion.
        if (toDelete.Count > 0)
        {
            foreach (Item.StatBlock statBlock in toDelete)
            {
                item.guaranteedStats.Remove(statBlock);
                item.randomisableStats.Remove(statBlock);
                EditorUtility.SetDirty(item);
            }
            toDelete = new List<Item.StatBlock>();
        }
    }

    /// <summary>
    /// Draw a single item stat block for the item.
    /// </summary>
    private void DrawStatBlock (Item item, Item.StatBlock statBlock, Rect rect)
    {
        EditorGUI.DrawRect(rect, new Color(0.25f, 0.25f, 0.25f));

        // Draw the delete button.
        Rect deleteRect = new Rect(rect.x + rect.width - 21, rect.y - 1, rect.height - 1, rect.height - 1);
        if (GUI.Button(deleteRect, "\u00D7", LogicEngineEditor.windowStyle_AddButtonSmall))
            toDelete.Add(statBlock);

        // Draw the label.
        string label = statBlock.stat.Label();
        if (statBlock.stat.ShowAsPercent(statBlock.isPercent)) label += " %";
        GUI.Label(new Rect(rect.x + 5, rect.y, 140, rect.height), label);

        // Draw the minimum and maximum inputs.
        float mod = statBlock.stat.DisplayModifier(statBlock.isPercent);
        Rect minRect = new Rect(rect.x + rect.width - 85, rect.y, 30, rect.height);
        Rect maxRect = new Rect(rect.x + rect.width - 53, rect.y, 30, rect.height);

        float previousMinimum = statBlock.minimum;
        float previousMaximum = statBlock.maximum;
        statBlock.minimum = EditorGUI.FloatField(minRect, statBlock.minimum * mod, LogicEngineEditor.windowStyle_TextField) / mod;
        statBlock.maximum = EditorGUI.FloatField(maxRect, statBlock.maximum * mod, LogicEngineEditor.windowStyle_TextField) / mod;

        if (previousMinimum != statBlock.minimum) EditorUtility.SetDirty(item);
        if (previousMaximum != statBlock.maximum) EditorUtility.SetDirty(item);
    }
}