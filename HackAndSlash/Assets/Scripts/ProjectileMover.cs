
using DamageNumbersPro;
using UnityEngine;

public class ProjectileMover : DamageDealer
{
    public float speed = 15f;
    public float timeToDestroy = 5f;
    public GameObject hit;
    public GameObject[] Detached;
    
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    
    private float _timer;
    private Rigidbody rb;
    public DamageNumber getDamage;
    protected override void Awake()
    {
        GetComponent<Collider>().enabled = false;
        rb = GetComponent<Rigidbody>();
        //rb.AddForce(transform.forward * speed);
    }
    public void Shoot()
    {
        GetComponent<Collider>().enabled = true;
        rb = GetComponent<Rigidbody>();
        Vector3 dirToPlayer = GameManager.Instance.Player.transform.position - transform.position;
        dirToPlayer.y += 0.5f;
        rb.AddForce(dirToPlayer * speed);
    }

    public void ShootToEnemy(Vector3 dir)
    {
        GetComponent<Collider>().enabled = true;
        rb = GetComponent<Rigidbody>();
        rb.AddForce(dir * speed);

    }

    protected override void Update()
    {
        base.Update();
        _timer += Time.deltaTime;
        if (_timer >= timeToDestroy)
        {
            Destroy(gameObject);
        }
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        IDamageable target = other.GetComponent<IDamageable>();
        if (target != null)
        {
            DieEffects();
            target.TakeDamage(damage, getDamage);
            DestroyGameObject(); // Blank unless you override something (ex. for a bullet)
        }
        else if(other.gameObject.layer == LayerMask.GetMask("Ground"))
        {
            DieEffects();
            DestroyGameObject();
        }
        else if (other.gameObject.layer == LayerMask.GetMask("Default"))
        {
            DieEffects();
            DestroyGameObject();
        }
    }
    protected override void DestroyGameObject()
    {
        Destroy(gameObject);
    }
    
    public void DieEffects()
    {
        var hitInstance = Instantiate(hit, this.transform.position, Quaternion.identity);//var hitInstance = Instantiate(hit, pos, rot);
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
