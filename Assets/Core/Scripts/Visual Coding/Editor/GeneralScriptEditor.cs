using GluonGui.WorkspaceWindow.Views.WorkspaceExplorer;
using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Playables;
using UnityEngine;

public class GeneralScriptEditor : BaseEditor
{
    protected override string ListItemFolder => VisualCodeLabels.Folders.LOGIC_CONTAINERS;

    protected override IVisualCodeHandler[] ListData => Resources.LoadAll<LogicContainer>(ListItemFolder);

    protected LogicContainer selectedLogicBlock => selectedItem == null ? null : (LogicContainer)selectedItem.GetData();

    protected override Type ManagedType => typeof(LogicContainer);

    protected override bool HasInspectorPanel() => false;
    protected override string GetListHeaderText => "General Scripts";

    private Texture2D cachedIcon = null;
    protected override Texture2D GetDefaultIcon()
    {
        const string iconFolder = "Icons/Code Folder";
        if (cachedIcon == null)
        {
            Sprite tmp = Resources.Load<Sprite>(iconFolder);
            if (tmp != null) cachedIcon = tmp.texture;
        }
        return cachedIcon;
    }
}
