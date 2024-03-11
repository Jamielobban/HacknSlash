using UnityEngine;
using UnityEngine.UI;

public class DeadOptions : MonoBehaviour, IInteractable
{
    public string sceneName;
    public GameObject loadBarGameObject;
    public Image loadBarFill;
    public void Interact()
    {
        loadBarGameObject.SetActive(true);
        Invoke(nameof(GoToScene), 1f);
    }

    private void GoToScene()
    {
        GameManager.Instance.LoadLevel(sceneName, loadBarFill);

    }
}
