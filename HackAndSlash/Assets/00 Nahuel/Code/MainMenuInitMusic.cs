using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuInitMusic : MonoBehaviour
{
    public float volumenInicial = 0.5f;
    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.MainMenuMusic);
       
    }
}
