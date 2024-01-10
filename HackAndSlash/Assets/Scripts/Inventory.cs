using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{


    [SerializeReference] public List<ItemSlotInfo> items = new List<ItemSlotInfo>();

    [Space]
    [Header("Inventory menu components")]
    public GameObject inventorymenu;
    public GameObject itemPanel;
    public GameObject itemPanelGrid;


    public List<ItemPanel> existingPanels = new List<ItemPanel>();
    [Space]
    public int inventorySize = 14;

    public PlayerStats playerStats;
    public GameObject itemDescription;
    public GameObject backdrop;
    ControllerManager controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindAnyObjectByType<ControllerManager>().GetComponent<ControllerManager>();

        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlotInfo(null, 0));
        }
        //AddItem(new HealingItem(), 15);
        //AddItem(new FireDamage(), 15);
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetTabButton().action != null)
        {
            if (controller.GetTabButton().action.WasPressedThisFrame())
            {
                if (inventorymenu.activeSelf)
                {
                    inventorymenu.SetActive(false);
                    itemDescription.SetActive(false);
                    backdrop.SetActive(false);
                    Time.timeScale = 1.0f;
                }
                else
                {
                    inventorymenu.SetActive(true);
                    itemDescription.SetActive(true);
                    backdrop.SetActive(true);
                   
                    RefreshInventory();
                    Time.timeScale = 0.0f;
                }
            }
        }
        //if(Input.GetKeyDown(KeyCode.K))
        //{

        //        ItemManager.instance.SpawnRandomItem(GameObject.FindObjectOfType<PlayerControl>().transform.position + new Vector3(0f, 5f, 0f));

            
        //}
    }

    public void RefreshInventory()
    {
        playerStats.RefreshStats();
        existingPanels = itemPanelGrid.GetComponentsInChildren<ItemPanel>().ToList();

        if (existingPanels.Count < inventorySize)
        {
            int amountToCreate = inventorySize - existingPanels.Count;
            for (int i = 0; i < amountToCreate; i++)
            {
                GameObject newPanel = Instantiate(itemPanel, itemPanelGrid.transform);
                existingPanels.Add(newPanel.GetComponent<ItemPanel>());
            }
        }
            int index = 0;
            foreach (ItemSlotInfo i in items)
            {
                i.name = "" + (index + 1);
                if (i.item != null) i.name += ": " + i.item.GiveName();
                else i.name += ": -";

                ItemPanel panel = existingPanels[index];
                if (panel != null)
                {
                    panel.name = i.name + " Panel";
                    panel.inventory = this;
                    panel.itemSlot = i;
                    if (i.item != null)
                    {
                        panel.itemImage.gameObject.SetActive(true);
                        panel.itemImage.sprite = i.item.GiveSprite();
                        panel.stacksText.gameObject.SetActive(true);
                        panel.stacksText.text = " " + i.stacks;
                        //Debug.Log("refresh");
                        //Debug.Log(i.stacks);
                    }
                    else
                    {
                        panel.itemImage.gameObject.SetActive(false);

                        panel.stacksText.gameObject.SetActive(false);

                    }
                }
                index++;
            
            }
    }
    const int max = 99;
    public int AddItem(Item item, int amount)
    {
        //Debug.Log("Added item");
        foreach (ItemSlotInfo i in items)
        {
            if (i.item != null)
            {
                if (i.item.GiveName() == item.GiveName())
                {
                    if (amount > max - i.stacks)
                    {
                        amount -= max - i.stacks;
                        i.stacks = max;
                    }
                    else
                    {
                        i.stacks = amount;
                        if (inventorymenu.activeSelf) RefreshInventory();
                        return 0;
                    }
                }
            }
        }

        foreach (ItemSlotInfo i in items)
        {
            if (i.item == null)
            {
                if (amount > max)
                {
                    i.item = item;
                    i.stacks = max;
                    amount -= max;
                }
                else
                {
                    i.item = item;
                    i.stacks = amount;
                    if (inventorymenu.activeSelf) RefreshInventory();
                    return 0;
                }
            }
        }
        if (inventorymenu.activeSelf) RefreshInventory();
        return amount;
    }

    public void ClearSlot(ItemSlotInfo slot)
    {
        slot.item = null;
        slot.stacks = 0;
    }

    public int GetStacks(Item item)
    {
        foreach (ItemSlotInfo i in items)
        {
            if (i.item != null && i.item.GiveName() == item.GiveName())
            {
                return i.stacks;
            }
        }
        return 0; // Default value if the item is not found
    }
}
