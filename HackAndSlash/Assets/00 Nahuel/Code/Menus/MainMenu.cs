using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : BaseMenu<MainMenu>
{
    public GameObject loadingInspector;
    public Image loadingFillAmountInspector;
    protected override void Awake()
    {
        base.Awake();
        loadingInspector.SetActive(false);
    }
    public void OnPlayPressed()
    {

      //  GameManager.Instance.LoadLevel("Nagu_MainScene", loadingInspector, loadingFillAmountInspector);
    }

    public void OnSettingsPressed()
    {

        SettingsMenu.Show();
    }

    public override void OnBackPressed()
    {

      //  GameManager.Instance.UpdateState(Enums.GameState.Exit);
    }

}
