using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public Item[] commonItems;
    public Item[] uncommonItems;
    public Item[] rareItems;
    public Item[] legendaryItems;

    public List<Item> actionItems = new List<Item>();

    public List<Item> abilityItems = new List<Item>();

    public int commonChance = 40;
    public int uncommonChance = 30;
    public int rareChance = 20;
    public int legendaryChance = 3;
    public int itemActionChance = 2;
    public int abilityChance = 5;

    public int itemsToGetAbility = 15;
    private int currentItemsShown = 0;
    
    public Item GetRandomItem()
    {
        int totalChance = commonChance + uncommonChance + rareChance + legendaryChance + itemActionChance + abilityChance;
        int randomValue = Random.Range(0, totalChance);

        currentItemsShown++;

        if(currentItemsShown >= itemsToGetAbility)
        {
            randomValue = totalChance;
        }
        if (randomValue < commonChance)
        {
            return GetRandomItemInArray(commonItems);
        }
        else if (randomValue < commonChance + uncommonChance)
        {
            return GetRandomItemInArray(uncommonItems);
        }
        else if (randomValue < commonChance + uncommonChance + rareChance)
        {
            return GetRandomItemInArray(rareItems);
        }
        else if(randomValue < commonChance + uncommonChance + rareChance + legendaryChance)
        {
            return GetRandomItemInArray(legendaryItems);
        }
        else if(randomValue < commonChance + uncommonChance + rareChance + legendaryChance + itemActionChance)
        {
            Item _item = GetRandomItemGuard(actionItems);
            if(_item != null)
            {
                actionItems.Remove(_item);
                return _item;
            }
            // Si no hay abilidades Legendary Item
            return GetRandomItemInArray(legendaryItems);
        }
        else
        {
            currentItemsShown = 0;
            if(GameManager.Instance.Player._abilityHolder.AnyAbilityEmpty())
            {
                Item _item;
                do
                {
                    _item = GetRandomAbilityGuard(abilityItems);

                } while(!GameManager.Instance.Player._abilityHolder.CanAddAbility(_item.data.ìnput));

                if (_item != null)
                {
                    return _item;
                }
            }
            // Si no hay abilidades Rare Item
            return GetRandomItemInArray(rareItems); 
        }
    }


    private Item GetRandomItemGuard(List<Item> list)
    {
        if(list.Count <= 0)
        {
            return null;
        }

        return list[Random.Range(0, list.Count)];
    }

    private Item GetRandomAbilityGuard(List<Item> list)
    {
        if(list.Count <= 0)
        {
            return null;
        }

        Item newItem;
        do
        {
            newItem = list[Random.Range(0, list.Count)];
        } while(ItemsLootBoxManager.Instance.itemsToSpawn.Contains(newItem));

        return newItem;
    }

    private Item GetRandomItemInArray(Item[] itemsArray) => itemsArray[Random.Range(0, itemsArray.Length)]; 

    public void SpawnItem(Vector3 initialPosition, GameObject itemSpawn)
    {
        Instantiate(itemSpawn, initialPosition, Quaternion.identity);
    }

}