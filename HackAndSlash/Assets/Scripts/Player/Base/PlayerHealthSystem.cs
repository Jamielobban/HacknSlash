using DamageNumbersPro;
using MoreMountains.Tools;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour, IDamageable
{
    public GameObject[] canvasToDeactivate;
    public Debuff[] debuffs;
    private PlayerControl _player;
    public float maxHealth;
    private float _currentHealth;
    private float _timer = 0f;

    public GameObject loadingBar;
    public Image loadingFill;

    public List<GameObject> bloodImages = new List<GameObject>();
    public GDTFadeEffect fadeOutLowHp;
    public AudioSource lowHpAudio;

    public GameObject onDie;
    private bool _isPlayer = true;
    public bool IsDamageable;

    public GameObject hitEffect;

    public float CurrentHealth
    {
        get { return _currentHealth; }
    }
    private void Awake()
    {
        IsDamageable = true;
        lowHpAudio = GetComponent<AudioSource>();   
        lowHpAudio.Stop();
        _player = transform.parent.GetComponent<PlayerControl>();
        GameObject parent = GameObject.Find("BloodParent");
        for (int i = 0; i < parent.transform.childCount; i++)
        {
            GameObject child = parent.transform.GetChild(i).gameObject;
            bloodImages.Add(child);
            child.SetActive(false);
        }
    }
    private void Start()
    {
        _player.hitTime = Time.time;
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
    public bool IsPlayer() => _isPlayer;

    public void Heal(float amount)
    {
        if (_currentHealth != maxHealth && amount >= 0.1)
        {
            _player.healPixel.Spawn(_player.transform.position + new Vector3(0f, 2f, 0f), amount);
        }
        _currentHealth += amount;

        if ((_currentHealth > maxHealth * 0.25) && bloodImages[2].activeSelf)
        {
            lowHpAudio.Stop();
            SetFadeOutBloodHard();
        }

        if (_currentHealth >= maxHealth)
        {
            _currentHealth = maxHealth;
        }
        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void TakeDamage(float damage, DamageNumber visualEffect)
    {
        if(_player.states == PlayerControl.States.HIT || _player.states == PlayerControl.States.DASH || _player.states == PlayerControl.States.DEATH || Time.time-_player.hitTime < 0.35f || !IsDamageable)
        {
            return;
        }

        if(_player.states != PlayerControl.States.ATTACK)
        {
            _player.states = PlayerControl.States.HIT;
        }

        visualEffect.Spawn(_player.transform.position + new Vector3(0f, 2f, 0f), (int)damage);

        _player.HitEffect();
        GameObject go = Instantiate(hitEffect, transform);
        go.transform.localScale = Vector3.one;
        go.transform.localPosition = new Vector3(0, 1, 0);
        SetBloodEffect();
        _currentHealth -= damage;
        if(GameManager.Instance.state != Enums.GameState.Tutorial)
        {
            if (_currentHealth <= 0)
            {
                lowHpAudio.Stop();
                _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
                _currentHealth = 0;
                Die();
            }
        }
        else
        {
            if (_currentHealth <= maxHealth * 0.3f)
            {
                lowHpAudio.Stop();
                _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
                _currentHealth = maxHealth * 0.3f;
            }
        }

        _player.hud.UpdateHealthBar(_currentHealth, maxHealth);
    }

    public void ApplyBurn()
    {
        float dmg = maxHealth * 0.05f; // 40 % of damage base

        debuffs[0].ApplyDebuff(dmg);
    }
    public void ApplyStun(float timeStun)
    {
        //Player Gets Stun
    }
    private void SetBloodEffect()
    {
        if (_currentHealth <= (maxHealth * 0.85) && _currentHealth > (maxHealth * 0.25f))
        {
            bloodImages[1].SetActive(true);
        }
        else if (_currentHealth <= (maxHealth * 0.35) && _currentHealth > 0)
        {
            lowHpAudio.Play();
            bloodImages[2].SetActive(true);
        }
        else
        {
            bloodImages[0].SetActive(true);
        }
    }

    public void SetFadeOutBloodHard()
    {
        if (bloodImages[2].activeSelf)
        {
            fadeOutLowHp.StartEffect();
        }
    }


    public void Die()
    {
        if(_player.states == PlayerControl.States.DEATH)
        {
            return;
        }
        SetFadeOutBloodHard();
        _player.DeadEffect();
        if(GameManager.Instance.state == Enums.GameState.Playing)
        {
            foreach (var canvas in canvasToDeactivate)
            {
                canvas.SetActive(false);
            }

            foreach (var enemyPool in LevelManager.Instance.EnemiesManager.parentObjectPools)
            {
                enemyPool.SetActive(false);
            }

            onDie.SetActive(true);
            AudioManager.Instance.PlayMusic(Enums.Music.Defeat);
            onDie.GetComponent<GDTFadeEffect>()?.StartEffect();
            Invoke(nameof(StartLoadingBar), 4f);
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
