using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class ItemManager : MonoBehaviour
{
    //private static ItemManager _instance;
    //public static ItemManager instance
    //{
    //    get
    //    {
    //        if (_instance == null)
    //        {
    //            _instance = FindObjectOfType<ItemManager>();
    //            if (_instance == null)
    //            {
    //                GameObject go = new GameObject("Item Manager");
    //                go.AddComponent<ItemManager>();
    //                DontDestroyOnLoad(go);
    //            }
    //        }
    //        return _instance;
    //    }
    //}

    public List<Item> itemList = new List<Item>();

    public int commonChance = 40;
    public int uncommonChance = 30;
    public int rareChance = 20;
    public int legendaryChance = 10;

    private void Awake()
    {
        //if (_instance == null)
        //{
        //    _instance = this;
        //}
        //AddItemsToList();
        
    }


    public Item GetRandomItem()
    {
        int totalChance = commonChance + uncommonChance + rareChance + legendaryChance;
        int randomValue = Random.Range(0, totalChance);
        Debug.Log(randomValue);

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
        else
        {
            return GetRandomItemOfRarity(RarityType.Legendary);
        }
    }



    //public Item GetRandomItem()
    //{
    //    Item itemToReturn;

    //    int valueRandom = Random.Range(0, 100);

    //    itemToReturn = itemList[Random.Range(0, itemList.Count)];
    //    //Debug.Log(itemToReturn.GiveName());
    //    return itemToReturn;
    //}

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
    // Call this method when you want to spawn a random item
    public void SpawnRandomItem(Vector3 initialPosition)
    {
        //bool spawnOnce = false;
        //if (itemPool.Count == 0)
        //{
        //    Debug.LogWarning("Item pool is empty. Add items to the pool.");
        //    return;
        //}
        ////if (!spawnOnce)
        ////{

        //// Instantiate the item at the initial position
        //GameObject randomItem = Instantiate(itemPool[Random.Range(0, itemPool.Count)], initialPosition, Quaternion.identity);
        //Vector3 playerForward = GameObject.FindObjectOfType<PlayerControl>().transform.GetChild(0).transform.forward;
        //Vector3 randomDirection = Quaternion.Euler(0, Random.Range(-45, 45), 0) * playerForward;
        //Vector3 targetPosition = initialPosition + randomDirection * Random.Range(5f, 10f);
        //RaycastHit hit;

        //if (Physics.Raycast(targetPosition, new Vector3(0, -1, 0), out hit, 200, 1 << 7))
        //{
        //    targetPosition = new Vector3(targetPosition.x, hit.point.y + 2, targetPosition.z);
        //}
        //spawnOnce = true;
        // Use DoTween to animate the item from the initial to the target position
        //randomItem.transform.DOJump(targetPosition, 2.0f, 1, 2.0f)
        // .SetEase(Ease.OutQuart)
        // .OnComplete(() =>
        // {
        //     // Animation complete, item can be picked up or further processed
        //     randomItem.GetComponent<SphereCollider>().enabled = true;
        //     randomItem.GetComponent<BounceEffect>().enabled = true;
        // });
        //}
    }

    public void AddItemsToList()
    {
        //itemList.Add(new CritItem());
        //itemList.Add(new CritDamageItem());
        //itemList.Add(new AttackDamge());
        //itemList.Add(new MaxHealthItem());
        //itemList.Add(new RecoveryFlower());
        //itemList.Add(new RegenerationItem());
        //itemList.Add(new MonsterTooth());
        //itemList.Add(new Slashes());
        //itemList.Add(new DoubleHit());
        //
    }

}