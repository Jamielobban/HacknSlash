using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SFXVolume : MonoBehaviour
{
    private Slider _slider;
    public AudioMixer mixer;

    void Awake()
    {
        _slider = GetComponent<Slider>();
        _slider.value = GameManager.Instance.volumeSFX;
    }

    private void Start()
    {
        if (_slider == null)
        {
            return;
        }
        mixer.SetFloat("SfxVolume", _slider.value);
        AudioManager.Instance.audioFx.volume = _slider.value;
        AudioManager.Instance.audioFxStopeable.volume = _slider.value;
    }
    public void ValueChangeCheck()
    {
        if (_slider != null)
        {
            mixer.SetFloat("SfxVolume", Mathf.Log10(_slider.value) * 20);
            AudioManager.Instance.audioFx.volume = _slider.value;
            AudioManager.Instance.audioFxStopeable.volume = _slider.value;
            GameManager.Instance.volumeSFX = _slider.value;
        }
    }
    private void OnDisable()
    {
    }
}
