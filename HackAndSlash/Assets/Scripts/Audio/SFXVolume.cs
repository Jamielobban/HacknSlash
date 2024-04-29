using FMOD.Studio;
using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static Enums;

public class SFXVolume : MonoBehaviour
{
    private Slider _slider;
    public AudioMixer mixer;
    private Bus sfx;

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

        sfx = RuntimeManager.GetBus("bus:/Reverb");
    }
    public void ValueChangeCheck()
    {
        if (_slider != null)
        {
            sfx.setVolume(_slider.value);

        }
    }
    private void OnDisable()
    {
    }
}
