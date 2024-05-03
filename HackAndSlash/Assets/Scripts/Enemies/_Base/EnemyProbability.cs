using UnityEngine;
using UnityEngine.Serialization;

[System.Serializable]
public class EnemyProbability
{
    [FormerlySerializedAs("enemy")] public EnemyBase enemyBase;
    [Range(0, 100)] public int probability;
}
