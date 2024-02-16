using System.Collections;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;

public class ItemPopupManager : MonoBehaviour
{
    private static ItemPopupManager instance;

    public GameObject ItemTooltip;
    public CanvasGroup popupCanvasGroup;
    private Tween itemPopupTween;
    public Image itemImage;
    public TMP_Text itemDescriptionText;
    private WaitForSeconds popupDelay = new WaitForSeconds(3f);

    private void Awake()
    {
        // Singleton pattern to ensure only one instance exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(ItemTooltip);
        }
        else
        {
            Destroy(ItemTooltip);
        }

        // Initialize other manager-specific setup here
    }

    public static ItemPopupManager Instance
    {
        get { return instance; }
    }

    public void ShowItemTooltip(Sprite itemSprite, string itemDescription)
    {
        if (!ItemTooltip.activeSelf)
        {
            ItemTooltip.SetActive(true);
        }
        // Kill the active tween if it exists
        KillActiveTween();

        // Set the item sprite and description
        itemImage.sprite = itemSprite;
        itemDescriptionText.text = itemDescription;

        // Fade in
        popupCanvasGroup.alpha = 0f;
        itemPopupTween = popupCanvasGroup.DOFade(1f, 0.3f).SetEase(Ease.OutQuad);

        // Activate the item popup

        // Start the coroutine to hide the popup after the delay
        StartCoroutine(WaitAndHidePopup());
    }

    private IEnumerator WaitAndHidePopup()
    {
        yield return popupDelay;

        // Fade out only if no new item has been picked up in the meantime
        if (itemPopupTween == null || !itemPopupTween.IsPlaying())
        {
            itemPopupTween = popupCanvasGroup.DOFade(0f, 0.5f).SetEase(Ease.InQuad)
                .OnComplete(() => {
                    itemPopupTween = null;
                    // Set itemPopupTween to null when completed
                    ItemTooltip.SetActive(false); // Deactivate the item popup when the popup fades out
                });
        }
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