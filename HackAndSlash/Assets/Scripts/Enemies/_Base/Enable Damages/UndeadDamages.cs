using UnityEngine;

public class UndeadDamages : EnableDamages
{
    public Collider doubleHitCollider;
    
    public void SetDoubleHitCollider() => doubleHitCollider.enabled = !doubleHitCollider.enabled;
}
