using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shock : Item
{
    public GameObject shockEffect;
    public override void OnItemPickup(PlayerControl player)
    {
        ItemManager manager = FindObjectOfType<ItemManager>();

        if (manager.actionItems.Contains(this))
        {
            manager.actionItems.Remove(this);
        }
        Invoke("EnableShock", 5);
    }

    void EnableShock()
    {
        shockEffect.SetActive(true);
        Invoke("EnableShock", 5);

    }

}
