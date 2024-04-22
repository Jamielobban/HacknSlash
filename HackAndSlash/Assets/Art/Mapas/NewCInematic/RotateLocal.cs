using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateLocal : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 localRotationAxis; //0,360,0
    [SerializeField] float period;
    private void Start()
    {
        
        //transform.DOMoveY(startPosition.y + verticaDistance, verticalMoveTime).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * period * Time.deltaTime);
    }
}
