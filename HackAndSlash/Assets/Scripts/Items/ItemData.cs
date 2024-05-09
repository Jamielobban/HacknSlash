using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Items/Data")]
public class ItemData : ScriptableObject
{
    [field: SerializeField] public bool isStackable { get; set; }
    [field: SerializeField] public int maxStackSize { get; set; } = 1;
    public int ID => id;
    public int id;
    public Enums.RarityType rarityType;
    [Header("Information: ")]
    public string itemName;
    public string itemType;
    public Color typeColor;
    public Sprite itemIcon;
    [TextArea] public string itemDescription;
    [TextArea] public string itemDefaultDescription;
    public float value;

    public Enums.AbilityInput �nput; // Only For Abilities
    public Sprite inputIcon;
    public Sprite inputXbox;
    public string extraInput; 
}
