using UnityEngine;

public class MagicCircle : BaseAbility
{
    public PlayerControl.Ataques ataque;
    protected override void SetVisualEffect()
    {
        base.SetVisualEffect();
       // GameObject go = Instantiate(data.effect, transform.position, Quaternion.identity);
        
    }
}
