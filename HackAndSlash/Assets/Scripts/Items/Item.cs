using UnityEngine;

[System.Serializable]
public abstract class Item : MonoBehaviour
{
    public ItemData data;
    
    protected string _defaultDescription;
    public string DefaultDescription => _defaultDescription;
    public virtual string GetName() => data.itemName;
    public virtual string GetDescription() => data.itemDescription;
    public virtual Sprite GetSprite() => data.itemIcon;
    public virtual Enums.RarityType GetRarity() => data.rarityType;

    public virtual void OnHit(PlayerControl player, int stacks)
    {

    }

    public virtual void OnJump(PlayerControl player, int stacks)
    {

    }

    public virtual void OnItemPickup(PlayerControl player)
    {

    }
}