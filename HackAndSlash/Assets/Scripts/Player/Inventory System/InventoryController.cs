using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryController : MonoBehaviour
{
    [SerializeField] private UIInventoryPage _inventoryUI;
    public int inventorySize = 20;
    private ControllerManager _controller;

    private void Start()
    {
        _controller = FindAnyObjectByType<ControllerManager>();
        _inventoryUI.InitializeInventoryUI(inventorySize);
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
                    AudioManager.Instance.PlayFx(Enums.Effects.OpenInventory);
                    EventSystem.current.SetSelectedGameObject(null); //selectedObject
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
