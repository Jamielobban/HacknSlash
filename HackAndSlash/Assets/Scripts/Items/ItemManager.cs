using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

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
    
    public Item GetRandomItem()
    {
        if(ManagerEnemies.Instance.IsMaxScore())
        {
            ManagerEnemies.Instance.ResetScore();
            if (GameManager.Instance.Player._abilityHolder.CanAddAbility())
            {
                Item _item = GetRandomItemGuard(abilityItems);

                if (_item != null)
                {
                    abilityItems.Remove(_item);
                    return _item;
                }
            }
            // Si no hay abilidades Rare Item
            return GetRandomItemInArray(rareItems);
        }

        int totalChance = commonChance + uncommonChance + rareChance + legendaryChance + itemActionChance + abilityChance;
        int randomValue = Random.Range(0, totalChance);

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
            if(GameManager.Instance.Player._abilityHolder.CanAddAbility())
            {
                Item _item = GetRandomItemGuard(abilityItems);

                if (_item != null)
                {
                    abilityItems.Remove(_item);
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

    private Item GetRandomItemInArray(Item[] itemsArray) => itemsArray[Random.Range(0, itemsArray.Length)]; 

    public void SpawnItem(Vector3 initialPosition, GameObject itemSpawn)
    {
        Instantiate(itemSpawn, initialPosition, Quaternion.identity);
    }

}