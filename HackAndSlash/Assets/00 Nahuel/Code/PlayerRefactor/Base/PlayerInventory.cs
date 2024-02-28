using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    private PlayerManager _player;
    public List<ItemList> items;
    private void Awake()
    {
        _player = GetComponent<PlayerManager>();
    }

    void Start()
    {
        StartCoroutine(CallItemUpdate());
    }

    void Update()
    {
        
    }

    private IEnumerator CallItemUpdate()
    {
        foreach (ItemList i in items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }


    public void CallItemOnPickup(StatType desiredStatType)
    {
        foreach (ItemList i in items)
        {
            if (i.statType == desiredStatType)
            {
                i.item.OnItemPickup(this.GetComponent<PlayerManager>(), i.stacks, i.statType); ;
            }
        }
    }


    //Just data
    public int GetItemStacks(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.stacks;
            }
        }
        return 0;
    }

    public bool CheckIfHasItem(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return true;
            }
        }
        return false;
    }

    public string GetItemDescription(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.itemDescription;
            }
        }
        return null;
    }
    public Sprite GetItemImage(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.itemImage;
            }
        }
        return null;
    }

    public RarityType GetItemRarity(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.rarity;
            }
        }
        return 0;
    }

}
