using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ItemPickup : MonoBehaviour
{
    public Item item;
    public Items itemDrop;
    public Inventory inventory;
    public int currentstacks;
    public PlayerControl playerControl;
    public GameObject itemPopup;
    public Image canvasImage;
    public TMP_Text itemDescriptionText;
    private Tween itemPopupTween;
    private WaitForSeconds popupDelay = new WaitForSeconds(3f);
    // Start is called before the first frame update
    private void Awake()
    {
        itemPopup = GameObject.FindGameObjectWithTag("ItemPopup");
        canvasImage = GameObject.FindGameObjectWithTag("ItemImage").GetComponent<Image>();
        itemDescriptionText = GameObject.FindGameObjectWithTag("ItemText").GetComponent<TMP_Text>();
        inventory = FindObjectOfType<Inventory>();
        playerControl = FindObjectOfType<PlayerControl>();
    }
    void Start()
    {
        item = AssignItem(itemDrop);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player enter");
            PlayerControl player = other.GetComponent<PlayerControl>();
            AddItem(player);
            inventory.AddItem(item, currentstacks/2);
            inventory.RefreshInventory();
            player.CallItemOnPickup(item.GetAssociatedStatType());
            SwitchItemAndRestartTween(item);
            //ShowItemTooltip(item);
            //Debug.Log("Add item");
            currentstacks = 0;

            GetComponent<SphereCollider>().enabled = false;
            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<BounceEffect>().enabled = false;

            StartCoroutine(DelayedDestroy(gameObject, 5f));
        }
    }
    public Item AssignItem(Items itemToAssign)
    {
        switch (itemToAssign)
        {
            case Items.HealingItem: return new HealingItem();
            case Items.FireDamageItem: return new FireDamage();
            case Items.HealingAreaItem: return new HealingArea();
            case Items.CritItem: return new CritItem();
            case Items.AttackDamageItem: return new AttackDamge();
            case Items.MeatItem: return new Meat();
            case Items.MonsterToothItem: return new MonsterTooth();
            case Items.GasolineItem: return new Gasoline();
            case Items.FeatherItem: return new Feather();

            default: return null;
        }
    }

    public void AddItem(PlayerControl player)
    {
        //currentstacks = 0;
        foreach (ItemList i in player.items)
        {
            if (i.item.GiveName() == item.GiveName())
            {
                i.stacks++;
                currentstacks++;
                //Debug.Log(currentstacks);
                Debug.Log(i.item.GiveName());
                return;
            }
        }

        // Retrieve the associated StatType from the item
        StatType associatedStatType = item.GetAssociatedStatType();

        player.items.Add(new ItemList(item, item.GiveName(), 1, item.GiveSprite(), item.GiveDescription(), associatedStatType));
        inventory.AddItem(item, 1);
    }
    private IEnumerator DelayedDestroy(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Cleanup or additional actions if needed
        // ...

        // Destroy the object
        Destroy(obj);
    }
    public void ShowItemTooltip(Item item)
    {
        if (item != null && itemPopup != null)
        {
            // Kill the active tween if it exists
            //KillActiveTween();

            canvasImage.sprite = item.GiveSprite();
            itemDescriptionText.text = item.GiveDescription();

            // Fade in
            itemPopup.GetComponent<CanvasGroup>().alpha = 0f;
            itemPopupTween = itemPopup.GetComponent<CanvasGroup>().DOFade(1f, 0.3f).SetEase(Ease.OutQuad);

            StartCoroutine(WaitAndHidePopup());
        }
    }

    private IEnumerator WaitAndHidePopup()
    {
        Debug.Log("Waiting");
        yield return new WaitForSeconds(3f); // Wait for the specified delay
        Debug.Log("Waiting complete");

        // Check if the itemPopupTween is not already playing
        if (itemPopupTween == null || !itemPopupTween.IsPlaying())
        {
            // Fade out only if no new item has been picked up in the meantime
            itemPopupTween = itemPopup.GetComponent<CanvasGroup>().DOFade(0f, 2f).SetEase(Ease.InQuad)
                .OnComplete(() => itemPopupTween = null); // Set itemPopupTween to null when completed
        }
    }

    private void SwitchItemAndRestartTween(Item newItem)
    {
        // Kill the active tween before switching to the new item
        KillActiveTween();

        // Switch to the new item
        item = newItem;

        // Restart the tween with the new item info
        ShowItemTooltip(item);

        // Start the coroutine to hide the popup after the delay
        StartCoroutine(WaitAndHidePopup());
    }

    private void KillActiveTween()
    {
        // Kill the active tween if it exists and is playing
        if (itemPopupTween != null && itemPopupTween.IsPlaying())
        {
            itemPopupTween.Kill();
            itemPopupTween = null; // Set itemPopupTween to null after killing
        }
    }
}
public enum Items
{
    HealingItem,
    FireDamageItem,
    HealingAreaItem,
    CritItem,
    AttackDamageItem,
    MeatItem,
    MonsterToothItem,
    GasolineItem,
    FeatherItem
}
