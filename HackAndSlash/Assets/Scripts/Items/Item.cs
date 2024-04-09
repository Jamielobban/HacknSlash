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

    // public virtual void OnKill(PlayerControl player, Enemy enemy, int stacks)
    // {
    //
    // }
    //
    // public virtual void OnCrit(PlayerControl player, Enemy enemy, int stacks)
    // {
    //
    // }

    public virtual void OnItemPickup(PlayerControl player)
    {

    }
}