using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AIManager : MonoBehaviour
{
    public Transform target;
    public float radiusAroundTarget;
    public List<EnemyBase> enemiesSurround = new List<EnemyBase>();
    public List<EnemyBase> enemiesToPlayer = new List<EnemyBase>();

    private void OnEnable()
    {
        MakesAgentsCircleTarget();
        MakesAgentGoToPlayer();
    }

    private void MakesAgentsCircleTarget()
    {
        for (int i = 0; i < enemiesSurround.Count; i++)
        {
            enemiesSurround[i].targetToSpecialMove = new Vector3(target.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / enemiesSurround.Count), target.position.y, target.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / enemiesSurround.Count));
        }
    }
    private void MakesAgentGoToPlayer()
    {
        foreach (var enemy in enemiesToPlayer)
        {
            enemy.target = GameManager.Instance.Player.transform;
        }
    }
}
