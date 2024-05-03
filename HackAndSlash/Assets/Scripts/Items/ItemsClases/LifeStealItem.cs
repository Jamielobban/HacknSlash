using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Steals enemies heal every hit.";
    }

    public override void OnItemPickup(PlayerControl player)
    {
        player.lifeStealPercent += data.value;
    }
}
