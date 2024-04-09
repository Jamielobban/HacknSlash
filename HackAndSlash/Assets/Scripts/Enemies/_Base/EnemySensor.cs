using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemySensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);
    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;
    public event PlayerExitEvent OnPlayerExit;

    public float sightDistance;
    private GameObject _player;
    private bool _playerEnter = false;

    private void Awake()
    {
        _player = GameManager.Instance.Player.gameObject;
    }

    private void Update()
    {
        if (Vector3.Distance(_player.transform.position, transform.position) < sightDistance && !_playerEnter)
        {
            OnPlayerEnter?.Invoke(_player.transform);
            _playerEnter = true;
        }
        else if (Vector3.Distance(_player.transform.position, transform.position) > sightDistance && _playerEnter)
        {
            OnPlayerExit?.Invoke(_player.transform.position);
            _playerEnter = false;
        }
    }

    // private void OnTriggerEnter(Collider other)
    // {
    //      if (other.TryGetComponent(out PlayerControl player))
    //      {
    //         OnPlayerEnter?.Invoke(other.transform);
    //      }
    // }
    // private void OnTriggerExit(Collider other)
    // {
    //      if (other.TryGetComponent(out PlayerControl player))
    //      {
    //         OnPlayerExit?.Invoke(other.transform.position);
    //      }
    // }

}
