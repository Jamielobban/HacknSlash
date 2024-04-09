using UnityEngine;

[CreateAssetMenu(fileName = "Item Data", menuName = "Items/Data")]
public class ItemData : ScriptableObject
{
    public int id;
    public Enums.RarityType rarityType;
    public string itemName;
    public string itemDescription;
    public Sprite itemIcon;
    public float value;
}
