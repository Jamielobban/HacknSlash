using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinMenu : BaseMenu<WinMenu>
{
    protected override void Awake()
    {
        base.Awake();
    }
    public void HideMenu()
    {
        Hide();
    }

    public void OnMainMenuReturn()
    {
       // SceneManager.LoadScene(0);

        // GameManager.Instance.UpdateState(Enums.GameState.Menu);
    }
    public override void OnBackPressed()
    {

        Hide();
    }
}
