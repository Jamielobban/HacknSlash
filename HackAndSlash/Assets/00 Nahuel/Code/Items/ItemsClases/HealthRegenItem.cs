using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.healthRegen += data.value;
    }
}
