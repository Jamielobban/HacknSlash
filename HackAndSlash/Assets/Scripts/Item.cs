using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    public abstract string GiveName();
    public virtual void Update(PlayerControl player, int stacks)
    {

    }

    public virtual void OnHit(PlayerControl player, Enemy1 enemy, int stacks)
    {

    }

    public virtual void OnJump(PlayerControl player, int stacks)
    {

    }
}
    
public class HealingItem : Item
{
    public override string GiveName()
    {
        return "Healing Item";
    }

    public override void Update(PlayerControl player, int stacks)
    {
        player.health += 3 + (2 * stacks);
    }
}

public class FireDamage : Item
{
    public override string GiveName()
    {
        return "Fire Damage Item";
    }

    public override void OnHit(PlayerControl player, Enemy1 enemy, int stacks)
    {
        enemy.health -= 3 * stacks;
    }

    public int GetDamage(PlayerControl player, Enemy1 enemy, int stacks)
    {
        return  -3 * stacks;
    }
}

public class HealingArea : Item
{
    GameObject effect;
    public override string GiveName()
    {
        return "Healing Area";
    }

    public override void OnJump(PlayerControl player, int stacks)
    {
        if(effect == null) effect = (GameObject)Resources.Load("Item Effects/Something",typeof(GameObject));
    }
}


//public class
    //dodge --> movespeed
    //crowbar
    //gasoline
    //teddy bear
    //bleed
    //Reduce all incoming damage by 5 (+5 per stack). Cannot be reduced below 1.
    //Killing an enemy spawns a healing orb that heals for 8 plus an additional 2% (+2% per stack) of maximum health.
    //2 seconds after getting hurt, heal for 20 plus an additional 5% (+5% per stack) of maximum health.
    //Your attacks have a 10% (+10% per stack) chance to 'Critically Strike', dealing double damage.
    //Killing an enemy ignites all enemies within 12m (+4m per stack) for 150% base damage. Additionally, enemies burn for 150% (+75% per stack) base damage.
    //Increase damage to enemies within 13m by 20% (+20% per stack).
    //Deal +75% (+75% per stack) damage to enemies above 90% health.