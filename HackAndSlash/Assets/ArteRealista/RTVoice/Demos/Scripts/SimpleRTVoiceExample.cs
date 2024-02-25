using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;
using System.Linq;
using UnityEditor.Rendering;
using MoreMountains.Tools;

/// <summary>
/// Simple example to demonstrate the basic usage of RT-Voice.
/// </summary>
public class SimpleRTVoiceExample : MonoBehaviour
{
   public string Culture = "en";
   public bool SpeakWhenReady;
    public RectTransform textPanelTransform;
    public TextMeshProUGUI dialogText; 
   private string uid; //Unique id of the speech
    public bool playing = false;

   private void OnEnable()
   {
      // Subscribe event listeners
      Speaker.Instance.OnVoicesReady += voicesReady;
      Speaker.Instance.OnSpeakStart += speakStart;
      Speaker.Instance.OnSpeakComplete += speakComplete;

    }

   private void OnDisable()
   {
      if (Speaker.Instance != null)
      {
         // Unsubscribe event listeners
         Speaker.Instance.OnVoicesReady -= voicesReady;
         Speaker.Instance.OnSpeakStart -= speakStart;
         Speaker.Instance.OnSpeakComplete -= speakComplete;
      }
   }

   public void Speak(string text, string whoSpeaks)
   {
        playing = true;
        uid = Speaker.Instance.Speak(text, null, Speaker.Instance.VoiceForCulture(Culture), true, 1.1f, 0.1f); //Speak with the first voice matching the given culture
        StartCoroutine(ShowText(text, whoSpeaks));
   }

    public void Silence()
    {
        Speaker.Instance.Silence();        
    }

    public void Pause()
    {
        Speaker.Instance.Pause();
    }

    public void UnPause()
    {
        Speaker.Instance.UnPause();
    }

    private void voicesReady()
    {
      //Debug.Log($"RT-Voice: {Speaker.Instance.Voices.Count} voices are ready to use!");

      //if (SpeakWhenReady) //Speak after the voices are ready
         //Speak();
    }

   private void speakStart(Wrapper wrapper)
   {
        playing = true;

      //if (wrapper.Uid == uid) //Only write the log message if it's "our" speech
         //Debug.Log($"RT-Voice: speak started: {wrapper}");

        if (!textPanelTransform.gameObject.activeSelf)
            textPanelTransform.gameObject.SetActive(true);
   }

   private void speakComplete(Wrapper wrapper)
   {
        playing = false;

        //if (wrapper.Uid == uid) //Only write the log message if it's "our" speech
        // Debug.Log($"RT-Voice: speak completed: {wrapper}");

        StartCoroutine(HideText());
   }

    IEnumerator HideText()
    {
        yield return  new WaitForSeconds(0.5f);
        if (textPanelTransform.gameObject.activeSelf)
            textPanelTransform.gameObject.SetActive(false);
    }

    IEnumerator ShowText(string text, string whoSpeaks)
    {
        string showText = text;
        string showLetters = "";
        for(int i = 0; i < showText.Length; i++)
        {  
            showLetters += showText[i];
            dialogText.text = whoSpeaks + ": " + showLetters;
            yield return new WaitForSeconds(0.045f);
        }
        
    }
}
// © 2022 crosstales LLC (https://www.crosstales.com)