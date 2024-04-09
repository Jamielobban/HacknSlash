using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillsEvent : EventMap
{
    /*[SerializeField] GameObject spawnerParent;
   // List<SpawnerBase> enemiesSpawner;

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
        enemiesSpawner[currentRound].gameObject.SetActive(false);
        base.FinishEvent();
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
    }

    bool AllEnemiesDefeated()
    {
        return enemiesSpawner[currentRound].enemiesFromThisSpawner.Count <= 0;
    }*/

}
