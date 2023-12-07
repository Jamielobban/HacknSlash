using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent _agent;

    protected virtual void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    public void EnableMovement()
    {
        _agent.isStopped = false;
    }

    public void DisableMovement()
    {
        _agent.isStopped = true;
    }

    public void HandleFollow(Transform target)
    {
        _agent.destination = target.position;
        HandleRotation();
    }

    // Patrolling between point or area?
    public void HandlePatrollBetweenPoints()
    {

    }

    public void HandlePatrollInArea()
    {

    }

    public void HandleRotation()
    {
        if(_agent.path.corners.Length > 0)
        {
            Vector3 forward = _agent.velocity.normalized;
            if(forward.magnitude > 0)
            {
                Quaternion rotation = Quaternion.LookRotation(forward); // 5f -> RotationSpeed
                Quaternion enemyRot = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
                transform.rotation = enemyRot;
            }
        }
    }
}
