using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;
    public Inventory inventory;
    public int currentstacks;
    public PlayerControl playerControl;
    // Start is called before the first frame update
    private void Awake()
    {
        inventory = FindObjectOfType<Inventory>();
        playerControl = FindObjectOfType<PlayerControl>();
    }
    void Start()
    {
        item = AssignItem(itemDrop);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player enter");
            PlayerControl player = other.GetComponent<PlayerControl>();
            AddItem(player);
            inventory.AddItem(item, currentstacks/2);
            inventory.RefreshInventory();
            currentstacks = 0;
            
            Destroy(this.gameObject);
        }
    }
    public Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
        {
            case Items.HealingItem: return new HealingItem();
            case Items.FireDamageItem: return new FireDamage();
            case Items.HealingAreaItem: return new HealingArea();
            default: return null;
        }
    }

    public void AddItem(PlayerControl player)
    {
        //currentstacks = 0;
        foreach (ItemList i in player.items)
        {
            if (i.name == item.GiveName())
            {
                i.stacks++;
                currentstacks++;
                Debug.Log(currentstacks);
                return;

            }
        }
        player.items.Add(new ItemList(item, item.GiveName(), 1, item.GiveSprite()));
        inventory.AddItem(item, 1);
    }

    //public void AddToInventory()
    //{

    //}
}

public enum Items
{
    HealingItem,
    FireDamageItem,
    HealingAreaItem
}