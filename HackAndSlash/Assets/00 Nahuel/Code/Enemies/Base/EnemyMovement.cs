using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
// -- Enemy base class for movements and displacements -- //
public class EnemyMovement : MonoBehaviour
{
    public Transform target;
    protected PlayerControl _player;
    protected NavMeshAgent _agent;
    protected Rigidbody _rb;
    protected Enemy _enemy;
    public NavMeshAgent Agent => _agent;
    public float hitForce;

    public bool _isPathValid;
    public NavMeshPath _path;
    public float chaseSightMin;
    public float chaseSightMax;

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

    [Range(-1, 1)]
    public float movementsPredictionThreshold = 0;
    public bool useMovementPrediction = false;
    protected virtual void Awake()
    {
        chaseSight.baseValue = Random.Range(chaseSightMin, chaseSightMax);
        respawnPoint = transform.position;
        patrollPoint = GetRandomNavmeshPoint();

        _enemy = GetComponent<Enemy>();
        _rb = GetComponent<Rigidbody>();
        _agent = GetComponent<NavMeshAgent>();
        _player = FindObjectOfType<PlayerControl>();
        target = _player.transform;

        //useMovementPrediction = Random.value > 0.5f;
    }

    private void Start()
    {
        _enemy.events.OnIdle += DisableMovement;
        _enemy.events.OnAttacking += DisableMovement;
        _enemy.events.OnPatrolling += EnableMovement;
        _enemy.events.OnFollowing += EnableMovement;
        _enemy.events.OnStun += DisableMovement;
    }

    public Rigidbody GetRigidBody() => _rb;
    public void EnableGravity() => _rb.useGravity = true;
    public void DisableGravity() => _rb.useGravity = false;
    public void EnableAgent()
    {
        _agent.enabled = true;
        _agent.Warp(transform.position);
        _agent.ResetPath();
    }
    public void DisableAgent() => _agent.enabled = false;
    private void EnableMovement() => _agent.isStopped = false;
    public void DisableMovement()
    {
        _agent.velocity = Vector3.zero;
        if(_agent.isActiveAndEnabled)
        {
            _agent.isStopped = true;
        }
    }
    public float predictionTime;
    public void HandleFollow()
    {
        if(_enemy.animations.Animator.GetFloat(Constants.ANIM_VAR_SPEED) < 0.1f)
        {
            StartCoroutine(_enemy.animations.IncreaseOverTime(0f, 1f));
        }
        if (Time.frameCount % 20 == 0 && _enemy._player.states != PlayerControl.States.JUMP)
        {
            _path = new NavMeshPath();
            _isPathValid = _enemy.movements.Agent.CalculatePath(_enemy.movements.target.position, _path);

            if (!_isPathValid && _enemy._player.states != PlayerControl.States.JUMP)
            {
                _enemy.events.Idle();
            }
        }

        if(!useMovementPrediction)
        {
            _agent.destination = target.position;
            HandleRotation();
        }
        else
        {
            float timeToPlayer = Vector3.Distance(_player.transform.position, transform.position) / _agent.velocity.magnitude;
            if(timeToPlayer > predictionTime)
            {
                timeToPlayer = predictionTime;
            }

            Vector3 targetPosition = target.position + _player.rb.velocity * timeToPlayer;
            Vector3 directionToTarget = (targetPosition - transform.position).normalized;
            Vector3 directionToPlayer = (_player.transform.position - transform.position).normalized;

            float dot = Vector3.Dot(directionToPlayer, directionToTarget);
            if(dot < movementsPredictionThreshold)
            {
                targetPosition = _player.transform.position;
            }
            _agent.SetDestination(targetPosition);
        }
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
    public void HitStopEffect()
    {
        _enemy.canAttack = false;
        _enemy.movements.DisableMovement();
        _enemy.movements.DisableAgent();
    }

    public void ResetHit()
    {
        _agent.velocity = Vector3.zero;
        _rb.velocity = Vector3.zero;
        EnableAgent();
        if (_agent.isActiveAndEnabled)
        {
            EnableMovement();
        }
        _enemy.canAttack = true;
    }

    public void HandleRotation()
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

    public bool CheckGround(Enemy e)
    {
        RaycastHit hit;
        if (Physics.Raycast(e.transform.position, Vector3.down, out hit, 1f, LayerMask.GetMask("Suelo")))
        {
            return true;
        }
        return false;
    }

    public void ApplyCustomGravity(float scale) => _rb.AddForce(Vector3.down * scale * Physics.gravity.magnitude, ForceMode.Acceleration);
    public void Throw() => _rb.AddForce(Vector3.up * toAirForce.Value, ForceMode.Impulse);
    public float DistanceToPlayer() => (Vector3.Distance(target.position, transform.position)- 0.5f);
    public bool InRangeToChase() => DistanceToPlayer() < chaseSight.Value;

    private void OnDestroy()
    {
        _enemy.events.OnIdle -= DisableMovement;
        _enemy.events.OnAttacking -= DisableMovement;
        _enemy.events.OnPatrolling -= EnableMovement;
        _enemy.events.OnFollowing -= EnableMovement;
        _enemy.events.OnAir -= DisableMovement;
        _enemy.events.OnStun -= DisableMovement;
    }
}
