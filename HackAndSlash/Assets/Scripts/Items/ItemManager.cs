using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Enums;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public List<Item> itemList = new List<Item>();

    public int commonChance = 40;
    public int uncommonChance = 30;
    public int rareChance = 20;
    public int legendaryChance = 5;
    public int abilityChance = 5;
    
    public Item GetRandomItem()
    {
        int totalChance = commonChance + uncommonChance + rareChance + legendaryChance + abilityChance;
        int randomValue = Random.Range(0, totalChance);

        if (randomValue < commonChance)
        {
            return GetRandomItemOfRarity(RarityType.Common);
        }
        else if (randomValue < commonChance + uncommonChance)
        {
            return GetRandomItemOfRarity(RarityType.Uncommon);
        }
        else if (randomValue < commonChance + uncommonChance + rareChance)
        {
            return GetRandomItemOfRarity(RarityType.Rare);
        }
        else if(randomValue < commonChance + uncommonChance + rareChance + legendaryChance)
        {
            return GetRandomItemOfRarity(RarityType.Legendary);
        }
        else
        {
            if(GameManager.Instance.Player._abilityHolder.CanAddAbility())
            {
                Item _item;
                do
                {
                    _item = GetRandomItemOfRarity(RarityType.Ability);

                } while (GameManager.Instance.Player.inventory.IsItemInInventoryByID(_item.data.ID));
                return _item;
            }
            else
            {
                return GetRandomItemOfRarity(RarityType.Legendary);
            }
        }
    }

    private Item GetRandomItemOfRarity(RarityType rarity)
    {
        List<Item> itemsOfRarity = itemList.FindAll(item => item.data.rarityType == rarity);

        if (itemsOfRarity.Count == 0)
        {
            Debug.LogWarning("No items of rarity " + rarity + " found.");
            return null;
        }

        return itemsOfRarity[Random.Range(0, itemsOfRarity.Count)];
    }

    public void SpawnItem(Vector3 initialPosition, GameObject itemSpawn)
    {
        Instantiate(itemSpawn, initialPosition, Quaternion.identity);
    }

}