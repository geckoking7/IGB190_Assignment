using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEditor;
using UnityEngine;

public class VisualCodeConsoleListener : EditorWindow
{
    public static List<VisualCodeLogMessage> messages = new List<VisualCodeLogMessage>();

    [InitializeOnLoadMethod]
    private static void Init()
    {
        VisualCodeScript.OnLog -= HandleLog;
        VisualCodeScript.OnLog += HandleLog;
    }

    private static void HandleLog(VisualCodeLogMessage message)
    {
        message.errorLocation = $"[{LogicEngine.current.engineHandler.GetData().name.Replace("(Clone)", "")}] [{LogicEngine.currentScript.scriptName}] [{LogicEngine.currentType}, Line {LogicEngine.currentLine + 1}]";
        messages.Add(message);
    }
}
