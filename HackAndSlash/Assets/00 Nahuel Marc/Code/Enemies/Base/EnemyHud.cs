using UnityEngine;
using UnityEngine.UI;

public class EnemyHud : MonoBehaviour
{
    public Slider healthBar;
    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.value = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }
}
