using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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


    public virtual void Update(PlayerInventory player, int stacks)
    {

    }

    public virtual void OnHit(PlayerInventory player, int stacks)
    {

    }

    public virtual void OnJump(PlayerInventory player, int stacks)
    {

    }

    public virtual void OnKill(PlayerInventory player, Enemy enemy, int stacks)
    {

    }

    public virtual void OnCrit(PlayerInventory player, Enemy enemy, int stacks)
    {

    }

    public virtual void OnItemPickup(PlayerManager player, int stacks, StatType statType)
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
    DoubleHit,
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
        return "Adds 2% crit chance per stack. Crits deal double damage.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Glasses");
    }
    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        //if (statType == StatType.CritChance)
        //{
        //    player.critChance += 2;
        //}
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
        return "Adds 2% crit chance damage per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Glasses");
    }
    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        //if (statType == StatType.CritDamage)
        //{
        //    player.critDamageMultiplier += 2;
        //}
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

        return "Gain 20% attack damage per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Damage");
    }
    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        if (statType == StatType.Damage)
        {
            player.stats.baseDamage += 0.2f;
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
        return "Adds 5 max health per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Meat");
    }

    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        //if (statType == StatType.MaxHealth)
        //{
        //    player.maxHealth += 5 + (1 * stacks);
        //    player.SetHealth();
        //}
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
        return "Adds 40 health per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Meat");
    }

    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        //if (statType == StatType.Health)
        //{
        //    if(player.currentHealth < player.maxHealth)
        //    {
        //        player.currentHealth += 40;
        //        player.SetHealth();
        //    }
        //}
    }
}


//On hit heal

public class MonsterTooth : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Rare;
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
        return "When striking an enemy heal 3 hp per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Tooth");
    }

    public override void OnHit(PlayerInventory player, int stacks)
    {
        //int heal = 3;
        //player.currentHealth += heal;

        //player.healPixel.Spawn(player.transform.position + new Vector3(0f,2f,0f), heal);
        //player.SetHealth();
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

public class Slashes : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Legendary;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.None;
    }
    public override string GiveName()
    {
        return "Slashes";
    }

    public override string GiveDescription()
    {
        return "Your heavy attacks now launch long-range slashes";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }

    //public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    //{
    //    if (statType == StatType.None)
    //    {
    //        if (player.currentHealth < player.maxHealth)
    //        {
    //            player.currentHealth += 40;
    //            player.SetHealth();
    //        }
    //    }
    //}

}

public class DoubleHit : Item
{
    public override RarityType GetRarity()
    {
        return RarityType.Common;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.DoubleHit;
    }
    public override string GiveName()
    {
        return "Double Hit Square";
    }

    public override string GiveDescription()
    {
        return "Your light attacks now hit twice.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/Gasoline");
    }
    public override void OnItemPickup(PlayerManager player, int stacks, StatType statType)
    {
        //if (statType == StatType.DoubleHit)
        //{
        //    Debug.Log("added");
        //    for (int i = 0; i < player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks.Length; i++)
        //    {
        //        player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks[i].repeticionGolpes = 1;
        //        player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks[i].delayRepeticionGolpes = 0.15f;
        //    }
        //}
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

    public override void Update(PlayerInventory player, int stacks)
    {
        //player.maxHealth += 3 + (2 * stacks);
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

    public override void OnHit(PlayerInventory player, int stacks)
    {
        //enemy.health -= 3 * stacks;
    }


    public int GetDamage(PlayerControl player, int stacks)
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
    public override void Update(PlayerInventory player, int stacks)
    {
        internalCooldown -= 1;
    }
    public override void OnJump(PlayerInventory player, int stacks)
    {
        if (internalCooldown <= 0)
        {
            if (effect == null) effect = (GameObject)Resources.Load("Item Effects/Something", typeof(GameObject));

            GameObject healingArea = GameObject.Instantiate(effect, player.transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.Euler(Vector3.zero));
            internalCooldown = 1f;
        }
    }
}
