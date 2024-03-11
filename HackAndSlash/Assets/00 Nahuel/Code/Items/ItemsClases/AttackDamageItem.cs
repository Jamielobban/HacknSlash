using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.attackDamage += data.value;
    }
}
