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
        GameManager.Instance.UnPauseMenuGame();
        loadBarGameObject.SetActive(true);
        Invoke(nameof(Active), 1f);
    }
    private void Active()
    {
        GameManager.Instance.LoadLevel(Constants.SCENE_MAINMENU, loadBarFill);
    }
}
