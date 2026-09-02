using UnityEngine;

public partial class VisualCodeScript
{
    public Buff ThisBuff()
    {
        return (Buff)LogicEngine.current.engineHandler;
    }
}