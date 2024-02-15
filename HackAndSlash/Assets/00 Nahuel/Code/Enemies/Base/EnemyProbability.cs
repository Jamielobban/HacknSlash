using UnityEngine;

[System.Serializable]
public class EnemyProbability
{
    public Enemy enemy;
    [Range(0, 100)] public int probability;
}
