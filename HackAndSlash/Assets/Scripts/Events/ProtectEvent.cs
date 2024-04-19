using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ProtectEvent : EventMap
{
    [SerializeField] List<Transform> targetsToProtect;
    public DamageableObject damageEventScript;

    protected override void Start()
    {
        base.Start();
        damageEventScript.CurrentHealth = damageEventScript.MaxHealth;
        
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(false));
        }
    }
    protected override void Update()
    {
        base.Update();

        if(CurrentEventState == Enums.EventState.PLAYING)
        {
            RetargetSpawnedEnemies();
            CheckEventState();
        }
    }
    protected override void StartEvent()
    {
        base.StartEvent();
        
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(true);
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(true));
        }
        StartSpawningEnemies();

    }
    protected override void NextRound()
    {
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(false);
        base.NextRound();
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(true);
        StartSpawningEnemies();

    }
    protected override void FinishEvent()
    {
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(false);
        base.FinishEvent();

        foreach (EnemiesToKill enemy in roundsOfEnemies)
        {
            foreach (var e in enemy.enemiesToKill)
            {
                e.GetComponent<EnemyBase>().OnDespawnEnemy();
            }
            enemy.enemiesToKill.Clear();
            enemy.parent.gameObject.SetActive(false);
        }

        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(false));
        }
    }
    protected override void RestartEvent()
    {
        base.RestartEvent();

        currentRound = 0;
        foreach (EnemiesToKill enemy in roundsOfEnemies)
        {
            enemy.parent.gameObject.SetActive(false);
            foreach (var e in enemy.enemiesToKill)
            {
                e.GetComponent<EnemyBase>().OnDespawnEnemy();
            }
            enemy.enemiesToKill.Clear();
        }

        RetargetSpawnedEnemies(false);
        foreach (Transform t in targetsToProtect)
        {
            List<DamageableObject> damageables = GetComponentsInChildren<DamageableObject>().ToList();
            foreach (DamageableObject dam in damageables)
            {
                dam.gameObject.SetActive(false);
                dam.MaxHeal();
            }
        }
    }
    void CheckEventState()
    {
        if (AllEnemiesDefeated())
        {
            if (currentRound >= roundsOfEnemies.Count - 1)
                FinishEvent();
            else
                NextRound();
        }
        else
        {
            foreach (Transform t in targetsToProtect)
            {
                List<DamageableObject> damageables = GetComponentsInChildren<DamageableObject>().ToList();
                foreach (DamageableObject dam in damageables)
                {
                    if (dam.IsDead)
                    {
                        RestartEvent();
                        break;
                    }
                }
            }
        }
    }

    void RetargetSpawnedEnemies(bool toPillar = true)
    {
        Transform target = targetsToProtect[Random.Range(0, targetsToProtect.Count)];

        foreach (GameObject e in roundsOfEnemies[currentRound].enemiesToKill)
        {
            e.GetComponent<EnemyBase>().target = target;
        }
    }    
}
