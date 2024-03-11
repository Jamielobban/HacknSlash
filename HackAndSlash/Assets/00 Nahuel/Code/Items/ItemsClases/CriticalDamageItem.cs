using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.critDamageMultiplier += data.value;
    }
}
