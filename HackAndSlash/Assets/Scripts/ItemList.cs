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
    //Dictionary<int, int> items;


    public ItemList(Item newItem, string newName, int newStacks, Sprite newImage)
    {
        item = newItem;
        name = newName;
        stacks = newStacks;
        itemImage = newImage;
    }

    public int GetStacks()
    {
        return stacks;
    }

}
