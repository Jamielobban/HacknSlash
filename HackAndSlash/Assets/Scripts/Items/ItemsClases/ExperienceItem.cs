using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperienceItem : Item
{
    public override void OnItemPickup(PlayerControl player)
    {
        player.xpPercentageExtra += data.value;
    }
}
