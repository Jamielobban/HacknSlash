using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIInventoryItem : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _stackText;
    public string description;
    [SerializeField] private Image _borderImage;
    [SerializeField] private UIInventoryPage _inventoryPage;

    private void Awake()
    {
        ResetData();
        Deselect();
    }

    private void Start()
    {
        _inventoryPage = FindObjectOfType<UIInventoryPage>();
    }

    public void ResetData()
    {
        _itemImage.gameObject.SetActive(false);
        _borderImage.gameObject.SetActive(false);
    }
    
    public void SetData(Sprite sprite, int quantity, string desc)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _stackText.text = "" + quantity;
        description = desc;
    }

    public void Select()
    {
        _borderImage.gameObject.SetActive(true); //Show Description
        _inventoryPage.HandleItemSelection(this);
    }


    public void Deselect()
    {
        _borderImage.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        Deselect();
    }
}
