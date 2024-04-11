using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(SphereCollider))]
public class EnemySensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);
    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;
    public event PlayerExitEvent OnPlayerExit;

    public float minSightDistance, maxSightDistance;
    private float _sightDistance;
    private GameObject _player;
    private bool _playerEnter = false;

    private void Start()
    {
        _player = GameManager.Instance.Player.gameObject;
        _sightDistance = Random.Range(minSightDistance, maxSightDistance);
    }

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < _sightDistance && !_playerEnter)
        {
            OnPlayerEnter?.Invoke(_player.transform);
            _playerEnter = true;
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) > _sightDistance && _playerEnter)
        {
            OnPlayerExit?.Invoke(_player.transform.position);
            _playerEnter = false;
        }
    }
}
