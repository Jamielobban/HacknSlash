using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CineamticController : MonoBehaviour
{
    [SerializeField] VideoPlayer vPlayer;
    [SerializeField] VideoClip menuLoopClip;
    // Start is called before the first frame update
    void Start()
    {
        vPlayer.loopPointReached += StartMenuLoop;
    }

    void StartMenuLoop(VideoPlayer vp)
    {
        vPlayer.loopPointReached -= StartMenuLoop;
        vPlayer.clip = menuLoopClip;
        vPlayer.isLooping = true;        
    }

    IEnumerator StartMenuLoopVideo()
    {
        yield return null;
        vPlayer.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
