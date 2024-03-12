using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Increases the chance of critical hit.";
    }

    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.critChance += data.value;

    }
}
