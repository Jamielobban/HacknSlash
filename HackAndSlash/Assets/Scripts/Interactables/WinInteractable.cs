using UnityEngine;
using UnityEngine.UI;

public class WinInteractable : MonoBehaviour
{
    public string sceneName;
    public GameObject onWinInteracted;
    public GameObject loadBarGameObject;
    public Image loadBarFill;
    public GameObject[] canvasToDeactivate;

    public void OnWin()
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
        GameManager.Instance.Player.SaveData();
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
        GameManager.Instance.UpdateState(Enums.GameState.StartPlaying);
        GameManager.Instance.LoadLevel(sceneName, loadBarFill);
    }
}
