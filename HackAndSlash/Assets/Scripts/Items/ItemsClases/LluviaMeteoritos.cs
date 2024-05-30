using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LluviaMeteoritos : Item
{
    public override void OnItemPickup(PlayerControl player)
    {
        ItemManager manager = FindObjectOfType<ItemManager>();

        if (manager.actionItems.Contains(this))
        {
            manager.actionItems.Remove(this);
        }
        player.meteoritos.SetActive(true);
    }
}
