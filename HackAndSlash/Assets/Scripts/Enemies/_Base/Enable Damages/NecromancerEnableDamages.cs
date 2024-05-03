using UnityEngine;
public class NecromancerEnableDamages : EnableDamages
{
    public RangedAttack _rangedAttack;
    
    public void Shoot() =>_rangedAttack.ShootAction();
    public void EnableChannel() => _rangedAttack.EnableChannelAction();
    public void DisableChannel() => _rangedAttack.DisableChannelAction();

}
