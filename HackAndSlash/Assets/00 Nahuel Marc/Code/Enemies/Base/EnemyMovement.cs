using UnityEngine;
using UnityEngine.AI;
// -- Enemy base class for movements and displacements -- //
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    protected NavMeshAgent _agent;
    protected EnemyEvents _events;
    protected Rigidbody _rb;
    protected Enemy _enemy;

    #region Stats

    public CharacterStat chaseSight = new CharacterStat();
    public CharacterStat areaPatroll = new CharacterStat();
    public CharacterStat toAirForce = new CharacterStat();

    #endregion
    #region AreaPatroll Variables
    private Vector3 respawnPoint;
    private Vector3 patrollPoint;
    private bool reached = false;
    #endregion

    protected virtual void Awake()
    {
        _enemy = GetComponent<Enemy>();
        respawnPoint = transform.position;
        patrollPoint = GetRandomNavmeshPoint();
        _rb = GetComponent<Rigidbody>();
        _events = GetComponent<EnemyEvents>();
        _agent = GetComponent<NavMeshAgent>();
        target = FindObjectOfType<PlayerControl>().transform;

        _events.OnHit += () => { if (_agent.isActiveAndEnabled) { DisableMovement(); } HitEffect(); } ;
        _events.OnIdle += DisableMovement;
        _events.OnAttacking += DisableMovement;
        _events.OnPatrolling += EnableMovement;
        _events.OnFollowing += EnableMovement;
        _events.OnStun += DisableMovement;
    }
    public Rigidbody GetRigidBody() => _rb;
    public void EnableGravity() => _rb.useGravity = true;
    public void DisableGravity() => _rb.useGravity = false;
    public void EnableAgent()
    {
        _agent.enabled = true;
        _agent.ResetPath();
    }
    public void DisableAgent() => _agent.enabled = false;
    private void EnableMovement() => _agent.isStopped = false;
    public void DisableMovement() => _agent.isStopped = true;


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
    public void HitEffect()
    {
        _enemy.canAttack = false;
        this.Wait(_enemy.animations.Animator.GetCurrentAnimatorClipInfo(0).Length, () =>
        {
            if (_agent.isActiveAndEnabled)
            {
                EnableMovement();
            }
            _enemy.canAttack = true;
        });
    }

    private void HandleRotation()
    {
        if(_agent.path.corners.Length > 0 && _agent.velocity.magnitude > 0)
        {
            Quaternion rotation = Quaternion.LookRotation(_agent.velocity.normalized); // 5f -> RotationSpeed
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 5f * Time.deltaTime);
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

    public void ApplyCustomGravity(float scale) => _rb.AddForce(Vector3.down * scale * Physics.gravity.magnitude, ForceMode.Acceleration);
    public void ThrowToAir() => _rb.AddForce(Vector3.up * toAirForce.Value, ForceMode.Impulse);
    public float DistanceToPlayer() => (Vector3.Distance(target.position, transform.position)- 0.5f);
    public bool InRangeToChase() => DistanceToPlayer() < chaseSight.Value;

    private void OnDestroy()
    {
        _events.OnIdle -= DisableMovement;
        _events.OnAttacking -= DisableMovement;
        _events.OnPatrolling -= EnableMovement;
        _events.OnFollowing -= EnableMovement;
        _events.OnAir -= DisableMovement;
        _events.OnStun -= DisableMovement;
    }
}
