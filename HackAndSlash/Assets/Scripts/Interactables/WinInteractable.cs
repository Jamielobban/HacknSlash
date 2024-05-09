using Unity.VisualScripting;
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
                Destroy(enemyPool.gameObject);
            }
        }
        LevelManager.Instance.EnemiesManager.DeleteSpawner();
        GameManager.Instance.Player.SaveData();
        AudioManager.Instance.PlayMusic(Enums.Music.Victory);
        onWinInteracted.SetActive(true);
        onWinInteracted.GetComponent<GDTFadeEffect>()?.StartEffect();
        Invoke(nameof(ActiveLoarBar), 3.5f);

    }
    private void ActiveLoarBar()
    {
        loadBarGameObject.SetActive(true);
        Invoke(nameof(SceneDead), 1);
    }
    private void SceneDead()
    {
        GameManager.Instance.LoadLevel(sceneName, loadBarFill);
    }
}
