using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageableObject : MonoBehaviour, IDamageable
{
    public GameObject canvas;
    [SerializeField] float maxHealth;
    [SerializeField] Image progressBar;
    float currentHealth;

    private void Start()
    {
        currentHealth = maxHealth;
        progressBar.material = new Material(progressBar.material); ;
    }

    private void Update()
    {
        int propId = Shader.PropertyToID("_ProgressBar");
        progressBar.material.SetFloat(propId, Mathf.Clamp(currentHealth / maxHealth, 0, 1));
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth < 0)
            currentHealth = 0;
    }

    public void Heal(float amount)
    {
        throw new System.NotImplementedException();
    }

    public void AirDamageable()
    {
        throw new System.NotImplementedException();
    }
}
