using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class slashes : MonoBehaviour
{
    public AttackCollider collider;
    public float speed;
    public PlayerControl playerControl;

    private GameObject effect;
    // Start is called before the first frame update
    void OnEnable()
    {
        playerControl = FindObjectOfType<PlayerControl>();

        if (playerControl.CheckIfHasItem("Slashes"))
        {
            Debug.Log("asdas");
            this.transform.GetComponent<ParticleSystem>().startLifetime = 2f;
            for (int i = 0; i < this.transform.childCount; i++)
            {

            
                ParticleSystem particleSystem = transform.GetChild(i).GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    particleSystem.startLifetime = 2f;
                }
            }


        }
        else
        {
            this.transform.GetComponent<ParticleSystem>().startLifetime = 0.5f;
            for (int i = 0; i < this.transform.childCount; i++)
            {


                ParticleSystem particleSystem = transform.GetChild(i).GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    particleSystem.startLifetime = 0.5f;
                }
            }
        }

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
        if (playerControl.CheckIfHasItem("Slashes")){
            this.transform.position += this.transform.forward * speed * Time.deltaTime;
        }
    }
}
