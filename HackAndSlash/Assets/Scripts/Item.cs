using UnityEngine;

[System.Serializable]
public abstract class Item
{
    int idItem;

    public abstract string GiveName();
    public abstract string GiveDescription();
    public abstract Sprite GiveSprite();
    public abstract StatType GetAssociatedStatType();

    public abstract Enums.RarityType GetRarity();


    public virtual void Update(PlayerControl player, int stacks)
    {

    }

    public virtual void OnHit(PlayerControl player, int stacks)
    {

    }

    public virtual void OnJump(PlayerControl player, int stacks)
    {

    }

    public virtual void OnKill(PlayerControl player, Enemy enemy, int stacks)
    {

    }

    public virtual void OnCrit(PlayerControl player, Enemy enemy, int stacks)
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
    DoubleHit,
    None
}



//Gives crit chance
public class CritItem : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Rare;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.CritChance;
    }
    public override string GiveName()
    {
        return "Critical Aura";
    }

    public override string GiveDescription()
    {
        return "Adds 2% crit chance per stack. Crits deal double damage.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/CriticalPercent");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.CritChance)
        {
            player.critChance += 2;
        }
    }
}


//Gives crit damage
public class CritDamageItem : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Legendary;
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
        return Resources.Load<Sprite>("Item Images/CriticalDamage");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.CritDamage)
        {
            player.critDamageMultiplier += 2;
        }
    }
}

//Gives attack damage
public class AttackDamge : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Uncommon;
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
        return Resources.Load<Sprite>("Item Images/AttackDamage");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.Damage)
        {
            player.attackDamage += 0.2f;
        }
    }
}

//Gives max health
public class MaxHealthItem : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Rare;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.MaxHealth;
    }
    public override string GiveName()
    {
        return "Health";
    }

    public override string GiveDescription()
    {
        return "Adds 5 max health per stack.";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/MaxHp");
    }

    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.MaxHealth)
        {
            player.healthSystem.maxHealth += 5 + (1 * stacks);
            player.hud.UpdateHealthBar(player.healthSystem.CurrentHealth, player.healthSystem.maxHealth);
        }
    }
}

//Gives current health
public class RecoveryFlower : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Common;
    }

    public override StatType GetAssociatedStatType()
    {
        return StatType.Health;
    }
    public override string GiveName()
    {
        return "Recovery";
    }

    public override string GiveDescription()
    {
        return "Heals 40 hp";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/HealLife");
    }

    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.Health)
        {
            player.healthSystem.Heal(+40);
        }
    }
}

public class RegenerationItem : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Rare;
    }
    public override StatType GetAssociatedStatType()
    {
        return StatType.Health;
    }
    public override string GiveName()
    {
        return "Regeneration Amulet";
    }

    public override string GiveDescription()
    {
        return "Heals 2 hp every 5 seconds per stack";
    }

    public override Sprite GiveSprite()
    {
        return Resources.Load<Sprite>("Item Images/HealthRegen");
    }
    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
    {
        if (statType == StatType.Health)
        {
            player.healthRegen += 2;
        }
    }
}

//On hit heal

public class MonsterTooth : Item
{
    public override Enums.RarityType GetRarity()
    {
        return Enums.RarityType.Legendary;
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
        return Resources.Load<Sprite>("Item Images/HealAttacking");
    }

    public override void OnHit(PlayerControl player, int stacks)
    {
        int heal = 3;
        player.healthSystem.Heal(heal);

        //player.SetHealth();
    }
}
#region Not Used 
//public class Gasoline : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }
//    public override string GiveName()
//    {
//        return "Gasoline";
//    }

//    public override string GiveDescription()
//    {
//        return "When killing an enemy spawn a firey circle on the floor that applies burn to all enemies inside it. Stack scales the circle 1.1x";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }
//}

//public class Lighter : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }
//    public override string GiveName()
//    {
//        throw new System.NotImplementedException();
//    }

//    public override string GiveDescription()
//    {
//        return "Hello";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }
//}

//public class Slashes : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Legendary;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }
//    public override string GiveName()
//    {
//        return "Slashes";
//    }

//    public override string GiveDescription()
//    {
//        return "Your heavy attacks now launch long-range slashes";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }

//    //public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
//    //{
//    //    if (statType == StatType.None)
//    //    {
//    //        if (player.currentHealth < player.maxHealth)
//    //        {
//    //            player.currentHealth += 40;
//    //            player.SetHealth();
//    //        }
//    //    }
//    //}

//}

//public class DoubleHit : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.DoubleHit;
//    }
//    public override string GiveName()
//    {
//        return "Double Hit Square";
//    }

//    public override string GiveDescription()
//    {
//        return "Your light attacks now hit twice.";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }
//    public override void OnItemPickup(PlayerControl player, int stacks, StatType statType)
//    {
//        //if (statType == StatType.DoubleHit)
//        //{
//        //    for (int i = 0; i < player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks.Length; i++)
//        //    {
//        //        player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks[i].repeticionGolpes = 1;
//        //        player.GetAttacks(PlayerInventory.ComboAtaques.Quadrat).attacks[i].delayRepeticionGolpes = 0.15f;
//        //    }
//        //}
//    }
//}

//public class HealingItem : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }
//    public override string GiveName()
//    {
//        return "Healing Item";
//    }
//    public override string GiveDescription()
//    {
//        return "Hello";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }

//    public override void Update(PlayerControl player, int stacks)
//    {
//        //player.maxHealth += 3 + (2 * stacks);
//    }
//}


//public class FireDamage : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }
//    public override string GiveName()
//    {
//        return "Fire Damage Item";
//    }

//    public override string GiveDescription()
//    {
//        return "Hello";
//    }
//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }

//    public override void OnHit(PlayerControl player, int stacks)
//    {
//        //enemy.health -= 3 * stacks;
//    }


//    public int GetDamage(PlayerControl player, int stacks)
//    {
//        return -3 * stacks;
//    }
//}

//public class HealingArea : Item
//{
//    public override Enums.RarityType GetRarity()
//    {
//        return Enums.RarityType.Common;
//    }
//    public override StatType GetAssociatedStatType()
//    {
//        return StatType.None;
//    }

//    float internalCooldown;
//    GameObject effect;
//    public override string GiveName()
//    {
//        return "Healing Area";
//    }
//    public override string GiveDescription()
//    {
//        return "Hello";
//    }
//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/Gasoline");
//    }
//    public override void Update(PlayerControl player, int stacks)
//    {
//        internalCooldown -= 1;
//    }
//    public override void OnJump(PlayerControl player, int stacks)
//    {
//        if (internalCooldown <= 0)
//        {
//            if (effect == null) effect = (GameObject)Resources.Load("Item Effects/Something", typeof(GameObject));

//            GameObject healingArea = GameObject.Instantiate(effect, player.transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.Euler(Vector3.zero));
//            internalCooldown = 1f;
//        }
//    }
//}
#endregion