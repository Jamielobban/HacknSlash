using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage _inventoryUI;
    [SerializeField] private InventorySO _inventoryData;
    public int inventorySize = 20;
    private ControllerManager _controller;

    private void Start()
    {
        _controller = FindAnyObjectByType<ControllerManager>();
        _inventoryUI.InitializeInventoryUI(_inventoryData.Size);
        _inventoryUI.OnDescriptionRequested += HandleDescriptionRequested;
        _inventoryUI.OnItemActionRequested += HandleItemActionnRequested;
    }

    private void HandleItemActionnRequested(int itemIndex)
    {
    }

    private void HandleDescriptionRequested(int itemIndex)
    {
        InventoryItem inventoryItem = _inventoryData.GetItemAt(itemIndex);
        if (inventoryItem.isEmpty)
        {
            return;
        }

        ItemData item = inventoryItem.item;
        _inventoryUI.UpdateDescription(itemIndex, item.itemIcon, item.itemName, item.itemDescription);
    }


    private void Update()
    {
        if (_controller.GetTabButton().action != null)
        {
            if (_controller.GetTabButton().action.WasPressedThisFrame())
            {
                if (!_inventoryUI.isActiveAndEnabled)
                {
                    _inventoryUI.Show();
                    foreach (var item in _inventoryData.GetCurrentInventoryState())
                    {
                        _inventoryUI.UpdateData(item.Key, item.Value.item.itemIcon, item.Value.quantity, item.Value.item.itemDescription);
                    }
                    AudioManager.Instance.PlayFx(Enums.Effects.OpenInventory);
                    EventSystem.current.SetSelectedGameObject(_inventoryUI.ListOfUIItems[0].gameObject); //selectedObject
                    GameManager.Instance.PauseMenuGame();
                    //RefreshInventory();
                }
                else
                {
                    _inventoryUI.Hide();
                    GameManager.Instance.UnPauseMenuGame();
                    EventSystem.current.SetSelectedGameObject(null);
                }
                
            }
        }
    }
}
