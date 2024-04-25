using UnityEngine;

public class SimpleProjectileSphere : MonoBehaviour {

    public float force;
    public Transform direction;
    private Rigidbody rb;

    void Start () {
        rb = GetComponent<Rigidbody>();
        rb.AddForce((direction.position - transform.position) * force);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, direction.position) < .75f)
        {
            Destroy(gameObject);
        }
    }

}
