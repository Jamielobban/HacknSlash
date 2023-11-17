using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceEffect : MonoBehaviour
{
    private void Start()
    {
        Bounce();  
        Rotate();
        
    }
    private void Update()
    {
    }

    void Bounce()
    {
        // Get the current position of the coin
        Vector3 startPosition = transform.position;

        // Calculate the end position (bounced position)
        Vector3 endPosition = startPosition + Vector3.up * 0.2f;

        // Use DOTween to create the bounce animation
        transform.DOMove(endPosition, 3 / 2f).SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                // Reverse the direction to create the bounce effect
                transform.DOMove(startPosition, 3 / 2f).SetEase(Ease.InQuad)
                    .OnComplete(Bounce); // Repeat the bounce
            });
    }
    private void Rotate()
    {
        transform.DORotate(new Vector3(0f, 0f, 360f), 3, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1); // Infinite loop for continuous rotation
    }
}
