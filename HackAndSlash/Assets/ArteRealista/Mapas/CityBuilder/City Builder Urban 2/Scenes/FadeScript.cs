using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class FadeScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    Image blackIMage;
    void OnEnable()
    {
        blackIMage.gameObject.SetActive(true);
        Color col = Color.black;
        col.a = 0;
        DOVirtual.Color(blackIMage.color, col, 1.8f, (col) =>
        {
            blackIMage.color = col;
        }).SetEase(Ease.InOutSine);
    }

    public void DoTransition()
    {
        Color col = Color.black;
        DOVirtual.Color(blackIMage.color, col, 0.8f, (col) =>
        {
            blackIMage.color = col;
        }).SetEase(Ease.InOutSine);

        StartCoroutine(ChangeScene());
    }

    IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(0.9f);
        SceneManager.LoadScene(2);
    }
}
