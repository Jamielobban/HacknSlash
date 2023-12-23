using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashes : MonoBehaviour
{
    public AttackCollider collider;
    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<AttackCollider>().tag = collider.tag;

        this.GetComponent<AttackCollider>().enemyHitAnim = collider.enemyHitAnim;
        this.GetComponent<AttackCollider>().KnockbackY = collider.KnockbackY;
        this.GetComponent<AttackCollider>().enemyStandUp = collider.enemyStandUp;



        this.GetComponent<AttackCollider>().attack = collider.attack;
        this.GetComponent<AttackCollider>().healthState = collider.healthState;
        this.GetComponent<AttackCollider>().Knockback = collider.Knockback;




        this.GetComponent<AttackCollider>().damage = collider.damage;


        this.GetComponent<AttackCollider>().SetFeedback(collider.enemyHitFeedback);

    }

// Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.forward * speed * Time.deltaTime;
    }
}
