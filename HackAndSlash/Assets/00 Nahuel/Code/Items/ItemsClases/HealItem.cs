using UnityEngine;

public class HealItem : Item
{
    public override void OnItemPickup(PlayerControl player, int stacks)
    {
        player.healthSystem.Heal(+data.value);
    }
}
