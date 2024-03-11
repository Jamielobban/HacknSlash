using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;


public class EnemySounds : MonoBehaviour
{
    public List<AudioClip> randomSoundsAround= new List<AudioClip>();
    public AudioSource randomSound;
    private float _timer = 0f;
    public float timeToRandomSound = 4f;

    
    void Start()
    {
        
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
