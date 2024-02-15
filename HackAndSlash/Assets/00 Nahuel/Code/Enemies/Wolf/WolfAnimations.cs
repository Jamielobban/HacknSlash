using UnityEngine;

public class WolfAnimations : EnemyAnimations
{
    protected override void Awake()
    {
        base.Awake();
        _events.OnAir += () => PlayTargetAnimation("Air", true); // Only wolf
    }
}
