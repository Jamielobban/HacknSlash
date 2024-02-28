using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("Item Manager");
                    go.AddComponent<ItemManager>();
                    DontDestroyOnLoad(go);
                }
            }
            return _instance;
        }
    }

    //public List<GameObject> itemPool = new List<GameObject>();
    //public int test;
    public List<GameObject> itemPool = new List<GameObject>();

    public List<Item> itemList = new List<Item>();
    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (_instance == null)
        {
            _instance = this;
            //DontDestroyOnLoad(ItemTooltip);
        }
        // Initialize other manager-specific setup here
        AddItemsToList();
        
    }

    void Start()
    {
        //foreach (var item in itemList)
        //{
        //    Debug.Log(item.GiveName());
        //}
    }

    void Update()
    {
        //Debug.Log(itemList.Count);
    }

    public Item GetRandomItem()
    {
        Item itemToReturn;
        //if (itemPool.Count == 0)
        //{
        //    Debug.LogWarning("Item pool is empty. Add items to the pool.");
        //    return null;
        //}
        foreach (var item in itemList)
        {
            Debug.Log(item.GiveName());
        }
        itemToReturn = itemList[Random.Range(0, itemList.Count)];
        //Debug.Log(itemToReturn.GiveName());
        return itemToReturn;
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
        itemList.Add(new AttackDamge());
        itemList.Add(new Meat());
        //itemList.Add(new BoosterShot());
        //itemList.Add(new MonsterTooth());
        //itemList.Add(new Slashes());
        //itemList.Add(new DoubleHit());
        //
    }

}