using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenuInitMusic : MonoBehaviour
{
    public float volumenInicial = 0.5f;
    public GameObject buttonStart;
    void Start()
    {
        AudioManager.Instance.PlayMusic(Enums.Music.MainMenuMusic);       
    }

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            EventSystem.current.SetSelectedGameObject(buttonStart);
        }
    }
}
