using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChoice : MonoBehaviour
{
    public Item item;
    public Image image;
    public TextMeshProUGUI itemName;
    public TextMeshProUGUI description;
    public GameObject rarityEffect;
    public TextMeshProUGUI type;
    public TextMeshProUGUI extraInputInfo;
    public Image inputAbility;
    public float scaleOnSelect = 2f;

    private void Awake()
    {
        image.color = Color.gray;
    }

    public void OnSelect()
    {
        transform.localScale = new Vector3(scaleOnSelect, scaleOnSelect, scaleOnSelect);
        image.color = Color.white;
    }

    public void OnPerform()
    {
        InventoryItem newItem = new InventoryItem
        {
            item = item.data,
            quantity = 1
        };
            
        ItemsLootBoxManager.Instance.ChooseItem(item, newItem);
    }

    public void OnDeselect()
    {
        transform.localScale = new Vector3(1.7f, 1.7f, 1.7f);
        image.color = Color.gray;
    }

    private IEnumerator IncreaseScale(float _targetScale)
    {
        float _time = 0f;
        float _maxTime = 1f;
        Vector3 startScale = transform.localScale;
        Vector3 targetScale = new Vector3(_targetScale, _targetScale, _targetScale);
        while (_time < _maxTime)
        {
            _time += Time.deltaTime;
            float t = _time / _maxTime;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }

        transform.localScale = targetScale;
    }
}
