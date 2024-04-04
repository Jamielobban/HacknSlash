using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemySensor : MonoBehaviour
{
    public delegate void PlayerEnterEvent(Transform player);
    public delegate void PlayerExitEvent(Vector3 lastKnownPosition);

    public event PlayerEnterEvent OnPlayerEnter;
    public event PlayerExitEvent OnPlayerExit;

    private void OnTriggerEnter(Collider other)
    {
         if (other.TryGetComponent(out PlayerControl player))
         {
            OnPlayerEnter?.Invoke(other.transform);
         }
    }
    private void OnTriggerExit(Collider other)
    {
         if (other.TryGetComponent(out PlayerControl player))
         {
            OnPlayerExit?.Invoke(other.transform.position);
         }
    }

}
