using System;
using UnityEngine;

public class VisualCodeLogMessage
{
    public string message;
    public string errorLocation;


    public float time;
    public string name;
    public string nodeType;
    public int lineNumber;
    public string timestamp;

    public IVisualCodeHandler target;
    public VisualCodeScript script;
    public GeneralNode node;
    public Texture icon;


    public int count = 1;

    public string detailedMessage;

    public VisualCodeLogMessage(string message, string detailedMessage, IVisualCodeHandler target, VisualCodeScript script, GeneralNode node)
    {
        this.message = message;
        this.detailedMessage = detailedMessage;
        this.target = target;
        this.script = script;
        this.node = node;
        this.icon = GetIconTexture(target);
        this.timestamp = DateTime.Now.ToString("HH:mm:ss");
    }

    private static Texture GetIconTexture(IVisualCodeHandler target)
    {
        if (target is Ability)
            return ((Ability)target).abilityIcon.texture;
        else if (target is Buff)
            return ((Buff)target).buffIcon.texture;
        if (target is Item)
            return ((Item)target).itemIcon.texture;
        return null;
    }
}