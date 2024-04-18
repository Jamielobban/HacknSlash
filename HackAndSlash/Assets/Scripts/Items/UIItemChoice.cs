using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItemChoice : MonoBehaviour
{
    public Item item;
    public Image image;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public GameObject rarityEffect;
    public float scaleOnSelect = 2f;

    public void OnSelect()
    {
        transform.localScale = new Vector3(1.9f, 1.9f, 1.9f);
        rarityEffect.SetActive(true);
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
        rarityEffect.SetActive(false);
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
