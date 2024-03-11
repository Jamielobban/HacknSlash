using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class EnemySounds : MonoBehaviour
{
    public List<AudioClip> randomSoundsAround= new List<AudioClip>();
    public AudioSource randomSound;
    public List<AudioClip> randomDeadSounds = new List<AudioClip>();
    public AudioSource deadSound3D;
    private float _timer = 0f;
    public float timeToRandomSound = 4f;

    
    void Start()
    {
        
    }
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
}
