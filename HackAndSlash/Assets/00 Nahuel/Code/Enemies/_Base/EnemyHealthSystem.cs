using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IDamageable
{
    [SerializeField] private float _maxHealth;
    private float _currentMaxHealth;
    private float _currentHealth;
    private Enemy _enemy;
    public EnemyHUD hud;
    
    private void Awake()
    {
        _enemy = transform.parent.GetComponent<Enemy>();
        _currentMaxHealth = _maxHealth;
        _currentHealth = _currentMaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (_enemy.EnemyFSM.ActiveStateName == Enums.EnemyStates.Dead)
        {
            return;
        }
        _currentHealth -= damage;
        //Invoke Hit Transition(?)
        hud.UpdateHealthBar(_currentHealth, _currentMaxHealth);
        if (_currentHealth <= 0)
        {
            //Invoke Die Transition (?)
        }
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;
        
        if(_currentHealth > _currentMaxHealth)
        {
            _currentHealth = _currentMaxHealth;
        }
        hud.UpdateHealthBar(_currentHealth, _currentMaxHealth);
    }
}
