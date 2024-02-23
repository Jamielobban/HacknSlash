using UnityEngine;
using UnityEngine.UI;

public class EnemyHud : MonoBehaviour
{
    public Image healthBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }
}
