using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerStats : MonoBehaviour
{
    [Space]
    [Header("PlayerStats menu components")]
    public GameObject playerStatsMenu;
    public TMP_Text critChanceText;
    public TMP_Text critDamMultiText;
    public TMP_Text attackDamageText;
    public TMP_Text healthRegenText;
    public GameObject healthText;
    private PlayerControl playerStats;

    ControllerManager controller;

    //public GameObject itemPanelGrid;
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindAnyObjectByType<ControllerManager>().GetComponent<ControllerManager>();

        playerStats = FindObjectOfType<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetTabButton().action != null)
        {
            if (controller.GetTabButton().action.WasPressedThisFrame())
            {
                if (playerStatsMenu.activeSelf)
                {
                    playerStatsMenu.SetActive(false);
                    healthText.SetActive(false);
                }
                else
                {
                    playerStatsMenu.SetActive(true);
                    healthText.SetActive(true);
                    RefreshStats();
                }
            }
        }
    }

    public void RefreshStats()
    {
        critChanceText.text = "Crit chance: " + playerStats.critChance.ToString() + "%";
        critDamMultiText.text = "Crit Damage:" + playerStats.critDamageMultiplier.ToString();
        attackDamageText.text = "Attack Damage:" + playerStats.attackDamage.ToString();
        healthRegenText.text = "Health Regen:" + playerStats.healthRegen.ToString();
    }
}
