using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Items/Data")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public bool isStackable { get; set; }
    public int ID => id;
    [field: SerializeField] public int maxStackSize { get; set; } = 1;
    public int id;
    public Enums.RarityType rarityType;
    public string itemName;
    [TextArea]public string itemDescription;
    [TextArea]public string itemDefaultDescription;
    public Sprite itemIcon;
    public float value;
}
