using UnityEngine;

public class EnemySensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);
    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;
    public event PlayerExitEvent OnPlayerExit;

    public float minSightDistance, maxSightDistance;
    private float _sightDistance;
    private GameObject _target;
    private bool _playerEnter = false;
    private EnemyBase _enemy;
    private void Start()
    {
        _enemy = transform.parent.parent.GetComponent<EnemyBase>();
        if (_enemy.target != null)
        {
            _target = _enemy.target.gameObject;
        }
        else
        {
            _target = GameManager.Instance.Player.gameObject;
        }
        _sightDistance = Random.Range(minSightDistance, maxSightDistance);
    }

    private void Update()
    {
        float distToPlayer = Vector3.Distance(_target.transform.position, transform.position);
        if (distToPlayer < _sightDistance && !_playerEnter)
        {
            OnPlayerEnter?.Invoke(_target.transform);
            _playerEnter = true;
        }
        else if (distToPlayer > _sightDistance && _playerEnter)
        {
            OnPlayerExit?.Invoke(_target.transform.position);
            _playerEnter = false;
        }
    }
}
