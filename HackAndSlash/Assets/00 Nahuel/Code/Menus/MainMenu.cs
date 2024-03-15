using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu<MainMenu>
{
    public GameObject loadingInspector;
    public GameObject mainMenuContent;
    public Image loadingFillAmountInspector;
    protected override void Awake()
    {
        base.Awake();
        loadingInspector.SetActive(false);
        mainMenuContent.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void OnPlayPressed()
    {
        AudioManager.Instance.PlayFx(Enums.Effects.Click);
        mainMenuContent.SetActive(false);
        loadingInspector.SetActive(true);
        Invoke(nameof(ActiveScene), 1f);
    }

    private void ActiveScene()
    {
        AudioManager.Instance.FadeMusic(1f, 0f);
        if (GameManager.Instance.isTutorialCompleted)
        {
            GameManager.Instance.UpdateState(Enums.GameState.Playing);
            GameManager.Instance.LoadLevel(Constants.SCENE_MAIN, loadingFillAmountInspector);
        }
        else
        {
            GameManager.Instance.UpdateState(Enums.GameState.Tutorial);
            GameManager.Instance.LoadLevel(Constants.SCENE_CINEMATIC, loadingFillAmountInspector);
        }
    }

    public void OnSettingsPressed()
    {
        AudioManager.Instance.PlayFx(Enums.Effects.Click);
        // SettingsMenu.Show();
    }

    public override void OnBackPressed()
    {
        AudioManager.Instance.PlayFx(Enums.Effects.Click);
        GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }

}
