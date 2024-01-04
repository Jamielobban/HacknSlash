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

    public abstract RarityType GetRarity();


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
    CritDamage,
    None
}

public enum RarityType
{
    Common,
    Uncommon,
    Rare,
    Legendary
}

//Gives crit chance
public class CritItem : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Uncommon;
    }
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
        return "Adds 5% crit chance per stack. Crits deal double damage.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Glasses");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.CritChance)
        {
            player.critChance += 5;
        }
    }
}


//Gives crit damage
public class CritDamageItem : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Legendary;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.CritDamage;
    }
    public override string GiveName()
    {
        return "Crit Damage";
    }

    public override string GiveDescription()
    {
        return "Adds 5% crit chance damage per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Glasses");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.CritDamage)
        {
            Debug.Log("Hello");
            player.critDamageMultiplier += 5;
        }
    }
}

//Gives attack damage
public class AttackDamge : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.Damage)
        {
            player.attackDamage += 0.2f + (stacks * 0.05f);
        }
    }
}

//Gives max health
public class Meat : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Uncommon;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.MaxHealth;
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
        if (statType == StatType.MaxHealth)
        {
            player.maxHealth += 5 + (1 * stacks);
            player.SetHealth();
        }
    }
}

//Gives current health
public class BoosterShot : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Rare;
    }

    public override StatType GetAssociatedStatType()
    {
        return StatType.Health;
    }
    public override string GiveName()
    {
        return "BoosterShot";
    }

    public override string GiveDescription()
    {
        return "Adds 10 health per stack.";
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
            player.SetHealth();
        }
    }
}


//On hit heal

public class MonsterTooth : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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

    public override void OnHit(PlayerControl player, EnemySkeletonSword enemy, int stacks)
    {
        base.OnHit(player, enemy, stacks);
    }
}

public class Gasoline : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
        player.maxHealth += 3 + (2 * stacks);
    }
}


public class FireDamage : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
        return -3 * stacks;
    }
}

public class HealingArea : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
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
        if (internalCooldown <= 0)
        {
            if (effect == null) effect = (GameObject)Resources.Load("Item Effects/Something", typeof(GameObject));

            GameObject healingArea = GameObject.Instantiate(effect, player.transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.Euler(Vector3.zero));
            internalCooldown = 1f;
        }
    }
}
