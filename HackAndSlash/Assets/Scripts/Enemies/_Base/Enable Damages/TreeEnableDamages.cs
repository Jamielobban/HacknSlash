using UnityEngine;
public class TreeEnableDamages : EnableDamages
{
    public Collider areaCollider;

    protected override void Awake()
    {
        base.Awake();
        areaCollider.enabled = false;
    }

    public void SetAreaCollider()
    {
        areaCollider.enabled = !areaCollider.enabled;
    }

    public void DisableAreaCollider()
    {
        areaCollider.enabled = false;
    }
}
