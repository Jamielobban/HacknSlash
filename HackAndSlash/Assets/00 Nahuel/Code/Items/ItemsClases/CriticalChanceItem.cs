using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalChanceItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.critChance += data.value;

    }
}
