using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CamMovement : MonoBehaviour
{
    Sequence sequence;
    // Start is called before the first frame update
    void Start()
    {
        Tween moveTween = this.transform.DOMoveX(15.46f, 20).SetEase(Ease.InOutSine); //3.79f
        Tween moveTween2 = this.transform.DOMoveX(15.46f, 6).SetEase(Ease.InOutSine);
        Tween rotLeftTween = this.transform.DORotate(new Vector3(0,45,0), 4).SetEase(Ease.InOutSine);
        Tween rotRightTween = this.transform.DORotate(new Vector3(0, 135, 0), 4).SetEase(Ease.InOutSine);
        Tween rotStraightTween = this.transform.DORotate(new Vector3(0, 90, 0), 4).SetEase(Ease.InOutSine);

        sequence = DOTween.Sequence();
        sequence.Append(moveTween)/*.Append(rotLeftTween).Append(rotRightTween).Append(rotStraightTween).Append(moveTween2)*/;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<Animator>(out Animator anim))
        {
            anim.CrossFadeInFixedTime("DoorOpen", 0.2f);
        }
    }
}
