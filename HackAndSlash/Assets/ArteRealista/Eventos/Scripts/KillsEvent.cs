using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillsEvent : EventMap
{
    [SerializeField] GameObject spawnerParent;
    List<SpawnerBase> enemiesSpawner;

    protected override void Start()
    {
        base.Start();
        enemiesSpawner = spawnerParent.GetComponentsInChildren<SpawnerBase>(true).ToList();
    }
    protected override void Update()
    {
        base.Update();
        CheckEventState();
    }    
    protected override void StartEvent()
    {
        base.StartEvent();
        enemiesSpawner[currentRound].gameObject.SetActive(true);
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

}
