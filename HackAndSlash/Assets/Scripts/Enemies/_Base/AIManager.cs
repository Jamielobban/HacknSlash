using System;
using System.Collections.Generic;
using UnityEngine;

public class AIManager : MonoBehaviour
{
    public Transform target;
    public float radiusAroundTarget;
    public List<EnemyBase> enemies = new List<EnemyBase>();

    private void OnEnable()
    {
        MakesAgentsCircleTarget();
    }

    private void MakesAgentsCircleTarget()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].targetToSpecialMove = new Vector3(target.position.x + radiusAroundTarget * Mathf.Cos(2 * Mathf.PI * i / enemies.Count), target.position.y, target.position.z + radiusAroundTarget * Mathf.Sin(2 * Mathf.PI * i / enemies.Count));
        }
    }
}
