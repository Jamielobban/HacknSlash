using UnityEngine;

public class EnemiesProjetile : MonoBehaviour
{
    public float damage;
    public Vector3 targetPosition;
    public float speed = 15f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    void Start()
    {
        targetPosition = FindObjectOfType<PlayerControl>().transform.position;
        if(Mathf.Abs(Vector3.Distance(targetPosition, transform.position)) <= 1)
        {
            Destroy(gameObject);
        }
        rb = GetComponent<Rigidbody>();
        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
        Destroy(gameObject, 5);
    }
    private void FixedUpdate()
    {
        if (speed != 0)
        {
            Vector3 direction = (targetPosition - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerControl player = other.GetComponent<PlayerControl>();
        if(player != null)
        {
            DieEffects();
            player.GetDamage(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void DieEffects()
    {
        var hitInstance = Instantiate(hit, transform.position, Quaternion.identity);
        if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
        else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }

        var hitPs = hitInstance.GetComponent<ParticleSystem>();
        if (hitPs != null)
        {
            Destroy(hitInstance, hitPs.main.duration);
        }
        else
        {
            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
            Destroy(hitInstance, hitPsParts.main.duration);
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
            }
        }
    }
}
