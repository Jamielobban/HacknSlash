using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CriticalDamageItem : Item
{
    private void Awake()
    {
        _defaultDescription = "The damage multiplier will increase when you cause critical damage.";
    }


    public override void OnItemPickup(PlayerControl player)
    {
        player.critDamageMultiplier += data.value;
    }
}
