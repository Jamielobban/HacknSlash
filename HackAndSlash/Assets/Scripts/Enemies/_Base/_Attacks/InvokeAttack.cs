using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

public class InvokeAttack : BaseEnemyAttack
{
    public List<EnemyBase> instantiableEnemies = new List<EnemyBase>();
    public List<GameObject> instantiablePoints = new List<GameObject>();
    public GameObject invokeEffect;
    private ManagerEnemies _manager;

    protected override void Awake()
    {
        base.Awake();
        _manager = ManagerEnemies.Instance;
    }
    public void OnInvoke(State<Enums.EnemyStates, Enums.StateEvent> state)
    {
        _enemyBase.transform.LookAt(_enemyBase.Player.transform.position);
        _enemyBase._currentTime = 0f;
        Ray ray = new Ray(_enemyBase.transform.position, Vector3.down);
        RaycastHit hitInfo;

        if (Physics.Raycast(ray, out hitInfo, 25f, LayerMask.GetMask("Ground")))
        {
            Vector3 hitPoint = hitInfo.point;
            Quaternion groundRotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            Quaternion desiredRotation = Quaternion.Euler(-90f, 0f, 0f); 
            Quaternion finalRotation = groundRotation * desiredRotation;
            GameObject nuevoPlano = Instantiate(invokeEffect, hitPoint, finalRotation);
        }
        Use();
    }

    protected override void AttackAction()
    {
        base.AttackAction();
        foreach (var point in instantiablePoints)
        {
            int enemy = Random.Range(0, instantiableEnemies.Count);
            EnemyBase enemyToSpawn = instantiableEnemies[enemy];
            PoolableObject poolable = _manager.enemyObjectsPools[enemyToSpawn].GetObject();

            if (poolable != null)
            {
                EnemyBase enemyBase = poolable.GetComponent<EnemyBase>();
                enemyBase.target = GameManager.Instance.Player.transform;
                enemyBase.spawner = this.gameObject;
                enemyBase.OnSpawnEnemy();
                NavMeshHit hit;
                if (UnityEngine.AI.NavMesh.SamplePosition(point.transform.position, out hit, 50f, -1))
                {
                    enemyBase.Agent.Warp(hit.position);
                    enemyBase.Agent.enabled = true;
                }
                else
                {
                    Debug.LogError($"Unable to place NavmeshAgent on Navmesh chau");
                }
            }
            else
            {
                Debug.LogError($"Unable to fetch enemy of type {point.transform.position} from object pool. Out of objects?");
            }
        }
    }
}
