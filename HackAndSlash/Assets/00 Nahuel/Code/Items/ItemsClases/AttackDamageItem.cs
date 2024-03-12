using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDamageItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Increases the attack damage by percentage.";
    }
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.attackDamage += data.value;
    }
}
