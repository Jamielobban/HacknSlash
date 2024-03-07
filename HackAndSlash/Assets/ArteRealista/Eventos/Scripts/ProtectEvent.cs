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
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.canvas.SetActive(false));
        }
    }
    protected override void Update()
    {
        base.Update();
        RetargetSpawnedEnemies();
        CheckEventState();
    }
    protected override void StartEvent()
    {
        base.StartEvent();
        enemiesSpawner[currentRound].gameObject.SetActive(true);
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.canvas.SetActive(true));
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
        base.FinishEvent();
        enemiesSpawner[currentRound].gameObject.SetActive(false);
        foreach (Transform t in targetsToProtect)
        {
            t.GetComponentsInChildren<DamageableObject>().ToList().ForEach(dam => dam.canvas.SetActive(false));
        }
    }
    void CheckEventState()
    {
        if (currentEventState == Enums.EventState.PLAYING && AllEnemiesDefeated())
        {
            if (currentRound >= enemiesSpawner.Count - 1)
                FinishEvent();
            else
                NextRound();
        }
    }
    bool AllEnemiesDefeated()
    {
        if (enemiesSpawner[currentRound].enemiesFromThisSpawner.Count < enemiesSpawner[currentRound].EnemiesToSpawn)
            return false;

        foreach (Enemy e in enemiesSpawner[currentRound].enemiesFromThisSpawner)
        {
            if (!e.isDead)
                return false;
        }

        return true;
    }

    void RetargetSpawnedEnemies()
    {
        foreach (Enemy e in enemiesSpawner[currentRound].enemiesFromThisSpawner)
        {
            if (e.isDead)
                continue;

            bool isCorrected = false;
            foreach(Transform t in targetsToProtect)
            {
                if(e.movements.target == t)
                {
                    isCorrected = true;
                    break;
                }
            }

            if(!isCorrected)
                e.movements.target = targetsToProtect[Random.Range(0, targetsToProtect.Count)];
        }
    }    
}
