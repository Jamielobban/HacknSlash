using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackOnAir : Item
{

    public override void OnItemPickup(PlayerControl player)
    {
        player.canAttackOnAir = true;
    }
}
