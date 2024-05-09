using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LluviaMeteoritos : Item
{
    public override void OnItemPickup(PlayerControl player)
    {
        player.meteoritos.SetActive(true);
    }
}
