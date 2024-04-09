using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextMeshProFadeObject : MonoBehaviour
{
    private TextMeshProUGUI _text;
    public float fadeDuration = 1f;
    public float timeToFadeOut = 2f;

    private void Awake()
    {
        _text = GetComponent<TextMeshProUGUI>();
    }

    public void FadeIn()
    {
        StartCoroutine(FadeText(0f, 1f, fadeDuration));
        Invoke("FadeOut", timeToFadeOut);
    }

    public void FadeOut() => StartCoroutine(FadeText(1f, 0f, fadeDuration));

    private IEnumerator FadeText(float startAlpha, float targetAlpha, float duration)
    {
        float currentTime = 0f;

        while (currentTime < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, currentTime / duration);
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, alpha);
            currentTime += Time.deltaTime;
            yield return null;
        }

        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, targetAlpha);
    }
}
