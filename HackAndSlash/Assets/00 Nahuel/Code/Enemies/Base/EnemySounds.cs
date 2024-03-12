using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Unity.VisualScripting.Member;


public class EnemySounds : MonoBehaviour
{
    public List<AudioClip> randomSoundsAround= new List<AudioClip>();
    public AudioSource randomSound;
    public List<AudioClip> randomDeadSounds = new List<AudioClip>();
    public AudioSource deadSound3D;
    private float _timer = 0f;
    public float timeToRandomSound = 4f;

    public AudioSource monsterFootStep;
    
    public void PlaySoundDead() 
    {
        deadSound3D.PlayOneShot(randomDeadSounds[Random.Range(0, randomDeadSounds.Count)]);
    }
    
    void Update()
    {
        _timer += Time.deltaTime;
        if(_timer> timeToRandomSound ) 
        {
            randomSound.PlayOneShot(randomSoundsAround[Random.Range(0, randomSoundsAround.Count)]);
            _timer= 0f;
           
        }
    }

    public void FadeFootSteps(float duration)
    {
        StartCoroutine(StartFade(monsterFootStep, duration, 0));
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
        if (audioSource.volume <= 0.1f)
        {
            audioSource.Stop();
        }
        yield break;
    }
}
