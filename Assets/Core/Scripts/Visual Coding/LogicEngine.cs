using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The LogicEngine class stores all of the visual scripting logic for a specific resource (e.g., ability, item).
/// A LogicEngine can contain any number of associated scripts, and contains its own local variables.
/// This allows for any amount of arbitrary logic to be attached to a resource.
/// </summary>
[System.Serializable]
public class LogicEngine
{
    public static Dictionary<string, GeneralNode> nodeTemplates = new Dictionary<string, GeneralNode>();
    public static Dictionary<string, GeneralNode> nodePresets = new Dictionary<string, GeneralNode>();

    // All of the scripts managed by the logic engine.
    public List<VisualCodeScript> scripts = new List<VisualCodeScript>();

    // Dictionaries storing variables for the engine. Each engine has its own local variables, and all engines share a global one.
    public Dictionary<string, object> localVariables = new Dictionary<string, object>();
    public static Dictionary<string, object> globalVariables = new Dictionary<string, object>();

    // The dictionary stores when scripts are disabled until. Disabled scripts will not run.
    public Dictionary<VisualCodeScript, float> disabledScripts = new Dictionary<VisualCodeScript, float>();


    
    public static LogicEngine current;
    public static VisualCodeScript currentScript;
    public static GeneralNode currentNode;
    public static int currentLine;
    public static string currentType;
    public static Dictionary<string, object> currentPresets;


    public static bool pausedExecution;

    public IVisualCodeHandler engineHandler;

    // Keeps track of selections. This isn't needed in gameplay and is just used for editor UI.
    // This should be moved such that it only exists in editor.
    [System.NonSerialized] public VisualCodeScript selectedScript = null;
    [System.NonSerialized] public List<GeneralNode> selectedNodes = new List<GeneralNode>();
    [System.NonSerialized] public List<VisualCodeTimer> activeTimers = new List<VisualCodeTimer>();

    public void SetSelection(VisualCodeScript script, GeneralNode node)
    {
        selectedScript = script;
        selectedNodes.Clear();
        selectedNodes.Add(node);
    }

    public static bool GlobalVariableExists (string variableName)
    {
        variableName = variableName.ToUpper();
        return globalVariables.ContainsKey (variableName);
    }

    public bool LocalVariableExists (string variableName)
    {
        variableName = variableName.ToUpper();
        return localVariables.ContainsKey(variableName); 
    }

    public static bool LocalVariableExists (LogicEngine engine, string variableName)
    {
        variableName = variableName.ToUpper();
        return engine.LocalVariableExists (variableName);
    }

    public static void SetLocalVariable(LogicEngine engine, string variableName, object value)
    {
        variableName = variableName.ToUpper();
        engine.SetLocalVariable(variableName, value);
    }

    public void SetLocalVariable(string variableName, object value)
    {
        variableName = variableName.ToUpper();
        localVariables[variableName] = value;
    }

    public static void SetGlobalVariable(string variableName, object value)
    {
        variableName = variableName.ToUpper();
        globalVariables[variableName] = value;
    }

    public static T GetGlobalVariable<T>(string variableName)
    {
        variableName = variableName.ToUpper();
        if (!globalVariables.ContainsKey(variableName))
        {
            globalVariables.Add(variableName, default(T));
        }
        return (T)globalVariables[variableName];
    }

    public static T GetLocalVariable<T>(LogicEngine engine, string variableName)
    {
        variableName = variableName.ToUpper();
        return engine.GetLocalVariable<T>(variableName);
    }

    public T GetLocalVariable<T>(string variableName)
    {
        variableName = variableName.ToUpper();
        if (!localVariables.ContainsKey(variableName))
        {
            localVariables.Add(variableName, default(T));
        }
        return (T)localVariables[variableName];
    }

    /// <summary>
    /// Create a shallow copy of the engine, with the same script objects.
    /// This is helpful for creating unique engines with the same logic (e.g.
    /// during gameplay).
    /// </summary>
    public LogicEngine ShallowCopy (IVisualCodeHandler engineHandler)
    {
        LogicEngine engine = new LogicEngine();
        engine.engineHandler = engineHandler;
        engine.scripts = scripts;
        return engine;
    }

    /// <summary>
    /// Create a deep copy of the engine, with deep copies of the same scripts,
    /// so they can be modified without affecting the original in any way.
    /// </summary>
    /// <returns></returns>
    public LogicEngine Copy ()
    {
        LogicEngine engine = new LogicEngine();
        foreach (VisualCodeScript script in scripts)
            engine.scripts.Add(script.Copy());
        return engine;
    }

    /// <summary>
    /// Trigger the specified event with the given presets.
    /// </summary>
    public void TriggerEvent(Dictionary<string, object> presets, string eventName)
    {
        if (presets == null) presets = new Dictionary<string, object>();
        foreach (VisualCodeScript script in scripts)
        {
            script.RunScript(presets, this, eventName);
        }
    }

    /// <summary>
    /// Setup the engine, performing any initial setup actions and creating
    /// all required timers.
    /// </summary>
    public void Setup ()
    {
        foreach (VisualCodeScript script in scripts)
        {
            foreach (GeneralNode node in script.eventNodes)
            {
                if (node.functionName == VisualCodeLabels.Events.EVENT_TIMER_CONTINUOUS_FINISHED)
                {
                    activeTimers.Add(new VisualCodeTimer(script, node, false));
                }
                else if (node.functionName == VisualCodeLabels.Events.EVENT_TIMER_ONE_OFF_FINISHED)
                {
                    activeTimers.Add(new VisualCodeTimer(script, node, true));
                }
            }
        }
    }

    public void DisableTimers ()
    {
        activeTimers = new List<VisualCodeTimer>();
    }

    public Unit GetOwner ()
    {
        if (engineHandler == null)
            return null;

        if (engineHandler is LogicContainer)
        {
            return GameManager.player;
        }
        Unit unit = engineHandler.GetOwner();
        return unit;
    }
} 
 