using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.transform.DOMoveX(3.79f, 14).SetEase(Ease.InOutSine);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Animator>(out Animator anim))
        {
            anim.CrossFadeInFixedTime("DoorOpen", 0.2f);
        }
    }
}
