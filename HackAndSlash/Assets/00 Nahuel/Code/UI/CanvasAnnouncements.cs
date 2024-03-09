using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnnouncements : MonoBehaviour
{
    public GameObject eventCompleted;
    public List<GDTFadeEffect> fadeEffects = new List<GDTFadeEffect>();

    public void ShowNotification(string titleToShow, string textToShow)
    {
    }

    public void ShowEventCompleted()
    {
        eventCompleted.SetActive(true);
        Invoke(nameof(HideEventCompleted), 1f);
    }

    private void HideEventCompleted()
    {
        foreach (var fade in fadeEffects)
        {
            fade.StartEffect();
        }
        Invoke(nameof(DeactiveEvent), fadeEffects[0].timeEffect);
    }

    private void DeactiveEvent()
    {
        eventCompleted.SetActive(false);
        AbilityPowerManager.instance.ShowNewOptions();
    }

}
