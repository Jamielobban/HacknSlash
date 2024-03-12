using UnityEngine;

[System.Serializable]
public abstract class Item : MonoBehaviour
{
    public ItemData data;
    protected string _defaultDescription;

    public string DefaultDescription => _defaultDescription;
    public virtual string GiveName() => data.itemName;
    public virtual string GiveDescription() => data.itemDescription;
    public virtual Sprite GiveSprite() => data.itemIcon;

    public virtual Enums.RarityType GetRarity() => data.rarityType;

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

    public virtual void OnItemPickup(PlayerControl player, int stacks)
    {

    }

    public float CalculateStatIncrease(float baseIncrease, int stacks, float maxStat)
    {
        return (baseIncrease + (2 * stacks)) / maxStat;
    }

}


//public class MonsterTooth : Item
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
//        return "Monster tooth";
//    }

//    public override string GiveDescription()
//    {
//        return "When striking an enemy heal 3 hp per stack.";
//    }

//    public override Sprite GiveSprite()
//    {
//        return Resources.Load<Sprite>("Item Images/HealAttacking");
//    }

//    public override void OnHit(PlayerControl player, int stacks)
//    {
//        int heal = 3;
//        player.healthSystem.Heal(heal);

//        //player.SetHealth();
//    }
//}


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