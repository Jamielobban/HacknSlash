using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// -- Base class for Probability Spawners, !! Spawner method should be always Probability !! -- //
public class ProbabilitySpawner : SpawnerBase
{
    [SerializeField] private List<EnemyProbability> _enemyProbabilities = new List<EnemyProbability>();
    protected Dictionary<Enemy, int> _enemiesDict = new Dictionary<Enemy, int>();

    protected override void Awake()
    {
        base.Awake();

        enemySpawnMethod = Enums.SpawnMethod.Probability;

        NormalizeProbabilities();

        foreach (var enemyProbability in _enemyProbabilities)
        {
            _enemiesDict.Add(enemyProbability.enemy, enemyProbability.probability);
        }        
    }

    protected override void SpawnProbabilityEnemy()
    {
        int totalProbabilities = 100;

        int randomValue = Random.Range(0, totalProbabilities);
        
        DoSpawnEnemy(GetProbableEnemeyKey(randomValue), ChooseRandomPositionOnNavMesh());
    }

    private Enemy GetProbableEnemeyKey(int value)
    {
        foreach (var kvp in _enemiesDict)
        {
            value -= kvp.Value;
            if(value <= 0)
            {
                // return kvp.Key;
                return kvp.Key;
            }
        }
        // Should not reach here
        Debug.LogError("Couldn't return a probability key enemy");
        return null;
    }

    private void NormalizeProbabilities()
    {
        int totalProbability = 0;

        foreach (var enemyProbability in _enemyProbabilities)
        {
            totalProbability += enemyProbability.probability;
        }

        if (totalProbability == 0)
        {
            enemySpawnMethod = Enums.SpawnMethod.Random; // Will Spawn Random Enemies
            Debug.LogError("The sum of probabilities is 0. Make sure you introduce valid probabilities. Spawning Method swaping to Random. ");
            return;
        }

        foreach (var enemyProbability in _enemyProbabilities)
        {
            enemyProbability.probability = Mathf.RoundToInt((float)enemyProbability.probability / totalProbability * 100);
        }
    }
}
