using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorTrigger : MonoBehaviour
{
    [SerializeField] TutorialManager tm;
    [SerializeField] PlayerControl pc;
    [SerializeField] Transform leftDoor, rightDoor;

    // Start is called before the first frame update    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        pc.playerAnim.CrossFadeInFixedTime("Walk", 0.2f);
        pc.transform.GetChild(0).LookAt(new Vector3(pc.transform.position.x, pc.transform.position.y, 94.7f));
        pc.transform.DOLocalMoveZ(94.7f, 4);
        pc.enabled = false;
        rightDoor.DOLocalMoveY(0.506f, 1);
        leftDoor.DOLocalMoveY(0.598f, 1);
        tm.GetFade.FadeOut(2.5f);
        tm.MuteInSeconds(2.5f);
        tm.EndTutorial();
    }
}
