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
        _slider.value = GameManager.Instance.volumeSFX;
        mixer.SetFloat("SfxVolume", _slider.value / 27);
        AudioManager.Instance.audioFx.volume = _slider.value / 27;
        AudioManager.Instance.audioFxStopeable.volume = _slider.value / 27;
    }
    public void ValueChangeCheck()
    {
        if (_slider != null)
        {
            mixer.SetFloat("SfxVolume", Mathf.Log10(_slider.value / 27) * 20);
            AudioManager.Instance.audioFx.volume = _slider.value / 27;
            AudioManager.Instance.audioFxStopeable.volume = _slider.value / 27;
            GameManager.Instance.volumeSFX = _slider.value / 27;
        }
    }
    private void OnDisable()
    {
    }
}
