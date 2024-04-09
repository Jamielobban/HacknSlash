using UnityEngine;

[System.Serializable]
public class ItemList 
{
    public Item item;
    public int stacks;
    public ItemList(Item newItem, int newStacks)
    {
        item = newItem;
        stacks = newStacks;
    }
}