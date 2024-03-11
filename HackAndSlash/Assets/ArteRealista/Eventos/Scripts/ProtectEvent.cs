using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProtectEvent : EventMap
{
    [SerializeField] GameObject spawnerParent;
    [SerializeField] List<Transform> targetsToProtect;    
    List<SpawnerBase> enemiesSpawner;
    

    protected override void Start()
    {
        base.Start();
        enemiesSpawner = spawnerParent.GetComponentsInChildren<SpawnerBase>(true).ToList();
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(false));
        }
    }
    protected override void Update()
    {
        base.Update();
        if(CurrentEventState == Enums.EventState.PLAYING)
            RetargetSpawnedEnemies();
        CheckEventState();
    }
    protected override void StartEvent()
    {

        base.StartEvent();
        enemiesSpawner[currentRound].gameObject.SetActive(true);
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(true));
        }
    }
    protected override void NextRound()
    {
        enemiesSpawner[currentRound].gameObject.SetActive(false);
        base.NextRound();
        enemiesSpawner[currentRound].gameObject.SetActive(true);
    }
    protected override void FinishEvent()
    {
        enemiesSpawner[currentRound].gameObject.SetActive(false);
        base.FinishEvent();
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.gameObject.SetActive(false));
        }
    }
    protected override void RestartEvent()
    {
        base.RestartEvent();
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
        if (_currentEventState == Enums.EventState.PLAYING && AllEnemiesDefeated())
        {
            if (currentRound >= enemiesSpawner.Count - 1)
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
    bool AllEnemiesDefeated()
    {
        return enemiesSpawner[currentRound].enemiesFromThisSpawner.Count <= 0;
    }

    void RetargetSpawnedEnemies(bool toPillar = true)
    {
        foreach (Enemy e in enemiesSpawner[currentRound].enemiesFromThisSpawner)
        {
            if (e.isDead)
                continue;

            e.movements.useMovementPrediction = false;

            bool isCorrected = false;

            if (toPillar)
            {
                foreach (Transform t in targetsToProtect)
                {
                    if (e.movements.target == t)
                    {
                        isCorrected = true;
                        break;
                    }
                }

                if (!isCorrected)
                    e.movements.target = targetsToProtect[Random.Range(0, targetsToProtect.Count)];
            }
            else
            {
                if (e.movements.target != GameManager.Instance.Player.transform)
                    e.movements.target = GameManager.Instance.Player.transform;

            }
            
        }
    }    
}
