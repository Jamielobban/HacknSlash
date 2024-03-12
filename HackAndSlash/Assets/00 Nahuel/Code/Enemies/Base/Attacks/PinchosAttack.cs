using UnityEngine;

public class PinchosAttack : MonoBehaviour
{
    public float damage;
    public float knockbakcForce = 1000;
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IDamageable>()?.TakeDamage(damage);

        GameObject player = other.gameObject.transform.parent.gameObject;

        Vector3 direction = (player.transform.position - transform.position).normalized;

        player.GetComponent<Rigidbody>().AddForce(direction * knockbakcForce, ForceMode.Impulse);
    }
}
