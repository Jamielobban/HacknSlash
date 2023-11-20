using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Item
{
    int idItem;

    public abstract string GiveName();

    public abstract string GiveDescription();
    public abstract Sprite GiveSprite();
    public abstract StatType GetAssociatedStatType();

    public virtual void Update(PlayerControl player, int stacks)
    {

    }

    public virtual void OnHit(PlayerControl player, EnemySkeletonSword enemy, int stacks)
    {

    }

    public virtual void OnJump(PlayerControl player, int stacks)
    {

    }

    public virtual void OnKill(PlayerControl player, EnemySkeletonSword enemy, int stacks)
    {

    }

    public virtual void OnCrit(PlayerControl player, EnemySkeletonSword enemy, int stacks)
    {

    }

    public virtual void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {

    }

    public float CalculateStatIncrease(float baseIncrease, int stacks, float maxStat)
    {
        return (baseIncrease + (2 * stacks)) / maxStat;
    }

}
public enum StatType
{
    Health,
    MaxHealth,
    CritChance,
    Damage,
    None
}
public class CritItem : Item
{

    public override StatType GetAssociatedStatType()
    {
        return StatType.CritChance;
    }
    public override string GiveName()
    {
        return "Crit Glasses";
    }

    public override string GiveDescription()
    {
        return "Adds 5% crit change per stack. Crits deal double damage.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Glasses");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.CritChance)
        {
            player.critChance += CalculateStatIncrease(5, stacks, player.maxCritChance);
        }
    }
}

public class AttackDamge : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.Damage;
    }
    public override string GiveName()
    {
        return "Damage";
    }

    public override string GiveDescription()
    {
        return "Gain 3 attack damage per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Damage");
    }
}

public class Meat : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.Health;
    }
    public override string GiveName()
    {
        return "Meat";
    }

    public override string GiveDescription()
    {
        return "Adds 10 max health per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Meat");
    }

    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.Health)
        {
            player.currentHealth += 5 + (1 * stacks);
        }
    }
}

public class MonsterTooth : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Monster tooth";
    }

    public override string GiveDescription()
    {
        return "When killing an enemy heal 3 hp per stack (+ 1 per stack).";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Tooth");
    }
}

public class Gasoline : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Gasoline";
    }

    public override string GiveDescription()
    {
        return "When killing an enemy spawn a firey circle on the floor that applies burn to all enemies inside it. Stack scales the circle 1.1x";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
}

public class Lighter : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        throw new System.NotImplementedException();
    }

    public override string GiveDescription()
    {
        return "Hello";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
}

public class Icy : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        throw new System.NotImplementedException();
    }

    public override string GiveDescription()
    {
        return "Hello";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
}

public class Feather : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Feather";
    }

    public override string GiveDescription()
    {
        return "Gain 1 extra jump per stack in the air.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
}

public class HealingItem : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Healing Item";
    }
    public override string GiveDescription()
    {
        return "Hello";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }

    public override void Update(PlayerControl player, int stacks)
    {
        player.health += 3 + (2 * stacks);
    }
}


public class FireDamage : Item
{
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Fire Damage Item";
    }

    public override string GiveDescription()
    {
        return "Hello";
    }
    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }

    public override void OnHit(PlayerControl player, EnemySkeletonSword enemy, int stacks)
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
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }

    float internalCooldown;
    GameObject effect;
    public override string GiveName()
    {
        return "Healing Area";
    }
    public override string GiveDescription()
    {
        return "Hello";
    }
    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
    public override void Update(PlayerControl player, int stacks)
    {
        internalCooldown -= 1;
    }
    public override void OnJump(PlayerControl player, int stacks)
    {
        if(internalCooldown <= 0) {
            if(effect == null) effect = (GameObject)Resources.Load("Item Effects/Something",typeof(GameObject));
            GameObject healingArea = GameObject.Instantiate(effect, player.transform.position + new Vector3(0f,0.1f,0f), Quaternion.Euler(Vector3.zero));
            internalCooldown = 1f;
        }
    }
}


//public class
    //crowbar --> 20% mes de daño a enemics que tenen 75% de vida, stack + 5%
    //gasoline --> quan moren surt explosio, stack 1.1 x scale
    //teddy bear --> 5% change to absorb, + 3% stack, logaritmic fins a 50%
    //bleed --> 10% change to inflict bleed, 5% every stack
    //Reduce all incoming damage by 5 (+5 per stack). Cannot be reduced below 1.
    //Killing an enemy spawns a healing orb that heals for 8 plus an additional 2% (+2% per stack) of maximum health.
    //2 seconds after getting hurt, heal for 20 plus an additional 5% (+5% per stack) of maximum health.
    //Your attacks have a 10% (+10% per stack) chance to 'Critically Strike', dealing double damage.
    //Increase damage to enemies within 13m by 20% (+20% per stack).
    //Leech seed