using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashWave : Item
{
    public GameObject dashEffects;
    public override void OnItemPickup(PlayerControl player)
    {

        ItemManager manager = FindObjectOfType<ItemManager>();

        if (manager.actionItems.Contains(this))
        {
            manager.actionItems.Remove(this);
        }
        player.OnDash += DoDash;

    }

    void DoDash()
    {
        for(int i = 0; i  < dashEffects.transform.childCount; i++)
        {
            if (!dashEffects.transform.GetChild(i).gameObject.activeSelf)
            {
                dashEffects.transform.GetChild(i).gameObject.SetActive(true);
                return;
            }

        }

    }

}
