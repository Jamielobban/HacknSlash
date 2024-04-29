using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    private Slider _slider;
    public AudioMixer mixer;
    private Bus music;



    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = 1f;        
    }

    private void Start()
    {
        if (_slider == null)
        {
            return;
        }

        _slider.value = 1;

        music = RuntimeManager.GetBus("bus:/Music");

    }
    public void ValueChangeCheck()
    {
        if (_slider != null)
        {
            music.setVolume(_slider.value);
        }
    }
    private void OnDisable()
    {
       // GameManager.Instance.sfxVolume = _slider.value;
    }
}
