using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour
{
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
        rightDoor.DOLocalMoveY(0.506f, 1);
        leftDoor.DOLocalMoveY(0.598f, 1);
    }
}
