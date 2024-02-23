using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsMenu : BaseMenu<SettingsMenu>
{

    public override void OnBackPressed()
    {

        Hide();
        Destroy(gameObject); //This menu doesn't destroy itself
    }
}
