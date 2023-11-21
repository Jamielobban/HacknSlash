using DamageNumbersPro;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.VFX;
using UnityEngine.UI;

public class EnemySkeletonSword : MonoBehaviour
{
    public Slider vida;
    public GameObject canvas;
    public int maxHealth;

    public int health;
    public DamageNumber numberPrefab;
    public DamageNumber burnDamageNumber;
    //public DamageNumber ampedNumberPrefab;
    public DamageNumber critDamageNumberPrefab;
    public int stateMultiplier;

    enum States { MOVE, ATTACK, IDLE, DELAY, JUMP, HIT, DEATH };

    public PlayerControl.HealthState healthState;
    enum Move { WALK, RUN };
    enum Jump { JUMP, AIR, LAND, DELAY };
    enum Hits { UP, LAND, DOWN, HIT1, AIR };
    enum Attack { ATTACK1, ATTACK2 };
    Attack ataques;
    Hits hit;
    Jump jump;
    Move move;
    States state;
    NavMeshAgent agent;
    Animator anim;
    GameObject player;
    bool delay;
    public GameObject[] hitWhiteEffects;
    public GameObject[] bloodEffects;
    public GameObject hitEffect2;
    Vector3 collisionPoint;
    Rigidbody rigidbody;
    public float gravity;
    public float JumpForce;
    public float delayCaer;
    public float delayJumpHit;
    float floor;
    float fallStartTime;
    public AnimationCurve curve;
    public AnimationCurve flyCurve;
    public float speedFlotando;
    public float ImpulsoGolpeAire;
    int hitCount;
    bool GetDamage;

    float flyTimer;
    float attackDelay;
    float actionsDelay;
    float attackTimer;

    public AnimationCurve attack1;
    public AnimationCurve attack2;

    float timeEffectApplied;
    float delayTimer;

    float speedMultiplier;
    public GameObject[] meshEffects;

    int AnimSpeed = 1;

