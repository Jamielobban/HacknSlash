using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Increases the maximum health available";
    }
    public override void OnItemPickup(PlayerControl player)
    {
        player.healthSystem.maxHealth += data.value;
        player.healthSystem.Heal(data.value);
        player.hud.UpdateHealthBar(player.healthSystem.CurrentHealth, player.healthSystem.maxHealth);
    }
}
