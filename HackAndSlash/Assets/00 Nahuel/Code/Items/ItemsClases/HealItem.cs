using UnityEngine;

public class HealItem : Item
{
    private void Awake()
    {
        _defaultDescription = "Heals, it's a instantant consumible item";
    }

    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.healthSystem.Heal(+data.value);
    }
}
