using System.Collections.Generic;
using UnityEngine;

public class InfinityProbabilitySpawner : InfiniteSpawner
{
    [SerializeField] private List<EnemyProbability> _enemyProbabilities = new List<EnemyProbability>();
    protected Dictionary<EnemyBase, int> _enemiesDict = new Dictionary<EnemyBase, int>();

    protected override void Start()
    {
        base.Start();
        enemySpawnMethod = Enums.SpawnMethod.Probability;

        NormalizeProbabilities();

        foreach (var enemyProbability in _enemyProbabilities)
        {
            _enemiesDict.Add(enemyProbability.enemyBase, enemyProbability.probability);
        }
    }
    protected override void SpawnProbabilityEnemy()
    {
        int totalProbabilities = 100;

        int randomValue = Random.Range(0, totalProbabilities);

        DoSpawnEnemy(GetProbableEnemeyKey(randomValue), ChooseRandomPositionOnNavMesh());
    }

    private EnemyBase GetProbableEnemeyKey(int value)
    {
        foreach (var kvp in _enemiesDict)
        {
            value -= kvp.Value;
            if (value <= 0)
            {
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
