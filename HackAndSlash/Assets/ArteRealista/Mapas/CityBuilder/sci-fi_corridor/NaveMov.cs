using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class NaveMov : MonoBehaviour
{
    public float velocity;
    public Image blackImage;
    public float fadeInDuration;
    public float fadeOutDuration;
    public float timeToFadeOut;
    public SimpleRTVoiceExample voice;
    // Start is called before the first frame update
    void Start()
    {
        blackImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());
        StartCoroutine(FadeOut());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position = this.transform.position + new Vector3(velocity * Time.deltaTime, 0,0);
        //Debug.Log(Time.time);
    }

    IEnumerator FadeIn()
    {
        yield return new WaitForSeconds(0.3f);
        DOTween.To(() => blackImage.color.a, x => blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, x), 0f, fadeInDuration).SetEase(Ease.Linear);        

    }
    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(timeToFadeOut);
        DOTween.To(() => blackImage.color.a, x => blackImage.color = new Color(blackImage.color.r, blackImage.color.g, blackImage.color.b, x), 1f, fadeInDuration).SetEase(Ease.Linear);        
        yield return new WaitForSeconds(fadeInDuration + 0.3f);
        SceneManager.LoadScene(1);

    }
}
