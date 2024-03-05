using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Crosstales.RTVoice;
using TMPro;
using static Unity.VisualScripting.Member;

public class SpeakExample : MonoBehaviour
{
    public TMP_InputField Text;
    public AudioSource source;

   
    public void Speak()
    {
        Speaker.Instance.Speak(Text.text, source, Speaker.Instance.VoiceForGender(Crosstales.RTVoice.Model.Enum.Gender.MALE, "us"), false, 1.5f);
        // Speaker.Speak(Text.text, null, Speaker.VoiceForName(VoiceName));
    }
}
