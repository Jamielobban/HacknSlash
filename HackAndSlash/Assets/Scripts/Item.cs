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
