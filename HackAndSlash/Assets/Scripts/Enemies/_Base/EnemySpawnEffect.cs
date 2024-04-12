using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnEffect : MonoBehaviour
{
    [Header("Configuration: ")] 
    public List<SkinnedMeshRenderer> skinnedMesh = new List<SkinnedMeshRenderer>();
    public float undissolveDuration;
    public float dissolveDuration;
    public float targetDissolveAmount;
    public bool IsSpawning => _isSpawning;
    private List<Material> _skinnedMaterials = new List<Material>();
    private bool _isSpawning = false;
    private EnemyBase _enemyBase;

    private void Awake()
    {
        SetSkinnedMaterials();
        _enemyBase = GetComponent<EnemyBase>();
        ResetDissolveAmount();
    }

    public void InitializeSpawnEffect()
    {
        StartCoroutine(ReSpawn());
    }

    public void InitializeDespawnEffect()
    {
        StartCoroutine(DeSpawn());
    }

    private IEnumerator DeSpawn()
    {
        if (_skinnedMaterials.Count > 0)
        {
            float elapsedTime = 0f;
            float startDissolveAmount = _skinnedMaterials[0].GetFloat("_DissolveAmount");
            for (int i = 0; i < _skinnedMaterials.Count; i++)
            {
                _skinnedMaterials[i].SetFloat("_EdgeThickness", 0.3f);
            }
            _isSpawning = true;
            while (elapsedTime < dissolveDuration)
            {
                float t = elapsedTime / dissolveDuration;
                float currentDissolveAmount = Mathf.Lerp(startDissolveAmount, targetDissolveAmount, t);
                for (int i = 0; i < _skinnedMaterials.Count; i++)
                {
                    _skinnedMaterials[i].SetFloat("_DissolveAmount", currentDissolveAmount);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        for (int i = 0; i < _skinnedMaterials.Count; i++)
        {
            _skinnedMaterials[i].SetFloat("_DissolveAmount", 1);
            _skinnedMaterials[i].SetFloat("_EdgeThickness", 0f);
        }
        
        ManagerEnemies.Instance.AddEnemyScore(_enemyBase.score);
        _enemyBase.OnDie();
        _isSpawning = false;
    }
    
    private IEnumerator ReSpawn()
    {
        if (_skinnedMaterials.Count > 0)
        {
            float elapsedTime = 0f;
            float startDissolveAmount = _skinnedMaterials[0].GetFloat("_DissolveAmount");
            for (int i = 0; i < _skinnedMaterials.Count; i++)
            {
                _skinnedMaterials[i].SetFloat("_EdgeThickness", 0.3f);
            }
            _isSpawning = true;
            while (elapsedTime < undissolveDuration)
            {
                float t = elapsedTime / undissolveDuration;
                float currentDissolveAmount = Mathf.Lerp(startDissolveAmount, 0, t);
                for (int i = 0; i < _skinnedMaterials.Count; i++)
                {
                    _skinnedMaterials[i].SetFloat("_DissolveAmount", currentDissolveAmount);
                }
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
        for (int i = 0; i < _skinnedMaterials.Count; i++)
        {
            _skinnedMaterials[i].SetFloat("_DissolveAmount", 0);
            _skinnedMaterials[i].SetFloat("_EdgeThickness", 0f);
        }
        _isSpawning = false;
    }

    public void ResetDissolveAmount()
    {
        if (_skinnedMaterials == null)
        {
            SetSkinnedMaterials();
        }
        for (int i = 0; i < _skinnedMaterials.Count; i++)
        {
            _skinnedMaterials[i].SetFloat("_DissolveAmount", 1);
        }
        _isSpawning = false;
    }
    private void SetSkinnedMaterials()
    {
        if (skinnedMesh.Count > 0)
        {
            for (int i = 0; i < skinnedMesh.Count; i++)
            {
                for (int j = 0; j < skinnedMesh[j].materials.Length; j++)
                {
                    _skinnedMaterials.Add(skinnedMesh[i].materials[j]);

                }
            }
        }
    }

    public bool SetSpawning(bool spawn) => _isSpawning = spawn;
}

