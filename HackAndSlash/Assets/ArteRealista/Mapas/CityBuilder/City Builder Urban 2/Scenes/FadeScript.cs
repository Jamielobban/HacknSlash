using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FadeScript
{
    // Start is called before the first frame update
    Image blackImage;  

    public FadeScript(Image _blackIMage) { blackImage = _blackIMage; } 

    public void FadeIn(float fadeInTime) => DOTween.To(() => blackImage.color.a, x => blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, x), 0f, fadeInTime).SetEase(Ease.Linear);

    public void FadeOut(float fadeInTime) => DOTween.To(() => blackImage.color.a, x => blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, x), 1f, fadeInTime).SetEase(Ease.Linear);
        
}
