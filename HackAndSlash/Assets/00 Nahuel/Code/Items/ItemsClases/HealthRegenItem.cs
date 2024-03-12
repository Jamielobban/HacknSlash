using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthRegenItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Regenerates Heal every 5 seconds.";
    }
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.healthRegen += data.value;
    }
}
