using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;

    public static AudioManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Audio Manager");
                go.AddComponent<AudioManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlayAudio(AudioSource audioSource, AudioClip audioToPlay)
    {
        audioSource.clip = audioToPlay;
        audioSource.Play();
    }

    public void PlayAudioWithDelay(AudioSource audioSource, AudioClip audioToPlay, float delayInSeconds)
    {
        StartCoroutine(PlayWithDelayCoroutine(audioSource, audioToPlay, delayInSeconds));
    }

    public void PlayAudioOnLoop(AudioSource audioSource, AudioClip audioToPlay, float loopDelayInSeconds)
    {
        StartCoroutine(PlayOnLoopCoroutine(audioSource, new AudioClip[] { audioToPlay }, loopDelayInSeconds));
    }

    public IEnumerator PlayWithDelayCoroutine(AudioSource audioSource, AudioClip audioToPlay, float delayInSeconds)
    {
        yield return new WaitForSeconds(delayInSeconds);
        PlayAudio(audioSource, audioToPlay);
    }

    public IEnumerator PlayOnLoopCoroutine(AudioSource audioSource, AudioClip[] audioClips, float loopDelayInSeconds)
    {
        while (true)
        {
            if (audioClips != null && audioClips.Length > 0)
            {
                int randomIndex = Random.Range(0, audioClips.Length);
                audioSource.clip = audioClips[randomIndex];
                audioSource.Play();
                Debug.Log("Playing audio: " + audioSource.clip.name);

                yield return new WaitForSeconds(audioSource.clip.length + loopDelayInSeconds);
            }
            else
            {
                Debug.LogError("No audio clips provided for loop!");
                yield break; // Break the loop if the audioClips array is null or empty
            }
        }
    }
    public void StopAudioLoop(AudioSource audioSource)
    {
        StopCoroutine(PlayOnLoopCoroutine(audioSource, null, 0f));
        audioSource.Stop();
    }
}
