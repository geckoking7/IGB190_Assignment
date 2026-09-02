
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


public class VisualCodeConsoleEditor : EditorWindow
{
    public static List<VisualCodeLogMessage> displayedConsoleItems = new List<VisualCodeLogMessage>();
    public VisualCodeLogMessage selectedConsoleItem;

    private static Color panelHeaderColor = new Color(0.1f, 0.1f, 0.1f, 1.0f);
    private static Color selectedColor = new Color(0.17f, 0.36f, 0.53f, 1f);
    //public static Color unselectedColor = new Color(0.15f, 0.15f, 0.15f);
    //public static Color unselectedColor2 = new Color(0.12f, 0.12f, 0.12f);

    public static Color unselectedColor = new Color(0.25f, 0.25f, 0.25f, 1.0f);
    public static Color unselectedColor2 = new Color(0.3f, 0.3f, 0.3f, 1.0f);


    private static Color bottomInfoPanelColor = new Color(0.3f, 0.3f, 0.3f, 1.0f);

    private double lastClickAt;

    Vector2 scrollPosition;
    public string searchText;

    static VisualCodeConsoleEditor() {
        VisualCodeScript.OnLog += AddToConsole;
    }

    private void DrawToolbar ()
    {
        // --- Begin Toolbar ---
        GUILayout.BeginHorizontal(EditorStyles.toolbar);

        // Left-aligned buttons
        if (GUILayout.Button("Clear", EditorStyles.toolbarButton))
            ClearConsoleItems();

        GUILayout.FlexibleSpace();

        GUILayout.EndHorizontal();
        // --- End Toolbar ---
    }

    /// <summary>
    /// Handle all drawing for the entire UI window.
    /// </summary>
    private void OnGUI ()
    {
        DrawToolbar();
        if (displayedConsoleItems == null) displayedConsoleItems = new List<VisualCodeLogMessage>();
        //if (displayedConsoleItems.Count < 20)
        //{
        //    displayedConsoleItems.Add(new ConsoleItem());
        //}

        //float posX = 0; 
        float posY = 21;
        float width = position.width;
        float itemHeight = 40;

        Rect requiredSize = new Rect(position);
        requiredSize.height = itemHeight * displayedConsoleItems.Count;

        Rect windowRect = new Rect(0, 21, position.width, position.height -21);

        scrollPosition = GUI.BeginScrollView(windowRect, scrollPosition, requiredSize, false, false, GUIStyle.none, GUI.skin.verticalScrollbar);

        

        

        int drawId = 0;
        foreach (VisualCodeLogMessage item in displayedConsoleItems)
        {
            Rect r = new Rect(position);
            r.height = itemHeight;
            r.y = position.y + itemHeight * drawId;
            //DrawConsoleItem(new Rect(posX, posY, width, itemHeight), item, drawId);
            DrawConsoleItem(r, item, drawId);
            posY += itemHeight;
            drawId++;
        }

        if (drawId == 0)
        {
            GUIStyle italicLabel = new GUIStyle(EditorStyles.label);
            italicLabel.fontStyle = FontStyle.Italic;

            Rect r = new Rect(position);
            r.x += 10;
            r.height = itemHeight;
            r.y = position.y + itemHeight * drawId;
            GUI.Label(r, "There are no messages to display.", italicLabel);
        }

        

        GUI.EndScrollView();

        Rect infoRect = new Rect(0, position.height - 200, position.width, 200);

        var myStyle = new GUIStyle(EditorStyles.label);
        myStyle.alignment = TextAnchor.UpperLeft;
        myStyle.richText = true;

        Rect rect = GUILayoutUtility.GetRect(new GUIContent("Error in MyScript.cs:42"), myStyle);
        //GUI.Label(rect, $"Error in <color=cyan>MyScript.cs:42</color>", myStyle);

        Vector2 textSize = EditorStyles.label.CalcSize(new GUIContent("Error in "));
        Rect linkRect = new Rect(infoRect.x, infoRect.y, textSize.x, rect.height); // approximate width

        EditorGUI.DrawRect(infoRect, bottomInfoPanelColor);

        
        if (selectedConsoleItem != null)
        {
            string file = GetFirstNodeScriptLine(selectedConsoleItem.detailedMessage);
            GUI.Label(infoRect, $"<color=yellow>Error: {selectedConsoleItem.message}</color> [<color=white>{file}</color>]\n" + selectedConsoleItem.detailedMessage, myStyle);
            //GUI.Label(infoRect, $"Error in <color=cyan>MyTest</color>", myStyle);
            if (Event.current.type == EventType.MouseDown && infoRect.Contains(Event.current.mousePosition))
            {
                int line = int.Parse(file.Split(':')[1]);
                string fileName = file.Split(':')[0];
                if (infoRect.Contains(Event.current.mousePosition))
                {
                    UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal($"Assets/Core/Scripts/Visual Coding/Nodes/{fileName}", line);
                    Event.current.Use();
                }
            }
        }

        // Simple hit test on the link substring
        

        
    }

