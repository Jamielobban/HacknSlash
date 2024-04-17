using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Items/Data")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public bool isStackable { get; set; }
    public int ID => GetInstanceID();
    [field: SerializeField] public int maxStackSize { get; set; } = 1;
    
    public Enums.RarityType rarityType;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public float value;
}
