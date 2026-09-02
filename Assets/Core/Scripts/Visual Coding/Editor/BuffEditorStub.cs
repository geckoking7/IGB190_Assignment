using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Buff))]
public class BuffEditorStub : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Edit Buff"))
        {
            BuffEditor window = GetExistingWindow();
            if (window == null)
            {
                OpenMultipleWindows.OpenAll();
                window = GetExistingWindow();
            }
            window.SetSelection((Buff)target);
            window.Focus();
        }
    }

    public static BuffEditor GetExistingWindow()
    {
        BuffEditor[] windows = Resources.FindObjectsOfTypeAll<BuffEditor>();
        return windows.Length > 0 ? windows[0] : null;
    }
}
