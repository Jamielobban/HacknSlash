using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    [Space]
    [Header("PlayerStats menu components")]
    public GameObject playerStatsMenu;
    public TextMeshProUGUI critChanceText;
    public TextMeshProUGUI critDamMultiText;
    public TextMeshProUGUI attackDamageText;
    public TextMeshProUGUI healthRegenText;
    public TextMeshProUGUI lifeSteal;
    public TextMeshProUGUI currentLife;

    private PlayerControl playerStats;

    void Start()
    {
        playerStats = FindObjectOfType<PlayerControl>();
    }

    void Update()
    {
        if (playerStatsMenu.activeSelf)
        {
            RefreshStats();
        }
    }

    public void RefreshStats()
    {
        critChanceText.text = "Crit chance:  " + playerStats.critChance.ToString() + "%";
        critDamMultiText.text = "Crit Dmg:  " + playerStats.critDamageMultiplier.ToString();
        attackDamageText.text = "Attack Dmg:  " + playerStats.attackDamage.ToString();
        healthRegenText.text = "Regeneration:  " + playerStats.healthRegen.ToString() + "  / 5s";
        lifeSteal.text = "Life Steal: " + playerStats.lifeStealPercent.ToString() + "%";
        currentLife.text = "" + playerStats.healthSystem.CurrentHealth + " / " + playerStats.healthSystem.maxHealth + " HP ";
    }
}
