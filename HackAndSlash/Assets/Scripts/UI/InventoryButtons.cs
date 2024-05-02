using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButtons : MonoBehaviour
{
    [SerializeField] private UIInventoryPage _inventoryUI;
    public GameObject loadBarGameObject;
    public Image loadBarFill;
    public void Continue()
    {
        _inventoryUI.Hide();
        GameManager.Instance.UnPauseMenuGame();
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void MainMenu() => ActiveLoadBarGameObject();

    private void ActiveLoadBarGameObject()
    {
        loadBarGameObject.SetActive(true);
        Invoke(nameof(GoToScene), 1f);
    }

    private void GoToScene()
    {
        GameManager.Instance.LoadLevel(Constants.SCENE_MAINMENU, loadBarFill);
    }
}
