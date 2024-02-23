using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUDSystem : MonoBehaviour
{
    public Image healthBar;

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        healthBar.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }
}
