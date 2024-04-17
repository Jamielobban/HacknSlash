using System;
using System.Collections.Generic;
using UnityEngine;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem _itemPrefab;
    [SerializeField] private RectTransform _slotsGridContent;
    [SerializeField] private UIInventoryDescription _itemDescription;
    private List<UIInventoryItem> _listOfUIItems = new List<UIInventoryItem>();
    public List<UIInventoryItem> ListOfUIItems => _listOfUIItems;

    public event Action<int> OnDescriptionRequested, OnItemActionRequested; 
    
    private void Awake()
    {
        Hide();
        _itemDescription.ResetDescription();
    }

    public void UpdateData(int itemIndex, Sprite itemImage, int itemStack, string description)
    {
        if (_listOfUIItems.Count > itemIndex)
        {
            _listOfUIItems[itemIndex].SetData(itemImage, itemStack, description);
        }
    }

    public void HandleItemSelection(UIInventoryItem inventoryItemUI)
    {
        int index = _listOfUIItems.IndexOf(inventoryItemUI);
        if (index == -1) return;
        OnDescriptionRequested?.Invoke(index);
    }
    
    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(_slotsGridContent);
            uiItem.transform.localScale = Vector3.one;
            uiItem.transform.localPosition = new Vector3(uiItem.transform.position.x, uiItem.transform.position.y, 0);
            uiItem.transform.localRotation = Quaternion.identity;
            _listOfUIItems.Add(uiItem);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _itemDescription.ResetDescription();
    } 
    
    public void Hide() => gameObject.SetActive(false);

    public void UpdateDescription(int index, Sprite icon, string name, string description)
    {
        _itemDescription.SetDescription(icon, name, description);
    }
}
