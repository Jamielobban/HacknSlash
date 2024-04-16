using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class UIInventoryPage : MonoBehaviour
{
    [SerializeField] private UIInventoryItem _itemPrefab;
    [SerializeField] private RectTransform _slotsGridContent;

    private List<UIInventoryItem> _listOfUIItems = new List<UIInventoryItem>();
    
    public void InitializeInventoryUI(int inventorySize)
    {
        for (int i = 0; i < inventorySize; i++)
        {
            UIInventoryItem uiItem = Instantiate(_itemPrefab, Vector3.zero, Quaternion.identity);
            uiItem.transform.SetParent(_slotsGridContent);
            uiItem.transform.localScale = Vector3.one;
            uiItem.transform.localPosition = new Vector3(uiItem.transform.position.x, uiItem.transform.position.y, 0);
            _listOfUIItems.Add(uiItem);
        }
    }

    public void Show() => gameObject.SetActive(true);
    public void Hide() => gameObject.SetActive(false);

}
