using UnityEngine;

[CreateAssetMenu(fileName = "Item Set", menuName = "Item Set")]
public class ItemSet : ScriptableObject
{
    public ItemSetBonus[] setBonuses;

    [System.Serializable]
    public class ItemSetBonus
    {
        public int requiredItemCount;
        public string description;
    }
}
