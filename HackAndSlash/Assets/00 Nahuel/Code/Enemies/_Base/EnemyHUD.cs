using UnityEngine;
using UnityEngine.UI;

public class EnemyHUD
{
    public Image healthBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = Mathf.Clamp01(currentHealth / maxHealth);
    }
}
