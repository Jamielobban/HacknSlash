using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;
    public Inventory inventory;
    public int currentstacks;
    public PlayerControl playerControl;
    //public GameObject itemPopup;
    //public Image canvasImage;
    //public TMP_Text itemDescriptionText;
    //private Tween itemPopupTween;
    //private WaitForSeconds popupDelay = new WaitForSeconds(3f);
    // Start is called before the first frame update
    private void Awake()
    {
        //itemPopup = GameObject.FindGameObjectWithTag("ItemPopup");
        //canvasImage = GameObject.FindGameObjectWithTag("ItemImage").GetComponent<Image>();
        //itemDescriptionText = GameObject.FindGameObjectWithTag("ItemText").GetComponent<TMP_Text>();
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
            //Debug.Log("Player enter");
            PlayerControl player = other.GetComponent<PlayerControl>();
            AddItem(player);
            inventory.AddItem(item, currentstacks/2);
            inventory.RefreshInventory();
            player.CallItemOnPickup(item.GetAssociatedStatType());
            ItemPopupManager.Instance.ShowItemTooltip(item.GiveSprite(), item.GiveDescription());
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
            case Items.CritItem: return new CritItem();
            case Items.AttackDamageItem: return new AttackDamge();
            case Items.MeatItem: return new Meat();
            case Items.MonsterToothItem: return new MonsterTooth();
            case Items.GasolineItem: return new Gasoline();
            case Items.FeatherItem: return new Feather();

            default: return null;
        }
    }

    public void AddItem(PlayerControl player)
    {
        //currentstacks = 0;
        foreach (ItemList i in player.items)
        {
            if (i.item.GiveName() == item.GiveName())
            {
                i.stacks++;
                currentstacks++;
                //Debug.Log(currentstacks);
                Debug.Log(i.item.GiveName());
                return;
            }
        }

        // Retrieve the associated StatType from the item
        StatType associatedStatType = item.GetAssociatedStatType();

        player.items.Add(new ItemList(item, item.GiveName(), 1, item.GiveSprite(), item.GiveDescription(), associatedStatType));
        inventory.AddItem(item, 1);
    }
}
public enum Items
{
    HealingItem,
    FireDamageItem,
    HealingAreaItem,
    CritItem,
    AttackDamageItem,
    MeatItem,
    MonsterToothItem,
    GasolineItem,
    FeatherItem
}
