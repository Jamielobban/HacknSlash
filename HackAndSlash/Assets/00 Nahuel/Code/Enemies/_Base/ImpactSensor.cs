using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
public class ImpactSensor : MonoBehaviour
{
    public delegate void CollisionEvent(Collision collision);

    public event CollisionEvent OnCollision;
    private void OnCollisionEnter(Collision collision)
    {
        OnCollision?.Invoke(collision);
    }
}
