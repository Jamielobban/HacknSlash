using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour, IDamageable
{
    public GameObject canvas;
    [SerializeField] float maxHealth;
    [SerializeField] Image progressBar;
    float currentHealth = 100;
    public GameObject onHitVfx;
    private bool _isPlayer = false;
    public float CurrentHealth { get => currentHealth; set => currentHealth = value;  }
    public float MaxHealth => maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        progressBar.material = new Material(progressBar.material); ;
    }
    public bool IsPlayer() => _isPlayer;

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        GameObject go = Instantiate(onHitVfx, new Vector3(transform.position.x, transform.position.y +0.5f, transform.position.z), Quaternion.identity);
        go.transform.localScale = Vector3.one * 2;
        UpdateProgressBar();
        if (currentHealth < 0)
            currentHealth = 0;
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        UpdateProgressBar();
        if(currentHealth > maxHealth)
            currentHealth = maxHealth;
    }

    public void RestartHeal() => currentHealth = maxHealth;

    public bool IsDead => currentHealth <= 0;
    
    private void UpdateProgressBar()
    {
        int propId = Shader.PropertyToID("_ProgressBar");
        progressBar.material.SetFloat(propId, Mathf.Clamp(currentHealth / maxHealth, 0, 1));
    }
}
