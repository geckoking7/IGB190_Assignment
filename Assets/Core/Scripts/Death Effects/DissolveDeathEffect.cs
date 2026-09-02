using Unity.VisualScripting;
using UnityEngine;

public class DissolveDeathEffect : MonoBehaviour, IDeathEffect
{
    private bool hasTriggered = false;
    public Material material;

    public void Trigger(Unit unit)
    {
        if (hasTriggered) return;
        hasTriggered = true;
        unit.animator.AddComponent<DeathMaterialEffect>().SetMaterial(material);
    }
}
