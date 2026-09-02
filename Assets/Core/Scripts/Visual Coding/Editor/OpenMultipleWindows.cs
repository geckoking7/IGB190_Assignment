using UnityEditor;
using UnityEngine;

/// <summary>
/// Simple control to open all logic engine windows together.
/// </summary>
public class OpenMultipleWindows : EditorWindow
{
    private const int width = 1500;
    private const int height = 800;

    [MenuItem("IGB190/Open Custom Windows")]
    public static void OpenAll()
    {
        // Open each window
        GetWindow<AbilityEditor>().Close();
        var window1 = GetWindow<AbilityEditor>("Ability Editor");
        float x = Screen.currentResolution.width / 2.0f - width / 2.0f;
        float y = Screen.currentResolution.height / 2.0f - height / 2.0f;
        window1.position = new UnityEngine.Rect(x, y, width, height);

        GetWindow<ItemEditor>().Close();
        var window2 = GetWindow<ItemEditor>("Item Editor", typeof(AbilityEditor));

        GetWindow<GeneralScriptEditor>().Close();
        var window3 = GetWindow<GeneralScriptEditor>("Gameplay Editor", typeof(ItemEditor));

        GetWindow<BuffEditor>().Close();
        var window4 = GetWindow<BuffEditor>("Buff Editor", typeof(GeneralScriptEditor));

        GetWindow<VisualCodeConsoleEditor>().Close();
        var window5 = GetWindow<VisualCodeConsoleEditor>("Visual Code Console", typeof(BuffEditor));

        window1.Focus();
    }
}