using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enums;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public AudioSource audioFx;
    public AudioSource audioMusic;
    public AudioSource audioMusicEffects;
    public AudioSource audioFxStopeable;

    private AudioClip[] _arrayFx;
    private AudioClip[] _arrayMusicEffects;
    private AudioClip[] _arrayMusic;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Audio Manager");
                go.AddComponent<AudioManager>();
                _instance = go.GetComponent<AudioManager>();
                DontDestroyOnLoad(go);                
            }
            return _instance;
        }
    }

    private void Awake()
    {
        LoadAllAudios();
        audioMusicEffects.loop = true;
        audioMusic.loop = true;
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadAllAudios()
    {
        _arrayMusic = Resources.LoadAll<AudioClip>(Constants.MUSICDIRECTORIES[0]);
        _arrayFx = Resources.LoadAll<AudioClip>(Constants.MUSICDIRECTORIES[1]);
        _arrayMusicEffects = Resources.LoadAll<AudioClip>(Constants.MUSICDIRECTORIES[2]);
    }
    public void PlayMusic(Enums.Music music)
    {
        if (audioMusic.clip != null)
        {
            CrossFade(_arrayMusic[(int)music], audioMusic, 1);
        }
        else
        {
            audioMusic.clip = _arrayMusic[(int)music];
            audioMusic.Play();
        }
        audioMusic.loop = true;
    }
    public void PlayMusicEffect(Enums.MusicEffects musicEffect)
    {
        if (audioMusicEffects != null)
        {
            CrossFade(_arrayMusicEffects[(int)musicEffect], audioMusicEffects, 1);
        }
        else
        {
            audioMusicEffects.volume = 0;
            audioMusicEffects.clip = _arrayMusicEffects[(int)musicEffect];
            audioMusicEffects.Play();
            FadeMusicEffect(1f);
        }
        audioMusicEffects.loop = true;
    }

    public void FadeMusicEffect(float volume)
    {
        Fade(audioMusicEffects, 1f, volume);
    }

    public void FadeMusic(float duration, float volume)
    {
        Fade(audioMusic, duration, volume);
    }

    private void Fade(AudioSource source, float duration, float volume)
    {
        StartCoroutine(StartFade(source, duration, volume));
    }
    IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }
    public void CrossFade(AudioClip clip, AudioSource source, float volume)
    {
        StartCoroutine(FadeItMusic(clip, source, volume));
    }
    IEnumerator FadeItMusic(AudioClip clip, AudioSource source, float volume)
    {
        ///Add new audiosource and set it to all parameters of original audiosource
        AudioSource fadeOutSource = gameObject.AddComponent<AudioSource>();
        fadeOutSource.clip = source.clip;
        fadeOutSource.time = source.time;
        fadeOutSource.volume = source.volume;
        fadeOutSource.outputAudioMixerGroup = source.outputAudioMixerGroup;

        //make it start playing
        fadeOutSource.Play();

        //set original audiosource volume and clip
        source.volume = 0f;
        source.clip = clip;
        float t = 0;
        float v = fadeOutSource.volume;
        source.Play();

        //begin fading in original audiosource with new clip as we fade out new audiosource with old clip
        while (t < 0.98f)
        {
            t = Mathf.Lerp(t, 1f, Time.deltaTime * 0.35f);
            fadeOutSource.volume = Mathf.Lerp(v, 0f, t);
            source.volume = Mathf.Lerp(0f, volume, t);
            yield return null;
        }
        source.volume = volume;
        //destroy the fading audiosource
        Destroy(fadeOutSource);
        yield break;
    }

    public void PlayFx(Enums.Effects fx)
    {
        audioFxStopeable.Stop();
        audioFx.PlayOneShot(_arrayFx[(int)fx]);
    }

    public void PlayRandomFx(List<AudioClip> _arrayFx)
    {
        if (_arrayFx.Count <= 0)
        {
            return;
        }
        audioFx.Stop();
        audioFx.PlayOneShot(_arrayFx[Random.Range(0, _arrayFx.Count)]);
    }

    public void PlayStopeableFx(Enums.Effects fx)
    {
        audioFxStopeable.Stop();
        audioFxStopeable.PlayOneShot(_arrayFx[(int)fx]);
    }

    public void PlayStopeableRandomFx(List<AudioClip> _arrayFx)
    {
        if (_arrayFx.Count <= 0)
        {
            return;
        }
        audioFxStopeable.Stop();
        audioFxStopeable.PlayOneShot(_arrayFx[Random.Range(0, _arrayFx.Count)]);
    }

    IEnumerator DelayTime(float delay, System.Action onTimeDelayed)
    {
        yield return new  WaitForSeconds(delay);
        onTimeDelayed();
    }

    public void PlayDelayFx(Enums.Effects fx, float delayTime) =>  StartCoroutine(DelayTime(delayTime, () => { PlayFx(fx); }));
    public void PlayDelayMusic(Enums.Music music, float delayTime) => StartCoroutine(DelayTime(delayTime, () => { PlayMusic(music); }));
    public void PlayDelayMusicFX(Enums.MusicEffects musicFX, float delayTime) => StartCoroutine(DelayTime(delayTime, () => { PlayMusicEffect(musicFX); }));
    public void PlayDelayStoppeableFX(Enums.Effects fx, float delayTime) => StartCoroutine(DelayTime(delayTime, () => { PlayStopeableFx(fx); }));

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }
}
