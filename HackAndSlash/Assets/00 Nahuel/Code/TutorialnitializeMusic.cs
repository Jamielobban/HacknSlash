using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialnitializeMusic : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.TutorialMusic); 
    }


}
