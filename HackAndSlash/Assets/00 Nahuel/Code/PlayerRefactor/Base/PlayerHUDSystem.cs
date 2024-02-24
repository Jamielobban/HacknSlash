using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDSystem : MonoBehaviour
{
    public Image progressBar;
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        int propId = Shader.PropertyToID("_ProgressBar");
        progressBar.material.SetFloat(propId, Mathf.Clamp(currentHealth / maxHealth, 0, 1));
    }
}
