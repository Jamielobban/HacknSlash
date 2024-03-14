using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasAnnouncements : MonoBehaviour
{
    public GameObject eventCompleted;
    public GameObject eventDefeated;
    public List<GDTFadeEffect> fadeEffectsCompleted = new List<GDTFadeEffect>();
    public List<GDTFadeEffect> fadeEffectsDefeated = new List<GDTFadeEffect>();

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
        foreach (var fade in fadeEffectsCompleted)
        {
            fade.StartEffect();
        }
        Invoke(nameof(DeactiveEvent), fadeEffectsCompleted[0].timeEffect * 0.5f);
    }
    private void DeactiveEvent()
    {
        eventCompleted.SetActive(false);
        AbilityPowerManager.instance.ShowNewOptions();
    }
    public void ShowEventDefeated()
    {
        eventDefeated.SetActive(true);
        Invoke(nameof(HideEventDefeated), 1f);
    }

    private void HideEventDefeated()
    {
        foreach (var fade in fadeEffectsDefeated)
        {
            fade.StartEffect();
        }
        Invoke(nameof(DeactiveEventDefeated), fadeEffectsDefeated[0].timeEffect * 0.5f);
    }
    private void DeactiveEventDefeated()
    {
        eventDefeated.SetActive(false);

    }

}
