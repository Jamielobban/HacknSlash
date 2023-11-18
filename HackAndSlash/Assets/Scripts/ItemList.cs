using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemList
{
    public Item item;
    public string name;
    public int stacks;
    public Sprite itemImage;
    public string itemDescription;
    public StatType statType;

    public ItemList(Item newItem, string newName, int newStacks, Sprite newImage, string newDescription, StatType newStatType)
    {
        item = newItem;
        name = newName;
        stacks = newStacks;
        itemImage = newImage;
        itemDescription = newDescription;
        statType = newStatType;
    }
}