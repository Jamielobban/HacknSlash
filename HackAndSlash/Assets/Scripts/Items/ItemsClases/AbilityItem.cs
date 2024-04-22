public class AbilityItem : Item
{
    public PlayerControl.Ataques ability;

    private void Awake()
    {
        _defaultDescription = "";
    }

    public override void OnItemPickup(PlayerControl player)
    {
        GameManager.Instance.Player._abilityHolder.AddAbility(ability);
    }
}
