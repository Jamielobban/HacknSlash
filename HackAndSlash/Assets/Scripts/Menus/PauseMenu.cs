using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : BaseMenu<PauseMenu>
{
    protected override void Awake()
    {
        // (!GameManager.Instance.canPause) return;
        base.Awake();
        GameManager.Instance.PauseGame();
    }

    public void OnSettingsMenu()
    {

        Hide();
        SettingsMenu.Show();
    }

    public void OnMainMenu()
    {
        GameManager.Instance.PauseGame();
        Hide();
        SceneManager.LoadScene(0);
    }

    public void OnQuitPressed()
    {
        GameManager.Instance.PauseGame();
        Hide();
        Destroy(gameObject); //This menu doesn't destroy itself
    }
}
