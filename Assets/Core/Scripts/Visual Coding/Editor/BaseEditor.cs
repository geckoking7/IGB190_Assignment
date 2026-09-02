using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
using UnityEditor.Sprites;
using UnityEditorInternal.VersionControl;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;
using static VisualCodeLabels.Presets.Events;

public class BaseEditor : EditorWindow
{
    private Vector2 listScrollPosition = Vector2.zero;

    protected Color panelColor = new Color(0.3f, 0.3f, 0.3f);
    protected Color selectedColor = new Color(.17f, .36f, .53f);
    protected Color unselectedColor = new Color(0.15f, 0.15f, 0.15f);

    private const float listPanelWidth = 200;
    private const float detailsPanelWidth = 230;

    protected const float headerHeight = 25;
    private const float itemHeight = 30;
    private const float spacer = 5;
    private const float itemPadding = 1;
    protected const float iconSize = 70;
    protected const float boxPadding = 5;
    protected const float smallPadding = 2;

    protected const float labelHeight = 20;
    protected const float toggleHeight = 22;

    protected static Color headerColor = new Color(0.1f, 0.1f, 0.1f, 1);
    protected static Color boxColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);



    protected IVisualCodeHandler selectedItem;
    protected LogicEngineEditor selectedItemEditor;

    protected virtual string GetListHeaderText => "Abilities";
    protected virtual string ListItemFolder => "";
    protected virtual IVisualCodeHandler[] ListData => null;
    protected virtual Type ManagedType => null;
    protected virtual string GetStyledName (IVisualCodeHandler item) => item.GetName();

    protected virtual bool HasInspectorPanel() => true;

    protected virtual Texture2D GetDefaultIcon() => null;


    protected virtual void DrawList (Rect panel, string title, IVisualCodeHandler[] items)
    {
        float posX = panel.x;
        float posY = panel.y;
        float width = panel.width;

        // Draw the background panel color.
        EditorGUI.DrawRect(panel, panelColor);

        // Draw the list header (with the '+' create button).
        Rect area = new Rect(posX, posY, width, headerHeight);
        EditorGUI.DrawRect(area, new Color(0.1f, 0.1f, 0.1f, 1));
        EditorGUI.LabelField(area, title, LogicEngineEditor.windowStyle_HeaderText);
        if (GUI.Button(new Rect(area.xMax - headerHeight, area.y - 1, headerHeight,
            headerHeight), "+", LogicEngineEditor.windowStyle_AddButton))
        {
            CreateNewListItem();
        }
        posY += headerHeight;


        // Set up list scrolling.
        Rect scrollRect = new Rect(panel);
        scrollRect.y += headerHeight;
        scrollRect.height -= headerHeight;
        Rect requiredSize = new Rect(scrollRect);
        requiredSize.height = (itemHeight + itemPadding) * items.Length;
        listScrollPosition = GUI.BeginScrollView(scrollRect, listScrollPosition,
            requiredSize, false, false, GUIStyle.none, GUIStyle.none);

        // Draw all individual list items
        foreach (IVisualCodeHandler item in items)
        {
            Rect itemRect = new Rect(posX, posY, width, itemHeight);
            DrawListItem(itemRect, item);
            posY += itemHeight + itemPadding;
        }

        GUI.EndScrollView();
        Event current2 = Event.current;
        if (panel.Contains(current2.mousePosition) && current2.type == EventType.MouseDown)
            GUI.FocusControl(null);
    }

    protected virtual void DrawListItem(Rect rect, IVisualCodeHandler item)
    {
        // Use the selection color if the item is selected.
        if (selectedItemEditor != null && selectedItemEditor.engine == item.GetEngine())
            EditorGUI.DrawRect(rect, selectedColor);
        else
            EditorGUI.DrawRect(rect, unselectedColor);

        // Draw the item icon.
        Rect iconRect = new Rect(rect.x, rect.y, itemHeight, itemHeight);
        EditorGUI.DrawRect(iconRect, Color.black);
        if (item.GetIcon() != null)
            GUI.DrawTexture(iconRect, item.GetIcon());
        else if (GetDefaultIcon() != null)
            GUI.DrawTexture(iconRect, GetDefaultIcon());

        //Rect textRect = new Rect(iconRect.x + itemHeight + spacer, iconRect.y, iconRect.width - itemHeight - spacer, itemHeight);
        Rect textRect = new Rect(rect);
        textRect.x += itemHeight + spacer;
        textRect.width = textRect.width - itemHeight - spacer;
        if (EditorUtility.IsDirty(item.GetData()))
            GUI.Label(textRect, "*" + GetStyledName(item), LogicEngineEditor.windowStyle_BodyText);
        else
            GUI.Label(textRect, GetStyledName(item), LogicEngineEditor.windowStyle_BodyText);


        //GUI.Label(rect, item.GetName(), LogicEngineEditor.windowStyle_BodyText);

        Event current = Event.current;
        if (textRect.Contains(current.mousePosition))
        {
            if (current.type == EventType.MouseDown)
            {
                SetSelection(item);
            }
            if (current.type == EventType.ContextClick)
            {
                IVisualCodeHandler test = item;
                SetSelection(test);
                GenericMenu menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy"), false, () => { ListItem_Copy(test); });
                menu.AddItem(new GUIContent("Rename"), false, () => { ListItem_Rename(test); });
                menu.AddItem(new GUIContent("Delete"), false, () => { ListItem_Delete(test); });
                if (EditorUtility.IsDirty(test.GetData()))
                {
                    menu.AddItem(new GUIContent("Revert"), false, () => { ListItem_Revert(test); });
                }
                menu.ShowAsContext();
                current.Use();
            }
        }
    }





    protected virtual void DrawItemInspector (Rect panel, IVisualCodeHandler item) 
    {
        // Draw the background panel color.
        EditorGUI.DrawRect(panel, panelColor);


    }

    

    protected virtual void CreateNewListItem ()
    {
        GenericMenu menu = new GenericMenu();
        ScriptableObject[] items = Resources.LoadAll<ScriptableObject>(VisualCodeLabels.Folders.TEMPLATES);

        List<IVisualCodeHandler> filteredItems = new List<IVisualCodeHandler>();
        foreach (ScriptableObject item in items)
        {
            if (item.GetType() == ManagedType)
            {
                filteredItems.Add((IVisualCodeHandler)item);
            }
        }
        foreach (var item in filteredItems)
        {
            IVisualCodeHandler i = item;
            menu.AddItem(new GUIContent(i.GetName()), false, () => { ListItem_Copy(i); });
        }
        menu.ShowAsContext();
    }

    

    protected virtual void ListItem_Copy (IVisualCodeHandler listItem)
    {
        string itemName = "";
        itemName = EditorInputDialog.Show("Enter New Name", "", listItem.GetName());
        if (itemName == null || itemName.Length == 0) return;

        string uniquePath = AssetDatabase.GenerateUniqueAssetPath($"{VisualCodeLabels.Folders.RESOURCES}/{ListItemFolder}/{itemName}.asset");
        string uniqueName = System.IO.Path.GetFileNameWithoutExtension(uniquePath);
        IVisualCodeHandler copy = listItem.CopyGeneral(uniqueName);
        
        AssetDatabase.CreateAsset(copy.GetData(), uniquePath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        SetSelection(copy);
    }

    /*
    /// <summary>
    /// Create a copy of the given ability, saving it to file.
    /// </summary>
    public void CopyItem(Ability abilityToCopy)
    {
        string abilityName = "";
        abilityName = EditorInputDialog.Show("Enter New Name", "", abilityToCopy.name);
        if (abilityName != null && abilityName.Length > 0)
        {
            Ability copy = abilityToCopy.Copy();
            copy.name = abilityName;
            string path = AssetDatabase.GenerateUniqueAssetPath($"{VisualCodeLabels.Folders.RESOURCES}/{VisualCodeLabels.Folders.ABILITIES}/{abilityName}.asset");
            AssetDatabase.CreateAsset(copy, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            SetSelection(copy);
        }
    }
    */



    protected virtual void ListItem_Delete(IVisualCodeHandler visualCodeHandler)
    {
        AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(visualCodeHandler.GetData()));
        SetSelection(ListData[0]);
    }

    protected virtual void ListItem_Rename(IVisualCodeHandler listItem)
    {
        string itemName = "";
        itemName = EditorInputDialog.Show("Enter New Name", "", listItem.GetName());
        if (itemName == null || itemName.Length == 0 || itemName == listItem.GetName()) return;

        string path = AssetDatabase.GetAssetPath(listItem.GetData());
        string uniquePath = AssetDatabase.GenerateUniqueAssetPath($"{VisualCodeLabels.Folders.RESOURCES}/{ListItemFolder}/{itemName}.asset");
        string uniqueName = System.IO.Path.GetFileNameWithoutExtension(uniquePath);
        AssetDatabase.RenameAsset(path, uniqueName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        SetSelection(listItem);
    }

    protected virtual void ListItem_Revert(IVisualCodeHandler listItem)
    {
        /*
        Ability disc = EditorUtilityExtensions.LoadFromDisk<Ability>(AssetDatabase.GetAssetPath(ability));
        EditorUtility.CopySerialized(disc, ability);
        EditorUtility.ClearDirty(ability);
        */
    }

    public void SetSelection (IVisualCodeHandler selectedItem)
    {
        this.selectedItem = selectedItem;
        selectedItemEditor = new LogicEngineEditor(this, selectedItem.GetEngine(), selectedItem);
    }

    protected virtual void DrawInspector (Rect panel)
    {
        // Draw the background panel color.
        EditorGUI.DrawRect(panel, panelColor);


    }

    private void OnGUI()
    {
        const float spacer = 4;
        float posX = spacer;
        float posY = spacer;

        // Check if anoher 
        if (selectedItem == null || selectedItemEditor == null || selectedItem.GetData() == null)
        {
            selectedItem = ListData[0];
            if (selectedItem == null) return;
            selectedItemEditor = new LogicEngineEditor(this, selectedItem.GetEngine(), selectedItem);
        }

        // Create an undo record.
        Undo.RecordObject(selectedItem.GetData(), "Changed: " + selectedItem.GetName());

        // Draw the left-side list.
        Rect listRect = new Rect(posX, posY, listPanelWidth, position.height - 2 * spacer);
        DrawList(listRect, " " + GetListHeaderText, ListData);
        posX += listPanelWidth + spacer;

        // Draw the selected item inspector.
        if (HasInspectorPanel())
        {
            Rect inspectorRect = new Rect(posX, posY, detailsPanelWidth, position.height - 2 * spacer);
            DrawItemInspector(inspectorRect, selectedItem);
            posX += detailsPanelWidth + spacer;
        }

        // Draw the visual code content.
        selectedItemEditor.Process();
        selectedItemEditor.DrawNodes(new Rect(posX, posY, this.position.width - posX - spacer, this.position.height - 2 * spacer));

        // Check for changes.
        ScriptableObject disc = EditorUtilityExtensions.LoadFromDisk<ScriptableObject>(AssetDatabase.GetAssetPath(selectedItem.GetData()));
        if (EditorUtilityExtensions.ComputeHash(disc) != EditorUtilityExtensions.ComputeHash(selectedItem.GetData()))
            EditorUtility.SetDirty(selectedItem.GetData());
        else
            EditorUtility.ClearDirty(selectedItem.GetData());
    }

    /// <summary>
    /// Constantly repaint the inspector (OnInspectorUpdate only updates 10 times per second).
    /// </summary>
    private void OnInspectorUpdate()
    {
        if (this.IsFocused())
            Repaint();
    }
}
