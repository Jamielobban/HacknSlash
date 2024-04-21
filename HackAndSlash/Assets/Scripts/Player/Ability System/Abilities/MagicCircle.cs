using UnityEngine;

public class MagicCircle : BaseAbility
{
    protected override void SetVisualEffect()
    {
        base.SetVisualEffect();
        GameObject go = Instantiate(data.effect, transform.position, Quaternion.identity);
        
    }
}
