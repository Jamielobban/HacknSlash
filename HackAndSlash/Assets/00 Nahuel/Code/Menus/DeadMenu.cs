using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadMenu : BaseMenu<DeadMenu>
{
    public void HideMenu()
    {
        Hide();
    }

    public void OnMainMenuReturn()
    {
      //  SceneManager.LoadScene(0);
    }
    public override void OnBackPressed()
    {

        Hide();
    }
}
