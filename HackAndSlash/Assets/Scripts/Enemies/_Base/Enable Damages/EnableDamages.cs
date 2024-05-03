using UnityEngine;

public class EnableDamages : MonoBehaviour
{
    public Collider meleeDamages;
    [SerializeField]protected EnemyBase _enemy;
    
    protected virtual void Awake() {}
    protected virtual void Start()
    {
        _enemy.GetComponent<EnemyBase>();
        meleeDamages.enabled = false;
    }

    public void EnableIsAttacking() => _enemy.isAttacking = true;
    public void DisableIsAttacking() => _enemy.isAttacking = false;
    public void SetMeleeCollider() => meleeDamages.enabled = !meleeDamages.enabled;
    public void DisableMeleeCollider() => meleeDamages.enabled = false;
    
}
