public class AbilityItem : Item
{

    public PlayerControl.Ataques ability;

    private void Awake()
    {
        _defaultDescription = "";
    }

    public override void OnItemPickup(PlayerControl player)
    {
        ability.spriteAbility = data.itemIcon;
        GameManager.Instance.Player._abilityHolder.AddAbility(data.ìnput, this);
    }
}
