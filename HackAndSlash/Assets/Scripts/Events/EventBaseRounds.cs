using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventBaseRounds : EventBase
{
    public List<EnemiesToKill> roundsOfEnemies = new List<EnemiesToKill>();
    protected int currentRound = 0;
    public bool canCheckEnemiesDead = false;

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

    protected override void CompleteEvent()
    {
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(false);
        base.CompleteEvent();
    }

    protected override void DefeatEvent()
    {
        base.DefeatEvent();
    }

    protected virtual void NextRound()
    {
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(false);
        currentRound++;
        roundsOfEnemies[currentRound].parent.gameObject.SetActive(true);
        StartSpawningEnemies();
    }

    protected virtual bool AllEnemiesDefeated() => roundsOfEnemies[currentRound].enemiesToKill.Count <= 0 && canCheckEnemiesDead;

    protected virtual void CheckEventState()
    {
        if(_currentEventState == Enums.EventState.PLAYING && AllEnemiesDefeated())
        {
            if(currentRound >= roundsOfEnemies.Count - 1)
            {
                CompleteEvent();
            }
            else
            {
                NextRound();
            }
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        if (roundsOfEnemies[currentRound].enemiesToKill.Contains(enemy))
        {
            roundsOfEnemies[currentRound].enemiesToKill.Remove(enemy);
        }
    }

    protected virtual void StartSpawningEnemies()
    {
        foreach (var enemy in roundsOfEnemies[currentRound].enemiesToKill)
        {
            enemy.GetComponent<EnemyBase>().OnSpawnEnemy();
        }
    }

}
