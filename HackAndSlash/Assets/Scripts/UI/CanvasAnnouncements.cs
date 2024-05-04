using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasAnnouncements : MonoBehaviour
{
    public GameObject eventCompleted;
    public GameObject eventDefeated;
    public GameObject notification;

    public List<Image> imagesNotification = new List<Image>();
    public List<Image> imagesOnComplete = new List<Image>();
    public List<Image> imagesOnDefeat = new List<Image>();

    public List<TextMeshProUGUI> textsNotification = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textsOnComplete = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> textsOnDefeat = new List<TextMeshProUGUI>();


    public TextMeshProUGUI titleNotification, descriptionNotification;

    public void ShowNotification(string titleToShow, string textToShow)
    {
        notification.SetActive(true);
        FadeAlpha(imagesNotification, 1, 1);
        FadeAlpha(textsNotification, 1, 1);
        Invoke(nameof(HideNotification), 1.5f);
    }
    private void HideNotification()
    {
        FadeAlpha(imagesNotification, 1, 1);
        FadeAlpha(textsNotification, 1, 1);
        Invoke(nameof(DeactiveNotification), 1.5f);
    }

    private void DeactiveNotification() => notification.SetActive(false);

    public void ShowEventCompleted()
    {
        eventCompleted.SetActive(true);

        FadeAlpha(imagesOnComplete, 1, 1);
        FadeAlpha(textsOnComplete, 1, 1);

        Invoke(nameof(HideEventCompleted), 1f);
    }
    private void HideEventCompleted()
    {
        FadeAlpha(imagesOnComplete, 0, 1);
        FadeAlpha(textsOnComplete, 0, 1);

        Invoke(nameof(DeactiveCompleted), 1.5f);
    }
    private void DeactiveCompleted()
    {
        eventCompleted.SetActive(false);
        ItemsLootBoxManager.Instance.ShowNewOptions();
    }

    public void ShowEventDefeated()
    {
        eventDefeated.SetActive(true);

        FadeAlpha(imagesOnDefeat, 1, 1);
        FadeAlpha(textsOnDefeat, 1, 1);

        Invoke(nameof(HideEventDefeated), 1f);
    }

    private void HideEventDefeated()
    {
        FadeAlpha(imagesOnDefeat, 0, 1);
        FadeAlpha(textsOnDefeat, 0, 1);
        Invoke(nameof(DeactiveEventDefeated), 1f);
    }
        
    private void DeactiveEventDefeated() => eventDefeated.SetActive(false);

    private void FadeAlpha<T>(List<T> list, float valueAlpha, float timeToFade) where T : Graphic
    {
        foreach (var image in list)
        {
            Color c = image.color;
            c.a = valueAlpha;
            image.DOColor(c, timeToFade).SetEase(Ease.InOutSine);
        }
    }

}
