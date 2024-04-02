using UnityEngine;
using UnityEngine.AI;
using UnityHFSM;

[RequireComponent(typeof(Animator),typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Header("References")] 
    [SerializeField] private PlayerControl _player;
    
    protected Animator _animator;
    protected NavMeshAgent _agent;
    protected StateMachine<Enums.EnemyStates, Enums.StateEvent> _enemyFSM;
    protected void Awake()
    {
        _player = GameManager.Instance.Player;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
        _enemyFSM = new StateMachine<Enums.EnemyStates, Enums.StateEvent>();
        
        _enemyFSM.AddState(Enums.EnemyStates.Idle, new IdleState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Chase, new ChaseState(false, this, _player.transform));
        _enemyFSM.AddState(Enums.EnemyStates.Wander, new WanderState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Hit, new HitState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Stun, new StunState(false, this, 2));
        _enemyFSM.AddState(Enums.EnemyStates.Attack, new AttackState(false, this, OnAttack));
        _enemyFSM.AddState(Enums.EnemyStates.Countering, new CounteringState(false, this));
        _enemyFSM.AddState(Enums.EnemyStates.Roll, new RollState(false, this, OnRoll));
        _enemyFSM.AddState(Enums.EnemyStates.Dead, new DeadState(false, this));
        
        _enemyFSM.SetStartState(Enums.EnemyStates.Idle);
        _enemyFSM.Init();
    }

    private void OnAttack(State<Enums.EnemyStates, Enums.StateEvent> state) {}
    private void OnRoll(State<Enums.EnemyStates, Enums.StateEvent> state) {}
    
    protected void Update()
    {
        _enemyFSM.OnLogic();
    }
}
