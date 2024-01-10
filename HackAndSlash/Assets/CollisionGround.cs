using UnityEngine;

public class CollisionGround : MonoBehaviour
{
    public bool isGrounded = false;
    private void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.5f, LayerMask.GetMask("Suelo")))
        {
           isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
