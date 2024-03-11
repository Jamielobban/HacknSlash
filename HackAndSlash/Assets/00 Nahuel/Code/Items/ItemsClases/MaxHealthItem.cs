using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxHealthItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.healthSystem.maxHealth += 5 + (1 * stacks);
        player.hud.UpdateHealthBar(player.healthSystem.CurrentHealth, player.healthSystem.maxHealth);
    }
}
