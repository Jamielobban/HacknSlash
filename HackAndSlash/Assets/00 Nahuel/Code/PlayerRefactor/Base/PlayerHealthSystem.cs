using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    private PlayerControl _player;
    public float maxHealth;
    private float _currentHealth;
    private float _timer = 0f;

    public GameObject loadingBar;
    public Image loadingFill;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    private void Awake()
    {
        _player = transform.parent.GetComponent<PlayerControl>();
    }
    private void Start()
    {
        maxHealth = _player.stats.maxHealth;
        _currentHealth = maxHealth;
    }

    private void Update()
    {
        if(_player.healthRegen > 0)
        {
            _timer += Time.deltaTime;
            if (_timer >= _player.timeToHeal)
            {
                Heal(_player.healthRegen);
                _timer = 0f;
            }
        }
    }
    public void AirDamageable()
    {
        throw new System.NotImplementedException();
    }

    public void Heal(float amount)
    {
        _currentHealth += amount;

        _player.healPixel.Spawn(_player.transform.position + new Vector3(0f, 2f, 0f), amount);

        if (_currentHealth >= maxHealth)
        {
            _currentHealth = maxHealth;
        }
        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        if(_player.states == PlayerControl.States.HIT || _player.states == PlayerControl.States.DASH || _player.states == PlayerControl.States.DEATH)
        {
            return;
        }

        _player.states = PlayerControl.States.HIT;

        _player.HitEffect();

        _currentHealth -= damage;
        if(GameManager.Instance.state != Enums.GameState.Tutorial)
        {
            if (_currentHealth <= 0)
            {
                _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
                _currentHealth = 0;
                Die();
            }
        }
        else
        {
            if (_currentHealth <= maxHealth * 0.2f)
            {
                _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
                _currentHealth = maxHealth * 0.2f;
            }
        }

        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void Die()
    {
        if(_player.states == PlayerControl.States.DEATH)
        {
            return;
        }
        _player.DeadEffect();
        if(GameManager.Instance.state == Enums.GameState.Playing)
        {
            Invoke(nameof(StartLoadingBar), 1f);
        }
    }

    private void StartLoadingBar()
    {
        loadingBar.SetActive(true);
        Invoke(nameof(StartLoadingNewScene), 1f);
    }

    private void StartLoadingNewScene()
    {
        GameManager.Instance.LoadLevel(Constants.SCENE_DEAD, loadingFill);
    }
}
