using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.Rendering;

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

    // Start is called before the first frame update
    void Start()
    {
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
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventorymenu.activeSelf)
            {
                inventorymenu.SetActive(false);

            }
            else
            {
                inventorymenu.SetActive(true);
                //AddItem(new HealingItem(), 1);
                Debug.Log("Hola");
                RefreshInventory();
            }
        }
    }

    public void RefreshInventory()
    {
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
                        i.stacks += amount;
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
        Debug.Log("None");
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
