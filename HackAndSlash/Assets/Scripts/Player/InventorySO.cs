using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu]
public class InventorySO : ScriptableObject
{
    [SerializeField] private List<InventoryItem> items = new List<InventoryItem>();
    [field: SerializeField] public int Size { get; private set; } = 20;
    public event Action<Dictionary<int, InventoryItem>> OnInventoryUpdated;
    public void Initialize()
    {
        if (items.Count != Size)
        {
            for (int i = 0; i < Size; i++)
            {
                items.Add(InventoryItem.GetEmptyItem());
            }
        }
        else // Clear All Items Picked (?)
        {
            for (int i = 0; i < items.Count; i++)
            {
                items[i] = InventoryItem.GetEmptyItem();
            }
        }
    }

    public int AddItem(ItemData item, int quantity)
    {
        if (!item.isStackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                while (quantity > 0 && !IsInventoryFull())
                {
                    quantity -= AddItemToFirstFreeSlot(item, 1);
                }
                InformAboutChange();
                return quantity;
            }
        }
        quantity = AddStackableItem(item, quantity);
        InformAboutChange();
        return quantity;
    }

    private int AddItemToFirstFreeSlot(ItemData item, int quantity)
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item,
            quantity = quantity
        };
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isEmpty)
            {
                items[i] = newItem;
                return quantity;
            }
        }

        return 0;
    }

    private bool IsInventoryFull() => items.Where(item => item.isEmpty).Any() == false;

    private int AddStackableItem(ItemData item, int quantity)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].isEmpty)
                continue;
            if(items[i].item.ID == item.ID)
            {
                int amountPossibleToTake = items[i].item.maxStackSize - items[i].quantity;

                if (quantity > amountPossibleToTake)
                {
                    items[i] = items[i].ChangeQuantity(items[i].item.maxStackSize);
                    quantity -= amountPossibleToTake;
                }
                else
                {
                    items[i] = items[i].ChangeQuantity(items[i].quantity + quantity);
                    InformAboutChange();
                    return 0;
                }
            }
        }
        while(quantity > 0 && IsInventoryFull() == false)
        {
            int newQuantity = Mathf.Clamp(quantity, 0, item.maxStackSize);
            quantity -= newQuantity;
            AddItemToFirstFreeSlot(item, newQuantity);
        }
        return quantity;
    }

    private void InformAboutChange() => OnInventoryUpdated?.Invoke(GetCurrentInventoryState());
    
    public void RemoveItem(int itemIndex, int amount)
    {
        if (items.Count > itemIndex)
        {
            if (items[itemIndex].isEmpty)
                return;
            int reminder = items[itemIndex].quantity - amount;
            if (reminder <= 0)
                items[itemIndex] = InventoryItem.GetEmptyItem();
            else
                items[itemIndex] = items[itemIndex]
                    .ChangeQuantity(reminder);

            InformAboutChange();
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

    public InventoryItem GetItemAt(int itemIndex) => items[itemIndex];
    public void AddItem(InventoryItem item) => AddItem(item.item, item.quantity);
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
