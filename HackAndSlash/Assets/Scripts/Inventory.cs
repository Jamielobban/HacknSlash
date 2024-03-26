using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.EventSystems;

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

    public GameObject itemDescription;
    ControllerManager controller;
    public GameObject selectedObject;


    void Start()
    {
        controller = FindAnyObjectByType<ControllerManager>();

        for (int i = 0; i < inventorySize; i++)
        {
            items.Add(new ItemSlotInfo(null, 0));
        }
    }

    void Update()
    {
        if (controller.GetTabButton().action != null)
        {
            if (controller.GetTabButton().action.WasPressedThisFrame())
            {
                if (inventorymenu.activeSelf)
                {
                    inventorymenu.SetActive(false);

                    if (!AbilityPowerManager.instance.isOpen)
                    {
                        GameManager.Instance.UnPauseMenuGame();
                        EventSystem.current.SetSelectedGameObject(null);
                    }
                }
                else
                {
                    if(!GameManager.Instance.isInMenu)
                    {
                        inventorymenu.SetActive(true);
                        AudioManager.Instance.PlayFx(Enums.Effects.OpenInventory);
                        //Set
                        EventSystem.current.SetSelectedGameObject(selectedObject);
                        RefreshInventory();

                        GameManager.Instance.PauseMenuGame();
                    }
                }
            }
        }

    }

    public void RefreshInventory()
    {
        //playerStats.RefreshStats();
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
