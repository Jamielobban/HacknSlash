using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();

    [field: SerializeField] public int Size { get; private set; } = 20;

    public void Initialize()
    {
        for (int i = 0; i < Size; i++)
        {
            items.Add(InventoryItem.GetEmptyItem());
        }
    }

    public void AddItem(ItemData item, int quantity)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isEmpty)
            {
                items[i] = new InventoryItem
                {
                    item = item,
                    quantity = quantity
                };
            }
        }
    }

    public Dictionary<int, InventoryItem> GetCurrentInventoryState()
    {
        Dictionary<int, InventoryItem> returnValue = new Dictionary<int, InventoryItem>();

        for (int i = 0; i < items.Count; i++)
        {
            if(items[i].isEmpty)
                continue;
            returnValue[i] = items[i];
        }

        return returnValue;
    }

    public InventoryItem GetItemAt(int itemIndex)
    {
        return items[itemIndex];
    }
}

[System.Serializable]
public struct InventoryItem
{
    public int quantity;
    public ItemData item;
    public bool isEmpty => item == null;
    public InventoryItem ChangeQuantity(int newQuantity)
    {
        return new InventoryItem
        {
            item = this.item,
            quantity = newQuantity
        };
    }

    public static InventoryItem GetEmptyItem() => new InventoryItem { item = null, quantity = 0 };
}
