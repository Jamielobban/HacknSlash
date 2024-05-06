using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage _inventoryUI;
    [SerializeField] private InventorySO _inventoryData;
    public int inventorySize = 20;
    private ControllerManager _controller;

    public GameObject musicObj, inventoryObj;
    public GameObject[] canvasToHide;

    private void Start()
    {
        _controller = FindAnyObjectByType<ControllerManager>();
        _inventoryData.Initialize();
        _inventoryUI.InitializeInventoryUI(_inventoryData.Size);
        _inventoryData.OnInventoryUpdated += UpdateInventoryUI;
        _inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
        _inventoryUI.Show();
        foreach (var item in _inventoryData.GetCurrentInventoryState())
        {
            _inventoryUI.UpdateData(item.Key, item.Value.item.itemIcon, item.Value.quantity, item.Value.item.itemDefaultDescription);
        }
        _inventoryUI.Hide();
    }

    private void UpdateInventoryUI(Dictionary<int, InventoryItem> inventoryState)
    {
        _inventoryUI.ResetAllItems();
        foreach (var item in inventoryState)
        {
            _inventoryUI.UpdateData(item.Key, item.Value.item.itemIcon, item.Value.quantity, item.Value.item.itemDefaultDescription);
        }
    }

    private void HandleDescriptionRequested(int itemIndex)
    {
        InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.IsEmpty)
        {
            return;
        }

        ItemData item = inventoryItem.item;
        _inventoryUI.UpdateDescription(itemIndex, item.itemIcon, item.itemName, item.itemDefaultDescription);
    }

    private void ResetInventory()
    {
        inventoryObj.SetActive(true);
        musicObj.SetActive(false);
    }

    private void Update()
    {
        if (_controller.GetTabButton().action != null)
        {
            if (_controller.GetTabButton().action.WasPressedThisFrame() && !ItemsLootBoxManager.Instance.isOpen)
            {
                if (!_inventoryUI.isActiveAndEnabled)
                {
                    ResetInventory();
                    _inventoryUI.Show();
                    foreach (var item in _inventoryData.GetCurrentInventoryState())
                    {
                        _inventoryUI.UpdateData(item.Key, item.Value.item.itemIcon, item.Value.quantity, item.Value.item.itemDefaultDescription);
                    }
                    foreach (var canvas in canvasToHide)
                    {
                        canvas.SetActive(false);
                    }
                    AudioManager.Instance.PlayFx(Enums.Effects.OpenInventory);
                    EventSystem.current.SetSelectedGameObject(_inventoryUI.ListOfUIItems[0].gameObject); //selectedObject
                    GameManager.Instance.PauseMenuGame();
                }
                else
                {
                    foreach (var canvas in canvasToHide)
                    {
                        canvas.SetActive(true);
                    }
                    _inventoryUI.Hide();
                    GameManager.Instance.UnPauseMenuGame();
                    EventSystem.current.SetSelectedGameObject(null);
                }
                
            }
        }
    }
}
