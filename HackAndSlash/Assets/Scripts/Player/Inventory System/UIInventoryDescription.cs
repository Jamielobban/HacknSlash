using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryDescription : MonoBehaviour
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _title;
    [SerializeField] private TextMeshProUGUI _description;

    private void Awake()
    {
        ResetDescription();
    }
    public void ResetDescription()
    {
        _itemImage.gameObject.SetActive(false);
        _title.text = "";
        _description.text = "";
    }

    public void SetDescription(Sprite sprite, string itemName, string itemDescription)
    {
        _itemImage.gameObject.SetActive(true);
        _itemImage.sprite = sprite;
        _title.text = itemName;
        _description.text = itemDescription;
    }
}
