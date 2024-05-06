using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinInteractable : MonoBehaviour, IInteractable
{
    public string sceneName;
    public GameObject onWinInteracted;
    public GameObject loadBarGameObject;
    public Image loadBarFill;
    public GameObject[] canvasToDeactivate;

    public void Interact()
    {
        foreach (var canvas in canvasToDeactivate)
        {
            canvas.SetActive(false);
        }
        if(LevelManager.Instance.EnemiesManager != null)
        {
            foreach (var enemyPool in LevelManager.Instance.EnemiesManager.parentObjectPools)
            {
                enemyPool.SetActive(false);
            }
        }

        AudioManager.Instance.PlayMusic(Enums.Music.Victory);
        onWinInteracted.SetActive(true);
        onWinInteracted.GetComponent<GDTFadeEffect>()?.StartEffect();
        Invoke(nameof(ActiveLoadBarGameObject), 3.5f);
    }

    private void ActiveLoadBarGameObject()
    {
        loadBarGameObject.SetActive(true);
        Invoke(nameof(GoToScene), 1f);
    }

    private void GoToScene()
    {
        GameManager.Instance.LoadLevel(sceneName, loadBarFill);
    }
}
