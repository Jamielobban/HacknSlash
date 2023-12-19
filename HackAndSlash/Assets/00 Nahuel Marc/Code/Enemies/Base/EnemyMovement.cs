using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    private NavMeshAgent _agent;
    private EnemyEvents _events;

    #region Stats

    public CharacterStat chaseSight = new CharacterStat();
    public CharacterStat areaPatroll = new CharacterStat();

    #endregion
    #region AreaPatroll Variables
    private Vector3 respawnPoint;
    private Vector3 patrollPoint;
    private bool reached = false;
    #endregion

    protected virtual void Awake()
    {
        respawnPoint = transform.position;
        patrollPoint = GetRandomNavmeshPoint();
        _events = GetComponent<EnemyEvents>();
        _agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerControl>().transform;

        _events.OnIdle += () => DisableMovement();
        _events.OnAttacking += () => DisableMovement();
        _events.OnPatrolling += () => EnableMovement();
        _events.OnFollowing += () => EnableMovement();
        //_events.OnAir += () => DisableMovement();
        _events.OnAir += () => { DisableMovement(); DisableAgent(); }; // TESTING
        _events.OnStun += () => DisableMovement();
    }
    public void EnableAgent() => _agent.enabled = true;
    public void DisableAgent() => _agent.enabled = false;
    private void EnableMovement() => _agent.isStopped = false;
    private void DisableMovement() => _agent.isStopped = true;

    public void HandleFollow()
    {
        _agent.destination = target.position;
        HandleRotation();
    }

    public void HandlePatrollInArea()
    {
        _agent.destination = patrollPoint;
        if(reached)
        {
            patrollPoint = GetRandomNavmeshPoint();
            reached = false;
        }
        if(Vector3.Distance(patrollPoint, transform.position) < 0.1f)
        {
            reached = true;
        }
    }

    public void KnockBack(float x, float y)
    {

    }

    public void ThrowToAir()
    {

    }

    private void HandleRotation()
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

    private Vector3 GetRandomNavmeshPoint()
    {
        Vector3 randomDir = Random.insideUnitSphere * areaPatroll.Value;
        randomDir += respawnPoint;
        NavMeshHit hit;
        Vector3 finalPos = Vector3.zero;
        if(NavMesh.SamplePosition(randomDir, out hit, areaPatroll.Value, 1))
        {
            finalPos = hit.position;
        }
        return finalPos;
    }

    private float DistanceToPlayer() => Vector3.Distance(target.position, transform.position);
    public bool InRangeToChase() => DistanceToPlayer() > chaseSight.Value;

    private void OnDestroy()
    {
        _events.OnIdle -= () => DisableMovement();
        _events.OnAttacking -= () => DisableMovement();
        _events.OnPatrolling -= () => EnableMovement();
        _events.OnFollowing -= () => EnableMovement();
    }
}
