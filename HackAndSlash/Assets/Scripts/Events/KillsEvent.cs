using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class KillsEvent : EventMap
{

    protected override void Update()
    {
        base.Update();
        CheckEventState();
    }    
    protected override void StartEvent()
    {
        base.StartEvent();
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(true);
        StartSpawningEnemies();
        canCheckEnemiesDead = true;

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
    }

    void CheckEventState()
    {
        if (_currentEventState == Enums.EventState.PLAYING && AllEnemiesDefeated())
        {
            if (currentRound >= roundsOfEnemies.Count - 1)
                FinishEvent();
            else
                NextRound();
        }
    }

}
