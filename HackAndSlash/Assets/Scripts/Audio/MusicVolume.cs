using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MusicVolume : MonoBehaviour
{
    private Slider _slider;
    public AudioMixer mixer;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = GameManager.Instance.volumeMusic;        
    }

    private void Start()
    {
        if (_slider == null)
        {
            return;
        }
        mixer.SetFloat("MusicVolume", _slider.value / 27);
        //_slider.value = GameManager.Instance.sfxVolume;
        AudioManager.Instance.audioMusic.volume = _slider.value / 27;
        AudioManager.Instance.audioMusicEffects.volume = _slider.value / 27;
    }
    public void ValueChangeCheck()
    {
        if (_slider != null)
        {
            mixer.SetFloat("MusicVolume", Mathf.Log10(_slider.value / 27) * 20);
            AudioManager.Instance.audioMusic.volume = _slider.value / 27;
            AudioManager.Instance.audioMusicEffects.volume = _slider.value / 27;
            GameManager.Instance.volumeMusic = _slider.value;
        }
    }
    private void OnDisable()
    {
       // GameManager.Instance.sfxVolume = _slider.value;
    }
}