    private static string GetFirstNodeScriptLine(string trace)
    {
        // Match lines like:
        // "at SomeClass.SomeMethod () [0x00000] in Assets/Core/Scripts/Visual Coding/Nodes/VisualCodeActions.cs:827"
        var match = Regex.Match(
            trace,
            //@"Assets/Core/Scripts/Visual Coding/Nodes/([^\\\/]+\.cs:\d+)"
            @"Assets\\Core\\Scripts\\Visual Coding\\Nodes\\([^\\\/]+\.cs:\d+)"
        );



        if (match.Success)
            return match.Groups[1].Value; // e.g. "VisualCodeActions.cs:827"

        return "No match found";
    }

    private void DrawConsoleItem (Rect rect, VisualCodeLogMessage item, int id)
    {
        if (item == selectedConsoleItem)
            EditorGUI.DrawRect(rect, selectedColor);
        else if (id % 2 == 0)
            EditorGUI.DrawRect(rect, unselectedColor);
        else
            EditorGUI.DrawRect(rect, unselectedColor2);

        Event current = Event.current;
        if (rect.Contains(current.mousePosition))
        {
            if (current.type == EventType.MouseDown)
            {
                
                Select(item);
                if (EditorApplication.timeSinceStartup - lastClickAt < 0.3f)
                {
                    Open(item);
                }
                lastClickAt = EditorApplication.timeSinceStartup;
            }
        }

        Rect labelRect = new Rect(rect);
        labelRect.x += 45;
        labelRect.y -= 8;

        GUIStyle richStyle = new GUIStyle(GUI.skin.label);
        richStyle.richText = true;

        GUI.Label(labelRect, $"<color=yellow>[{item.timestamp}] {item.message}</color>", richStyle);

        Rect labelRect2 = new Rect(rect);
        labelRect2.x += 45;
        labelRect2.y += 8;

        
        GUI.Label(labelRect2, item.errorLocation, richStyle);


        Rect iconRect = new Rect(rect);
        iconRect.width = 40;
        iconRect.height = 40;
        if (item.icon != null)
        {
            GUI.DrawTexture(iconRect, item.icon);
        }
    }

    private void ClearConsoleItems ()
    {
        displayedConsoleItems.Clear();
    }

    private void Select (VisualCodeLogMessage consoleItem)
    {
        selectedConsoleItem = consoleItem;
    }

    private void Open (VisualCodeLogMessage item)
    {
        if (item.target is Ability)
        {
            EditorWindow.FocusWindowIfItsOpen<AbilityEditor>();
            Ability ability = ((Ability)item.target).template;
            GetWindow<AbilityEditor>().SetSelection(ability);
            GeneralNode node = FindNodeToSelect(ability, item.script, item.node);
            ability.engine.SetSelection(item.script, node);
        }
        else if (item.target is Item)
        {
            EditorWindow.FocusWindowIfItsOpen<ItemEditor>();
            Item i = ((Item)item.target).template;
            GetWindow<ItemEditor>().SetSelection(i);
            GeneralNode node = FindNodeToSelect(i, item.script, item.node);
            i.engine.SetSelection(item.script, node);
        }
        else if (item.target is Buff)
        {
            EditorWindow.FocusWindowIfItsOpen<BuffEditor>();
            Buff buff = ((Buff)item.target);
            GetWindow<BuffEditor>().SetSelection(buff);
            GeneralNode node = FindNodeToSelect(buff, item.script, item.node);
            buff.engine.SetSelection(item.script, node);
        }
        else if (item.target is LogicContainer)
        {
            EditorWindow.FocusWindowIfItsOpen<GeneralScriptEditor>();
            LogicContainer container = ((LogicContainer)item.target);
            GetWindow<GeneralScriptEditor>().SetSelection(container);
            GeneralNode node = FindNodeToSelect(container, item.script, item.node);
            container.engine.SetSelection(item.script, node);
        }
    }

    private GeneralNode FindNodeToSelect (IVisualCodeHandler handler, VisualCodeScript script, GeneralNode node)
    {
        GeneralNode nodeToSelect = node;
        while (nodeToSelect.parentNode != null)
            nodeToSelect = nodeToSelect.parentNode;
        return nodeToSelect;
    }

    public static void AddToConsole (VisualCodeLogMessage message)
    {
        if (displayedConsoleItems == null)
            displayedConsoleItems = new List<VisualCodeLogMessage>();

        message.errorLocation = $"[{LogicEngine.current.engineHandler.GetData().name.Replace("(Clone)", "")}] [{LogicEngine.currentScript.scriptName}] [{LogicEngine.currentType}, Line {LogicEngine.currentLine + 1}]";

        displayedConsoleItems.Add(message);
        //GetWindow<VisualCodeConsoleEditor>().OnGUI();
    }

    private static Texture GetIconTexture (IVisualCodeHandler target)
    {
        if (target is Ability)
            return ((Ability)target).abilityIcon.texture;
        else if (target is Buff)
            return ((Buff)target).buffIcon.texture;
        if (target is Item)
            return ((Item)target).itemIcon.texture;
        return null;
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
#endif