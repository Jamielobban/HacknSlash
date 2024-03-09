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
    }
    public void OnPlayPressed()
    {
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Click);
        mainMenuContent.SetActive(false);
        loadingInspector.SetActive(true);
        Invoke(nameof(ActiveScene), .07f);
    }

    private void ActiveScene()
    {
        if (GameManager.Instance.isTutorialCompleted)
        {
            GameManager.Instance.LoadLevel("DanielIceMap ", loadingFillAmountInspector);
        }
        else
        {
            GameManager.Instance.LoadLevel("01 Cinematic", loadingFillAmountInspector);
        }
    }

    public void OnSettingsPressed()
    {
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Click);
        // SettingsMenu.Show();
    }

    public override void OnBackPressed()
    {
        AudioManager.Instance.PlayMusicEffect(Enums.MusicEffects.Click);
        GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }

}
