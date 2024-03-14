
using UnityEngine;
using UnityEngine.UI;

public class DebugerHacks : MonoBehaviour
{
    public string sceneName;
    public GameObject loadBarGameObject;
    public Image loadBarFill;
    private bool clicked = false;
    private void Awake()
    {
        clicked = false;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.J) && !clicked)
        {
            clicked = true;
            loadBarGameObject.SetActive(true);
            Invoke(nameof(GoToScene), 1f);
        }
    }

    private void GoToScene()
    {
        GameManager.Instance.LoadLevel(sceneName, loadBarFill);
    }
}
