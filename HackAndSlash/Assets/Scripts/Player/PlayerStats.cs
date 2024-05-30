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
    public TextMeshProUGUI currentExtraXp;

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
        critChanceText.text = "Crit chance:  " + playerStats.critChance.ToString("F2") + "%";
        critDamMultiText.text = "Crit Dmg:  " + playerStats.critDamageMultiplier.ToString("F2");
        attackDamageText.text = "Attack Dmg:  " + playerStats.attackDamage.ToString("F2");
        healthRegenText.text = "Regeneration:  " + playerStats.healthRegen.ToString("F2") + "  / 5s";
        lifeSteal.text = "Life Steal: " + playerStats.lifeStealPercent.ToString("F2") + "%";
        currentLife.text = "" + playerStats.healthSystem.CurrentHealth.ToString("F2") + " / " + playerStats.healthSystem.maxHealth.ToString("F0") + " HP ";
        currentExtraXp.text = "XP Multiplier: " + playerStats.xpPercentageExtra.ToString("F2");
    }
}