    float hitTime;
    float knockbackForce; 
    Vector3 knockbackForceDir;
    float hitTimeDown;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        vida.value = 1;
        speedMultiplier = 1;
        healthState = PlayerControl.HealthState.NORMAL;
        actionsDelay = Time.time;
        attackDelay = 2;
        GetDamage = true;
        rigidbody = this.GetComponent<Rigidbody>();
        delay = false;
        player = GameObject.FindObjectOfType<PlayerControl>().gameObject;
        anim = this.GetComponent<Animator>();
        ChangeToMove();
        agent = this.GetComponent<NavMeshAgent>();
    }
    void StartWalk()
    {
        if (state == States.HIT)
            return;
        if (Vector3.Distance(this.transform.position, player.transform.position) > agent.stoppingDistance)
        {
            move = Move.WALK;
            state = States.MOVE;
            anim.CrossFadeInFixedTime("walk", 0.2f);
            agent.speed = 2 * speedMultiplier;
        }
        else
        {
            ChangeToIdle();
        }
    }
    void StartRun()
    {
        if (state == States.HIT)
            return;
        if (Vector3.Distance(this.transform.position, player.transform.position) > agent.stoppingDistance)
        {
            move = Move.RUN;
            state = States.MOVE;
            anim.CrossFadeInFixedTime("run", 0.2f);
            agent.speed = 4 * speedMultiplier;

        }
        else
        {
            ChangeToIdle();
        }
    }
    void ChangeToMove()
    {
        int random = (int)Random.RandomRange(0, 5);

        if (random < 3)
        {
            Invoke("StartWalk", 0.3f);

        }
        else
        {
            //anim.CrossFadeInFixedTime("roar", 0.2f);
            Invoke("StartRun", 0.3f);
        }


    }

    void ChangeToIdle()
    {
        attackDelay = 0.25f;

        attackTimer = Time.time;
        GetDamage = true;

        agent.enabled = true;

        state = States.IDLE;
        anim.CrossFadeInFixedTime("idle", 0.2f);
    }


    // Update is called once per frame
    void Update()
    {
        //if (state != States.MOVE)
        //    agent.speed = 0;

        anim.speed = AnimSpeed * speedMultiplier;

        switch (state)
        {
            case States.MOVE:
                switch (move)
                {
                    case Move.WALK:

                        break;
                    case Move.RUN:

                        break;
                }

                if (agent.enabled == true)
                {
                    agent.destination = player.transform.position;
                    if (agent.remainingDistance <= agent.stoppingDistance)
                    {
                        ChangeToIdle();
                    }
                    if (agent.isOnOffMeshLink)
                    {
                        anim.CrossFadeInFixedTime("jump", 0.2f);
                        jump = Jump.JUMP;
                        state = States.JUMP;
                    }
                }
                break;
            case States.IDLE:

                var targetRotation = Quaternion.LookRotation(player.transform.position - this.transform.position);
                this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 3 * Time.deltaTime);


                if (Vector3.Distance(this.transform.position, player.transform.position) > agent.stoppingDistance)
                {
                    ChangeToMove();
                    state = States.DELAY;
                }
                else
                {
                    if ((Time.time - attackTimer) > attackDelay)
                    {
                        int attack = (int)Random.RandomRange(0, 10);

                        attackTimer = Time.time;
                        agent.enabled = false;
                        if (attack < 4)
                        {
                            state = States.ATTACK;
                            ataques = Attack.ATTACK1;
                            anim.CrossFadeInFixedTime("attack1", 0.1f);

                        }
                        else
                        {
                            state = States.ATTACK;

                            ataques = Attack.ATTACK2;
                            anim.CrossFadeInFixedTime("attack2", 0.1f);



                        }
                    }
                }

                break;
            case States.ATTACK:
                //Vector3 look2 = new Vector3(player.transform.position.x, this.transform.position.y, player.transform.position.z);
                //var targetRotation3 = Quaternion.LookRotation(look2 - this.transform.position);
                //this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation3, 3 * Time.deltaTime);

                switch (ataques)
                {
                    case Attack.ATTACK1:
                        rigidbody.AddForce(this.transform.forward * 500 * attack1.Evaluate((Time.time - attackTimer)) * Time.deltaTime, ForceMode.Force);
                        if ((Time.time - attackTimer) > 1f)
                        {
                            ChangeToIdle();
                            attackDelay = Random.RandomRange(1, 3);

                        }

                        break;
                    case Attack.ATTACK2:
                        rigidbody.AddForce(this.transform.forward * 500 * attack2.Evaluate((Time.time - attackTimer)) * Time.deltaTime, ForceMode.Force);
                        if ((Time.time - attackTimer) > 1f)
                        {
                            ChangeToIdle();
                            attackDelay = Random.RandomRange(1, 3);

                        }
                        break;
                }

                break;
            case States.DELAY:

                break;
            case States.JUMP:

                switch (jump)
                {
                    case Jump.JUMP:
                        if (!agent.isOnOffMeshLink)
                        {

                            anim.CrossFadeInFixedTime("land", 0.2f);
                            Invoke("ChangeToIdle", 0.3f);
                            jump = Jump.LAND;


                        }
                        break;
                    case Jump.AIR:

                        break;
                    case Jump.LAND:

                        break;
                    case Jump.DELAY:

                        break;
                }

                break;
            case States.HIT:

                if((Time.time - hitTime) < 0.2f)
                {
                    float value = 1  - (Mathf.Pow(((Time.time-hitTime)/0.2f),2));
                    rigidbody.AddForce(knockbackForceDir * (knockbackForce*(value)) * Time.deltaTime, ForceMode.Force);
                }




                switch (hit)
                {
                    case Hits.UP:
                        ApplyGravity();
                        break;
                    case Hits.DOWN:
                        ApplyGravity();
                        RaycastHit rayhit;

                        if (Physics.Raycast(transform.position, transform.TransformDirection(-this.transform.up), out rayhit, 200, 1 << 7))
                        {

                            if (rayhit.distance < 0.5f)
                            {
                                floor = rayhit.point.y;
                                anim.CrossFadeInFixedTime("GolpeSuelo", 0.2f);
                                this.transform.position = new Vector3(this.transform.position.x, rayhit.point.y, this.transform.position.z);
                                hit = Hits.LAND;
                                Invoke("Levantarse", 0.5f);
                                //GetDamage = false;

                            }
                        }
                        break;
                    case Hits.LAND:

                        break;
                    case Hits.HIT1:

                        break;
                    case Hits.AIR:
                        rigidbody.AddForce(this.transform.up * speedFlotando * curve.Evaluate((Time.time - delayCaer)), ForceMode.Force);
                        if ((Time.time - delayCaer) > 0.25f)
                        {
                            hit = Hits.DOWN;
                            anim.CrossFadeInFixedTime("fall", 0.2f);
                            fallStartTime = Time.time;

                        }
                        break;
                }
                break;
            case States.DEATH:

                if ((Time.time - hitTime) < 0.5f)
                {
                    float value = 1 - (Mathf.Pow(((Time.time - hitTime) / 0.5f), 2));
                    rigidbody.AddForce(knockbackForceDir * (knockbackForce * (value)) * Time.deltaTime, ForceMode.Force);

                }
                if ((Time.time - hitTime) < 2f)
                {
                    float value = (Mathf.Pow(((Time.time - hitTime) / 2f), 2));
                    rigidbody.AddForce(-this.transform.up * (30000 * (value)) * Time.deltaTime, ForceMode.Force);

                }

                break;
        }
        if (state == States.DEATH)
        {
            return;
        }
        if ((Time.time - timeEffectApplied) >= delayTimer && healthState != PlayerControl.HealthState.NORMAL)
        {
            switch (healthState)
            {
                case PlayerControl.HealthState.FROZEN:
                    speedMultiplier = 1f;
                    agent.speed *= 2f;
                    meshEffects[0].SetActive(false);
                    meshEffects[1].SetActive(false);

                    break;
                case PlayerControl.HealthState.BURNED:
                    Debug.Log("Burning off");
                    break;
                case PlayerControl.HealthState.POSIONED:
                    Debug.Log("Poison off");
                    break;
                case PlayerControl.HealthState.AMPED:
                    Debug.Log("Amp off");
                    break;
                case PlayerControl.HealthState.NORMAL:
                    speedMultiplier = 1f;
                    break;
                case PlayerControl.HealthState.WEAKENED:
                    break;
            }
            healthState = PlayerControl.HealthState.NORMAL;
            stateMultiplier = 0;
        }
    }
    void Levantarse()
    {
        if (state == States.DEATH)
        {
            return;
        }
        if (hit != Hits.LAND)
            return;
        anim.CrossFadeInFixedTime("Levantarse", 0.2f);
        this.transform.position = new Vector3(this.transform.position.x, floor, this.transform.position.z);

        Invoke("ChangeToIdle", 0.6f);

    }

    void SpawnWhiteSplash(Vector3 spawnPoint)
    {
        GameObject hitToLook = Instantiate(hitWhiteEffects[Random.Range(0, hitWhiteEffects.Length - 1)], spawnPoint + new Vector3(0f, 1f, 0f), Quaternion.identity);
        hitToLook.transform.LookAt(GameObject.FindGameObjectWithTag("PlayerCenter").transform.position);
    }

    void SpawnBloodSplash(Vector3 spawnPoint)
    {
        GameObject hitToLook = Instantiate(bloodEffects[Random.Range(0, bloodEffects.Length)], spawnPoint + new Vector3(0f, 1f, 0f), Quaternion.identity);
        hitToLook.transform.LookAt(GameObject.FindGameObjectWithTag("PlayerCenter").transform.position);
    }

    void SpawnHitLine(Vector3 spawnPoint)
    {
        GameObject hitToLook = Instantiate(hitEffect2, spawnPoint + new Vector3(Random.Range(0, 0.15f), Random.Range(0.85f, 1.15f), 0f), Quaternion.identity);
        hitToLook.transform.LookAt(GameObject.FindGameObjectWithTag("PlayerCenter").transform.position);
    }
    Vector3 FindClosestPointOnCollider(Collider collider, Vector3 point)
    {
        return collider.ClosestPointOnBounds(point);
    }
    void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, this.gravity * (Time.time - fallStartTime), 0);
        rigidbody.AddForce(gravity * Time.fixedDeltaTime, ForceMode.Force);

    }
    void DelayAire()
    {
        if (state == States.DEATH)
        {
            return;
        }
        anim.CrossFadeInFixedTime("Flotando", 0.2f);
        delayCaer = Time.time;
        hit = Hits.AIR;
    }
    void StandUp()
    {
        if (state == States.DEATH)
        {
            return;
        }
        if ((Time.time - hitTimeDown) < 0.9f)
            return;
        anim.CrossFadeInFixedTime("StandUp", 0.2f);
        Invoke("ChangeToIdleHit", 0.4f);

    }
    void ChangeToIdleHit()
    {
        if (state == States.DEATH)
        {
            return;
        }
        rigidbody.isKinematic = true;

        hitCount--;
        if (hitCount == 0)
        {
            attackDelay = 0.5f;

            GetDamage = true;
            attackTimer = Time.time;

            agent.enabled = true;
            state = States.IDLE;
            anim.CrossFadeInFixedTime("idle", 0.2f);
        }

    }
    int ticksRemainingBurn;
    void Burn()
    {
        if (state == States.DEATH)
        {
            return;
        }
        if (ticksRemainingBurn > 0)
        {
            int damageToDeal = 5 + (1 * GameObject.FindObjectOfType<PlayerControl>().GetItemStacks("Fire Damage Item"));
            this.health -= damageToDeal;
            Debug.Log(damageToDeal);
            DamageNumber burnNumber = burnDamageNumber.Spawn(this.transform.position, -damageToDeal);
            // DamageNumber damageNumber = numberPrefab.Spawn(collisionPoint, -6);
            ticksRemainingBurn--;
            if(ticksRemainingBurn == 0)
            {
                CancelInvoke("Burn");
            }
        }
    }

    int ticksRemainingPoison;
    void Poison()
    {
        if (state == States.DEATH)
        {
            return;
        }
        if (ticksRemainingPoison > 0)
        {
            //int damageToDeal = 1 + (1 * GameObject.FindObjectOfType<PlayerControl>().GetItemStacks("Fire Damage Item"));
            //this.health -= damageToDeal;
            //Debug.Log(damageToDeal);
            DamageNumber burnNumber = burnDamageNumber.Spawn(this.transform.position, -1);
            this.health -= 1;
            // DamageNumber damageNumber = numberPrefab.Spawn(collisionPoint, -6);
            ticksRemainingPoison--;
            if (ticksRemainingPoison == 0)
            {
                CancelInvoke("Poison");
            }
        }
    }

    void HandleEnemyHit(bool isCritical, Vector3 collisionPoint)
    {
        // Instantiate the appropriate DamageNumber prefab based on whether it's a crit or not
        if (isCritical)
        {
            Instantiate(critDamageNumberPrefab, collisionPoint, Quaternion.identity);
        }
        else
        {
            Instantiate(numberPrefab, collisionPoint, Quaternion.identity);
        }

        // You can add additional logic here based on the enemy being hit
        // For example, updating enemy health, playing sound effects, triggering animations, etc.
    }

    void ChangeHealth(int value)
    {
            health += (int)value;
            vida.value = (float)((float)health / (float)maxHealth);
    }
    int hits = 0;
    void RestartHit()
    {
        hits = 0;
    }
    private void OnTriggerEnter(Collider other)
    {

        if (other.GetComponent<AttackCollider>() != null && GetDamage && hits == 0)
        {
            hits++;
            Invoke("RestartHit", 0.05f);

            rigidbody.isKinematic = false;
            if (state == States.DEATH)
            {
                return;
            }
            //health -= (int)other.GetComponent<AttackCollider>().GetCritOrDamage();
            ChangeHealth(-(int)other.GetComponent<AttackCollider>().damage);

            if (player.GetComponent<PlayerControl>().ReturnIfCrit())
            {
                DamageNumber damageNumber = critDamageNumberPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)other.GetComponent<AttackCollider>().damage);
            }
            else
            {
                DamageNumber damageNumber = numberPrefab.Spawn(collisionPoint + new Vector3(0f, 1.5f, 0f), (int)other.GetComponent<AttackCollider>().damage);
            }

            //Debug.Log("Does this always hit");
            if (health <= 0)
            {
                collisionPoint = FindClosestPointOnCollider(other, transform.position);
                //other.GetComponent<AttackCollider>().SubscribeToAttackResult();
                Vector3 ForceDirection = -(GameObject.FindFirstObjectByType<PlayerControl>().transform.GetChild(0).forward).normalized;
                this.transform.LookAt(this.transform.position + ForceDirection);
                hitTime = Time.time;
                knockbackForce = 30000;
                //rigidbody.AddForce(-ForceDirection * other.GetComponent<AttackCollider>().Knockback * Time.fixedDeltaTime, ForceMode.Impulse);
                knockbackForceDir = -ForceDirection;
                agent.enabled = false;
                other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();
                state = States.DEATH;
                SpawnWhiteSplash(collisionPoint);
                SpawnBloodSplash(collisionPoint);
                SpawnHitLine(collisionPoint);
                //ComboManager.instance.IncreaseCombo();
                AbilityPowerManager.instance.IncreaseCombo();
                anim.CrossFadeInFixedTime("Death", 0.2f);
                GetDamage = false;
                this.gameObject.layer = 9;
                this.gameObject.tag = "Untagged";
                canvas.SetActive(false);
                return;
            }

            if (healthState == other.GetComponent<AttackCollider>().healthState)
                {
                    stateMultiplier++;
                    timeEffectApplied = Time.time;
                    if (stateMultiplier >= 1)
                    {
                        delayTimer = 5;
                    }
                }
                else if (other.GetComponent<AttackCollider>().healthState != PlayerControl.HealthState.NORMAL)
                {
                    timeEffectApplied = Time.time;
                    stateMultiplier = 0;
                    healthState = other.GetComponent<AttackCollider>().healthState;
                    switch (healthState)
                    {
                        case PlayerControl.HealthState.FROZEN:
                            speedMultiplier = 0.5f;
                            meshEffects[0].SetActive(true);
                            meshEffects[1].SetActive(true);

                            Debug.Log("Ice applied");
                            break;
                        case PlayerControl.HealthState.BURNED:
                            ticksRemainingBurn = 2;
                            InvokeRepeating("Burn", 0.5f, 1f);
                            //fire effect.setactive true;
                            Debug.Log("Burning Applied");
                            break;
                        case PlayerControl.HealthState.POSIONED:
                            ticksRemainingPoison = 5;
                            InvokeRepeating("Poison", 0.5f, 0.25f);
                            Debug.Log("Poison Applied");
                            break;
                        case PlayerControl.HealthState.AMPED:
                            StartCoroutine(this.gameObject.GetComponent<AmpExplosion>().AmpExplosionDelay());
                            Debug.Log("Amp applied");
                            //electric effect.setactive true;
                            break;
                        case PlayerControl.HealthState.NORMAL:
                            speedMultiplier = 1f;
                            break;
                        case PlayerControl.HealthState.WEAKENED:
                            break;
                    }

                    delayTimer = 3;
                }
            if (state == States.JUMP)
            {
                delayCaer = Time.time;

                rigidbody.AddForce(this.transform.up * ImpulsoGolpeAire * Time.fixedDeltaTime, ForceMode.Impulse);
                anim.CrossFadeInFixedTime("AirDamage", 0.2f);

                other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();

                collisionPoint = FindClosestPointOnCollider(other, transform.position);
                SpawnWhiteSplash(collisionPoint);
                SpawnBloodSplash(collisionPoint);
                SpawnHitLine(collisionPoint);
                //ComboManager.instance.IncreaseCombo();
                AbilityPowerManager.instance.IncreaseCombo();

                fallStartTime = Time.time;
                state = States.HIT;
                hit = Hits.AIR;
            }
            else
            {
    

                agent.enabled = false;

                if (other.CompareTag("GolpeVertical"))
                {
                    fallStartTime = Time.time;

                    anim.CrossFadeInFixedTime("GolpeSalto", 0.2f);

                    other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();

                    collisionPoint = FindClosestPointOnCollider(other, transform.position);
                    SpawnWhiteSplash(collisionPoint);
                    SpawnBloodSplash(collisionPoint);
                    SpawnHitLine(collisionPoint);
                    //ComboManager.instance.IncreaseCombo();
                    AbilityPowerManager.instance.IncreaseCombo();
                    //health -= (int)other.GetComponent<AttackCollider>().GetCritOrDamage();

                    rigidbody.AddForce(this.transform.up * JumpForce, ForceMode.Impulse);

                    Invoke("DelayAire", delayJumpHit);

                    state = States.HIT;
                    hit = Hits.UP;

                }
                else if (other.CompareTag("GolpeAire"))
                {
                    delayCaer = Time.time;

                    rigidbody.AddForce(this.transform.up * ImpulsoGolpeAire * Time.fixedDeltaTime, ForceMode.Impulse);
                    anim.CrossFadeInFixedTime("AirDamage", 0.2f);

                    other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();

                    collisionPoint = FindClosestPointOnCollider(other, transform.position);
                    SpawnWhiteSplash(collisionPoint);
                    SpawnBloodSplash(collisionPoint);
                    SpawnHitLine(collisionPoint);
                    //ComboManager.instance.IncreaseCombo();
                    AbilityPowerManager.instance.IncreaseCombo();
                    //health -= (int)other.GetComponent<AttackCollider>().GetCritOrDamage();
                    fallStartTime = Time.time;
                    state = States.HIT;
                    hit = Hits.AIR;
                }
                else if (other.CompareTag("GolpeVerticalCircular"))
                {
                    rigidbody.AddForce(this.transform.up * other.GetComponent<AttackCollider>().KnockbackY * Time.fixedDeltaTime, ForceMode.Impulse);
                    anim.CrossFadeInFixedTime("GolpeSalto", 0.2f);
                    fallStartTime = Time.time;
                    other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();


                    collisionPoint = FindClosestPointOnCollider(other, transform.position);
                    SpawnWhiteSplash(collisionPoint);
                    SpawnBloodSplash(collisionPoint);
                    SpawnHitLine(collisionPoint);
                    //health -= (int)other.GetComponent<AttackCollider>().GetCritOrDamage();
                    //ComboManager.instance.IncreaseCombo();
                    AbilityPowerManager.instance.IncreaseCombo();
                    state = States.HIT;
                    hit = Hits.UP;
                    Invoke("DelayAire", delayJumpHit);
                }
                else if (other.GetComponent<AttackCollider>() != null)
                {
                    hitCount++;

                    if (other.GetComponent<AttackCollider>().enemyStandUp)
                    {
                        Invoke("StandUp", 1f);
                        hitTimeDown = Time.time;
                    }
                    else
                    {
                        Invoke("ChangeToIdleHit", 0.5f);

                    }
                    
                    state = States.HIT;
                    hit = Hits.HIT1;

                    anim.CrossFadeInFixedTime(other.GetComponent<AttackCollider>().enemyHitAnim, 0.2f);

                    rigidbody.AddForce(this.transform.up * other.GetComponent<AttackCollider>().KnockbackY * Time.fixedDeltaTime, ForceMode.Impulse);

                    other.GetComponent<AttackCollider>().enemyHitFeedback?.PlayFeedbacks();


                    //Vector3 player = new Vector3(GameObject.FindGameObjectWithTag("PlayerCenter").transform.position.x, 0, GameObject.FindGameObjectWithTag("PlayerCenter").transform.position.z);
                    //Vector3 enemy = new Vector3(this.transform.position.x, 0, this.transform.position.z);

                    Vector3 ForceDirection = -(GameObject.FindFirstObjectByType<PlayerControl>().transform.GetChild(0).forward).normalized;
                    this.transform.LookAt(this.transform.position + ForceDirection);

                    hitTime = Time.time;
                    knockbackForce = other.GetComponent<AttackCollider>().Knockback;
                    //rigidbody.AddForce(-ForceDirection * other.GetComponent<AttackCollider>().Knockback * Time.fixedDeltaTime, ForceMode.Impulse);
                    knockbackForceDir = -ForceDirection;
                    // Vector3 collisionPosition = (GameObject.FindGameObjectWithTag("PlayerCenter").transform.position - (this.transform.position + new Vector3(0f, 2f, 0f))).normalized;
                    collisionPoint = FindClosestPointOnCollider(other, transform.position);
                    SpawnWhiteSplash(collisionPoint);
                    SpawnBloodSplash(collisionPoint);
                    SpawnHitLine(collisionPoint);
                    AbilityPowerManager.instance.IncreaseCombo();
                    //health -= (int)other.GetComponent<AttackCollider>().GetCritOrDamage();
                    //if (other.transform.GetComponentInParent<PlayerControl>() != null)
                    //{
                    //    other.transform.GetComponentInParent<PlayerControl>().CallItemOnHit(this);
                    //}
                    //DamageNumber damageNumber = numberPrefab.Spawn(collisionPoint, GameObject.FindFirstObjectByType<PlayerControl>().attackDamage);
                }
            }
        }

    }
}
