using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using UnityEngine.VFX;
using System.Linq;
using UnityEngine.UIElements;

public class PlayerControl : MonoBehaviour
{
    public int health;
    public List<ItemList> items = new List<ItemList>();

    public enum HealthState { FROZEN, BURNED, POSIONED, WEAKENED, NORMAL };

    enum States { MOVE, DASH, JUMP, ATTACK, IDLE, DELAYMOVE };

    enum Attacks { GROUND, AIR, RUN, FALL };

    enum Moves { WALK, RUN, IDLE };

    enum Dash { START, DASH, DOUBLEDASH, AIRDASH, END };

    enum Jump { JUMP, FALL, LAND };

    States states;
    Attacks attacks;
    Moves moves;
    Dash dash;
    Jump jump;

    public GameObject camera;
    public float CameraRotatSpeed;
    public float walkSpeed;
    public float runSpeed;
    public float walkSpeedAir;
    public float runSpeedAir;

    public GameObject movementController;
    public GameObject player;

    Vector3 moveDir;
    Vector3 moveDirSaved;

    Animator playerAnim;

    float delayMove;

    public float delayIdleToMoveTime;

    public enum ComboAtaques { Quadrat, HoldQuadrat, Triangle, HoldTriangle, combo5, air1, air2, run1, run2 };

    public int currentScroll;
    [System.Serializable]
    public struct Ataques
    {
        public float ataque;
        public float delay;
        public bool nextAttack;
        public MMFeedbacks effects;

        public MMFeedbacks enemyFeedback;

        public string name;

        public float transition;

        public AnimationCurve curvaDeVelocidadMovimiento;
        public float velocidadMovimiento;

        public AnimationCurve curvaDeVelocidadMovimientoY;
        public float velocidadMovimientoY;

        public float delayFinal;

        public GameObject collider;
        public string colliderTag;
        public float delayGolpe;

        public Vector2 EnemyKnockBackForce;
        public string enemyHitAnim;

        public bool EnemyStandUp;
        public int repeticionGolpes;
        public float delayRepeticionGolpes;

        //1 is light, 2 is heavy
        public GameObject BlessingToSpawnHeavy;
        //public GameObject BlessingToSpawnHeavy;
        //public GameObject BlessingToSpawnHeavy;
        //pickup
    }
    [System.Serializable]
    public class ListaAtaques
    {
        public ComboAtaques combo;
        public Ataques[] attacks;
        public Blessing[] blessing;
    }
    public ListaAtaques[] ataques;
    ListaAtaques currentComboAttacks;
    public ListaAtaques[] airCombo;
    public ListaAtaques[] runCombo;

    int currentComboAttack;

    float attackStartTime;
    float fallStartTime;
    public float gravity;
    float timeJumping;
    float timeLanding;

    bool doubleJump;
    public float jumpForce;
    public float distanceFloor;

    public float delayCombos;
    float comboFinishedTime;
    bool attackFinished;

    bool cuadrado;
    bool triangulo;

    float dealyAttackFall;
    ControllerManager controller;
    float delayDash;
    public float dashSpeed;
    Vector2 dashDirection;
    public float delayDashes;
    public GetEnemies enemieTarget;

    public GameObject[] dashEffects;

    bool dashDown;

    float landHeight;

    bool stopAttack;

    public DoubleJumpVFXController doubleJumpVFX;
    public LandingVFXController landVFX;
    public SingleJumpVFXController singleJumpVFX;

    public int dashConsecutivos;
    public int dashCount;

    public enum PassiveCombo
    {
        QUADRATFLOOR,
        TRIANGLEFLOOR,
        HOLDQUADRATFLOOR,
        HOLDTRIANGLEFLOOR
    }
    public List<PassiveCombo> passiveCombo = new List<PassiveCombo>();
    // Start is called before the first frame update
    void Start()
    {
       
        //HealingArea item = new HealingArea();
        //items.Add(new ItemList(item, item.GiveName(), 1));
        StartCoroutine(CallItemUpdate()); 
        dashCount = 0;
        attackFinished = false;
        attackFinished = false;
        controller = GameObject.FindAnyObjectByType<ControllerManager>().GetComponent<ControllerManager>();
        currentComboAttack = -1;
        playerAnim = player.GetComponent<Animator>();

        states = States.IDLE;
        moves = Moves.IDLE;
    }

    public int GetStacks(Item item)
    {
        foreach (ItemList i in items)
        {
            if (i.item != null && i.item.GiveName() == item.GiveName())
            {
                i.GetStacks();
            }
        }
        return 0; // Default value if the item is not found
    }

    ListaAtaques GetAttacks(ComboAtaques combo)
    {
        for (int i = 0; i < ataques.Length; i++)
        {
            if (ataques[i].combo == combo)
            {
                return ataques[i];
            }
        }
        for (int i = 0; i < airCombo.Length; i++)
        {
            if (airCombo[i].combo == combo)
            {
                return airCombo[i];
            }
        }


        for (int i = 0; i < runCombo.Length; i++)
        {
            if (runCombo[i].combo == combo)
            {
                return runCombo[i];
            }
        }


        return null;
    }
    void AcabarAtaqueCaida()
    {
        CheckIfReturnIdle();
        CheckIfStartMove();
        CheckIfIsFalling();
    }

    bool CheckIfNextAttack()
    {

        if (currentComboAttacks.combo == ComboAtaques.air2)
        {

            if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].ataque && attacks == Attacks.AIR)
            {
                attacks = Attacks.FALL;
                playerAnim.CrossFadeInFixedTime("FallAttack", 0.1f);

            }
            else if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].ataque && attacks == Attacks.FALL)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
                {
                    if (hit.distance < 0.5f)
                    {
                        playerAnim.speed = 1;
                        if (currentComboAttacks.attacks[currentComboAttack].effects != null)
                            currentComboAttacks.attacks[currentComboAttack].effects.PlayFeedbacks();

                        currentComboAttacks.attacks[currentComboAttack].collider.GetComponent<AttackCollider>().enemyHitAnim = currentComboAttacks.attacks[currentComboAttack].enemyHitAnim;
                        currentComboAttacks.attacks[currentComboAttack].collider.GetComponent<AttackCollider>().KnockbackY = currentComboAttacks.attacks[currentComboAttack].EnemyKnockBackForce.y;
                        currentComboAttacks.attacks[currentComboAttack].collider.GetComponent<AttackCollider>().enemyStandUp = currentComboAttacks.attacks[currentComboAttack].EnemyStandUp;

                        currentComboAttacks.attacks[currentComboAttack].collider.GetComponent<AttackCollider>().Knockback = currentComboAttacks.attacks[currentComboAttack].EnemyKnockBackForce.x;
                        currentComboAttacks.attacks[currentComboAttack].collider.GetComponent<AttackCollider>().SetFeedback(currentComboAttacks.attacks[currentComboAttack].enemyFeedback);
                        currentComboAttacks.attacks[currentComboAttack].collider.tag = currentComboAttacks.attacks[currentComboAttack].colliderTag;
                        currentComboAttacks.attacks[currentComboAttack].collider.SetActive(true);
                        StartCoroutine(DesactivarCollisionGolpe(0.05f, currentComboAttack));

                        playerAnim.CrossFadeInFixedTime("LandAttack", 0.1f);
                        doubleJump = false;
                        Debug.Log("Doouble Jump");
                        comboFinishedTime = Time.time;
                        attackFinished = true;
                        delayCombos = currentComboAttacks.attacks[currentComboAttack].delayFinal;
                        dealyAttackFall = Time.time;
                        states = States.IDLE;
                        currentComboAttack = -1;
                        moveDirSaved = new Vector3();
                        landHeight = hit.point.y;
                        //this.transform.position = new Vector3(this.transform.position.x, landHeight + 0.2f, this.transform.position.z);

                        return true;
                    }

                }
            }
            if (CheckIfDash())
            {
                dashDown = true;
            }
            return true;
        }

        if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].ataque)
        {

            playerAnim.speed = 1;

            if (currentComboAttack + 1 == currentComboAttacks.attacks.Length)
            {
                comboFinishedTime = Time.time;
                attackFinished = true;
                delayCombos = currentComboAttacks.attacks[currentComboAttack].delayFinal;

                currentComboAttack = -1;
            }
            if (currentComboAttacks.combo != ComboAtaques.air1 && currentComboAttacks.combo != ComboAtaques.air2 && currentComboAttacks.combo != ComboAtaques.HoldQuadrat && currentComboAttacks.combo != ComboAtaques.HoldTriangle)
            {
                CheckIfReturnIdle();
                CheckMove();
            }
            else
            {
                fallStartTime = Time.time;
                if (CheckIfLand())
                {

                    return true;
                }

                CheckIfIsFalling();




            }

            return true;
        }
        else
        {
            return false;
        }
    }
    void ApplyGravity()
    {
        Vector3 gravity = new Vector3(0, this.gravity * (Time.time - fallStartTime), 0);

        //if((landHeight <= 0 && ((this.transform.position.y-landHeight) >= 0) && Mathf.Abs(this.transform.position.y - landHeight) < (4.5f* (((Time.time - fallStartTime))))) || (landHeight > 0 && ((this.transform.position.y - landHeight) > 0) && Mathf.Abs(this.transform.position.y - landHeight) < (4.5f * (((Time.time - fallStartTime))))))
        //{
        //    this.transform.position = new Vector3(this.transform.position.x, landHeight+0.1f, this.transform.position.z);
        //}
        //else
        //{
        this.GetComponent<Rigidbody>().AddForce(gravity * Time.fixedDeltaTime, ForceMode.Force);
        //}

    }
    void moveInAir(float vel)
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.TransformDirection(this.transform.GetChild(0).forward), out hit, 200))
        {
            if (hit.distance < 1)
            {
                return;
            }

            // Smoothly rotate towards the target point.


        }
        player.transform.LookAt(player.transform.position + moveDirSaved);
        if (moves == Moves.IDLE)
            this.GetComponent<Rigidbody>().AddForce(moveDirSaved * 0 * Time.deltaTime, ForceMode.Force);
        else
            this.GetComponent<Rigidbody>().AddForce(moveDirSaved * vel * Time.deltaTime, ForceMode.Force);
    }

    private bool AreListsEqual<T>(List<T> listA, List<T> listB)
    {
        return listA.SequenceEqual(listB);
    }
    private IEnumerator DelayGolpe(float time, int golpe)
    {

        yield return new WaitForSeconds(time);

        if (states == States.ATTACK && !stopAttack)
        {
            if (currentComboAttacks.attacks[golpe].effects != null)
            {
                currentComboAttacks.attacks[golpe].effects.PlayFeedbacks();
            }
            bool hasApplied = false;
            if (currentComboAttacks.blessing != null)
            {
                foreach (Blessing i in currentComboAttacks.blessing)
                {
                    if (AreListsEqual(i.passiveCombo, passiveCombo))
                    {
                        hasApplied = true;
                        //i.spawnEffect(player.transform.position + (player.transform.forward * 2));
                        GameObject effect = Instantiate(i.visualEffect, player.transform.position + (player.transform.forward * 2),Quaternion.identity);
                        Collider effectCollider = effect.GetComponent<Collider>();
                        effect.GetComponent<AttackCollider>().healthState = i.healthState;
                        effect.GetComponent<AttackCollider>().enemyHitAnim = currentComboAttacks.attacks[golpe].enemyHitAnim;
                        effect.GetComponent<AttackCollider>().KnockbackY = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.y;
                        effect.GetComponent<AttackCollider>().enemyStandUp = currentComboAttacks.attacks[golpe].EnemyStandUp;

                        effect.GetComponent<AttackCollider>().Knockback = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.x;

                        effect.GetComponent<AttackCollider>().SetFeedback(currentComboAttacks.attacks[golpe].enemyFeedback);
                        effect.tag = currentComboAttacks.attacks[golpe].colliderTag;
                        effectCollider.enabled = true;
                        StartCoroutine(DesactivarCollisionGolpeBlessing(1f, effectCollider));

                    }
                }
            }
            if (!hasApplied)
            {
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().healthState = HealthState.NORMAL;
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyHitAnim = currentComboAttacks.attacks[golpe].enemyHitAnim;
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().KnockbackY = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.y;
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyStandUp = currentComboAttacks.attacks[golpe].EnemyStandUp;

                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().Knockback = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.x;

                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().SetFeedback(currentComboAttacks.attacks[golpe].enemyFeedback);
                currentComboAttacks.attacks[golpe].collider.tag = currentComboAttacks.attacks[golpe].colliderTag;
                currentComboAttacks.attacks[golpe].collider.SetActive(true);
                StartCoroutine(DesactivarCollisionGolpe(0.05f, golpe));
            }
        }


    }
    private IEnumerator DesactivarCollisionGolpe(float time, int golpe)
    {

        yield return new WaitForSeconds(time);
        currentComboAttacks.attacks[golpe].collider.SetActive(false);

    }

    private IEnumerator DesactivarCollisionGolpeBlessing(float time, Collider golpe)
    {

        yield return new WaitForSeconds(time);
        golpe.enabled = false;

    }

    public void SpawnObjectWithDelay(float delay)
    {
        Invoke("SpawnObject", delay); // Invoke the method after the specified delay
    }
    private void SpawnObject()
    {
        Instantiate(currentComboAttacks.attacks[currentComboAttack].BlessingToSpawnHeavy, transform.GetChild(0).position + new Vector3(0f, 0f, 5f), Quaternion.identity);
    }
    void PlayAttack()
    {

        playerAnim.speed = 1.5f;
        currentComboAttack++;

        //if ((currentComboAttack == currentComboAttacks.attacks.Count() - 1) 
        //    /*currentComboAttacks.attacks[currentComboAttack].effectToSpawn != null*/)
        //{
        //    BlessingFactory.Instance.SpawnHeavyAttackBlessing(this.transform.position, Quaternion.identity);
        //}
        if (currentComboAttacks.attacks[currentComboAttack].BlessingToSpawnHeavy != null)
        {
            DynamicCameraControl.Instance.ChangeTopRigHeightAndReset(2f, 1f, 0.5f);
            SpawnObjectWithDelay(0.5f);
        }
        if (currentComboAttacks.attacks[currentComboAttack].collider != null && currentComboAttacks.combo != ComboAtaques.air2)
        {

            for (int i = 0; i <= currentComboAttacks.attacks[currentComboAttack].repeticionGolpes; i++)
            {
                stopAttack = false;
                StartCoroutine(DelayGolpe(currentComboAttacks.attacks[currentComboAttack].delayGolpe + (i * currentComboAttacks.attacks[currentComboAttack].delayRepeticionGolpes), currentComboAttack));


            }
        }

        playerAnim.CrossFadeInFixedTime(currentComboAttacks.attacks[currentComboAttack].name, currentComboAttacks.attacks[currentComboAttack].transition);

        attackStartTime = Time.time;
    }
    void AttackMovement()
    {
        if (currentComboAttack == -1)
            currentComboAttack = 0;
        this.GetComponent<Rigidbody>().AddForce(this.transform.up * currentComboAttacks.attacks[currentComboAttack].curvaDeVelocidadMovimientoY.Evaluate(Time.time - attackStartTime) * currentComboAttacks.attacks[currentComboAttack].velocidadMovimientoY * Time.deltaTime, ForceMode.Force);

        player.transform.GetChild(3).transform.localPosition += new Vector3(0, 0, 1).normalized;
        Vector3 dir = this.transform.position - player.transform.GetChild(3).transform.position;
        //player.transform.LookAt(movementController.transform.position);
        this.GetComponent<Rigidbody>().AddForce(-dir.normalized * currentComboAttacks.attacks[currentComboAttack].curvaDeVelocidadMovimiento.Evaluate(Time.time - attackStartTime) * currentComboAttacks.attacks[currentComboAttack].velocidadMovimiento * Time.deltaTime, ForceMode.Force);
        player.transform.GetChild(3).transform.localPosition = new Vector3();

    }

    bool CheckIfIsFalling()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
        {
            if (hit.distance > distanceFloor)
            {
                fallStartTime = Time.time;
                states = States.JUMP;
                jump = Jump.FALL;
                if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                    playerAnim.CrossFadeInFixedTime("Fall", 0.2f);

                return true;

            }
            else if (hit.distance < 0)
            {
                //this.transform.position = new Vector3(this.transform.position.x, hit.point.y+0.1f, this.transform.position.z);
                doubleJump = false;

                return CheckIfLand();
            }
            else
            {
                //this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.1f, this.transform.position.z);
                doubleJump = false;

                return false;
            }

        }
        return false;



    }

    ComboAtaques GetCurrentAttackCombo()
    {
        return currentComboAttacks.combo;
    }
    bool CheckIfJump()
    {

        if (controller.CheckIfJump())
        {
            this.GetComponent<Rigidbody>().drag = 5;

            if (states != States.JUMP)
            {
                singleJumpVFX.PlaySingleJumpVFX(this.transform.position);
                timeJumping = Time.time;
                fallStartTime = Time.time;
                states = States.JUMP;
                jump = Jump.JUMP;
                playerAnim.CrossFadeInFixedTime("Jump", 0.1f);
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
                return true;

            }
            else if (!doubleJump)
            {
                Move(1);
                doubleJumpVFX.PlayDoubleJumpVFX(this.transform.position + new Vector3(0f, 4f, 0f));
                doubleJump = true;
                timeJumping = Time.time;
                fallStartTime = Time.time;
                states = States.JUMP;
                jump = Jump.JUMP;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().AddForce(this.transform.up * jumpForce * 1.5f, ForceMode.Impulse);
                playerAnim.CrossFadeInFixedTime("DoubleJump", 0.2f);
                return true;

            }


        }

        return false;
    }
    bool CheckIfLand()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
        {
            landHeight = hit.point.y;

            if ((hit.distance < 2 * ((gravity * (Time.time - fallStartTime)) / gravity) || hit.distance < 0.5f))
            {
                timeLanding = Time.time;
                if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                    playerAnim.CrossFadeInFixedTime("Land", 0.2f);
                states = States.JUMP;
                jump = Jump.LAND;
                doubleJump = false;
                moveDirSaved = new Vector3();
                return true;
            }

        }
        return false;
    }
    private IEnumerator DashEffectDisable(float time, int dash)
    {
        yield return new WaitForSeconds(time);
        dashEffects[dash].SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if (!controller.GetController())
            return;

        RotateCamera();
        switch (states)
        {
            case States.IDLE:
                var a = this.transform.position;
                if (attacks == Attacks.FALL && (Time.time - dealyAttackFall) < 0.5f)
                    break;
                else if (attacks == Attacks.FALL && (Time.time - dealyAttackFall) >= 0.5f)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, landHeight + 0.2f, this.transform.position.z);

                    attacks = Attacks.GROUND;
                    playerAnim.CrossFadeInFixedTime("Idle", 0.2f);

                }
                if (CheckIfDash())
                {
                    dashDown = false;
                    break;
                }
                if (CheckAtaques())
                    break;
                if (CheckIfIsFalling())
                    break;
                if (CheckIfJump())
                    break;
                CheckIfStartMove();
                this.GetComponent<Rigidbody>().drag = 15;

                break;
            case States.ATTACK:
                this.GetComponent<Rigidbody>().drag = 15;
                CheckNextAttack();
                AttackMovement();


                if (CheckIfNextAttack())
                    break;
                switch (attacks)
                {
                    case Attacks.GROUND:
                        if (CheckIfDash())
                        {
                            dashDown = false;
                            break;
                        }
                        break;
                    case Attacks.AIR:
                        if (CheckIfDash())
                        {
                            dashDown = true;
                            break;
                        }

                        break;
                    case Attacks.FALL:
                        if (CheckIfDash())
                        {
                            dashDown = true;
                            break;
                        }

                        break;
                    case Attacks.RUN:
                        if (CheckIfDash())
                        {
                            dashDown = false;
                            break;
                        }
                        break;
                }
                break;
            case States.DASH:
                this.GetComponent<Rigidbody>().drag = 15;
                this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, this.transform.GetChild(0).transform.localEulerAngles.y, 0);

                if (CheckAtaques())
                    break;
                switch (dash)
                {
                    case Dash.START:
                        if ((Time.time - delayDash) > 0.05f)
                        {
                            playerAnim.speed = 1;
                            for (int i = 0; i < dashEffects.Length; i++)
                            {
                                if (dashEffects[i].activeSelf == false)
                                {
                                    dashEffects[i].SetActive(true);
                                    StartCoroutine(DashEffectDisable(2, i));
                                    break;
                                }
                            }



                            if (dashDirection == new Vector2(0, -1))
                            {
                                player.transform.GetChild(3).transform.localPosition += new Vector3(dashDirection.x, 0, dashDirection.y).normalized;
                                Vector3 dir = this.transform.position - player.transform.GetChild(3).transform.position;

                                this.GetComponent<Rigidbody>().AddForce(dir * dashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

                                player.transform.GetChild(3).transform.localPosition = new Vector3();
                            }
                            else
                            {
                                movementController.transform.localPosition -= new Vector3(dashDirection.x, 0, dashDirection.y).normalized;
                                Vector3 dir = this.transform.position - movementController.transform.position;
                                this.GetComponent<Rigidbody>().AddForce(dir * dashSpeed * Time.fixedDeltaTime, ForceMode.Impulse);

                                movementController.transform.localPosition = new Vector3();
                            }



                            delayDash = Time.time;
                            dash = Dash.DASH;

                        }
                        break;
                    case Dash.DASH:
                        if ((Time.time - delayDash) > 0.05f)
                        {
                            delayDash = Time.time;

                            dash = Dash.END;

                            playerAnim.CrossFadeInFixedTime("DashAparecer", 0.2f);

        

                        }

                        if (dashDown)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
                            {
                                if ((hit.distance > 0.5f))
                                {
                                    this.GetComponent<Rigidbody>().AddForce(-this.transform.up * 15000 * Time.fixedDeltaTime, ForceMode.Force);
                                }
                            }
                        }
                        break;
                    case Dash.DOUBLEDASH:

                        break;
                    case Dash.AIRDASH:

                        break;
                    case Dash.END:
                        if ((Time.time - delayDash) > 0.05f)
                        {
                            for (int i = 0; i < dashEffects.Length; i++)
                            {
                                if (dashEffects[i].activeSelf == false)
                                {
                                    dashEffects[i].SetActive(true);
                                    StartCoroutine(DashEffectDisable(2, i));
                                    break;

                                }
                            }
                            delayDash = Time.time;
                            moveDirSaved = new Vector3();

                            if (CheckIfIsFalling())
                                break;
                            CheckIfReturnIdle();

                            //CheckIfStartMove();
                            CheckMove();

                        }
                        break;
                }

                break;
            case States.JUMP:
                ApplyGravity();
                this.GetComponent<Rigidbody>().drag = 5;
                if (CheckIfDash())
                {
                    dashDown = true;

                    break;
                }
                switch (moves)
                {
                    case Moves.WALK:
                        moveInAir(walkSpeedAir);

                        break;
                    case Moves.RUN:
                        moveInAir(runSpeedAir);
                        break;
                }
                switch (jump)
                {

                    case Jump.JUMP:

                        if (((Time.time - timeJumping) > 0.2f && !doubleJump) || ((Time.time - timeJumping) > 0.4f && doubleJump))
                        {
                            playerAnim.CrossFadeInFixedTime("Fall", 0.2f);

                            jump = Jump.FALL;
                        }

                        break;
                    case Jump.FALL:
                        if (CheckIfJump())
                            break;
                        if (CheckAtaques())
                            break;
                        if (CheckIfLand())
                            break;
                        break;
                    case Jump.LAND:
                        if ((Time.time - timeLanding) > 0.10f)
                        {
                            //Debug.Log("Landed");
                            //landVFX.transform.position = this.transform.position;
                            //landVFX.Play();
                            landVFX.PlayDustVFX(this.transform.position);
                            CallItemOnJump();
                            player.transform.GetChild(1).Rotate(new Vector3(0, 1, 0), -90);
                            playerAnim.CrossFadeInFixedTime("Idle", 0.2f);
                            //this.transform.position = new Vector3(this.transform.position.x, landHeight+0.2f, this.transform.position.z);
                            //states = States.IDLE;
                            CheckIfReturnIdle();
                            CheckMove();
                        }
                        break;
                }
                break;
            case States.MOVE:
                this.GetComponent<Rigidbody>().drag = 15;
                if (CheckIfDash())
                {
                    dashDown = false;

                    break;
                }
                if (CheckIfIsFalling())
                    break;
                if (CheckAtaques())
                    break;
                if (CheckIfJump())
                    break;
                switch (moves)
                {
                    case Moves.WALK:
                        Move(walkSpeed);
                        //Debug.Log("Walking");
                        //walkVFX.Play();
                        break;
                    case Moves.RUN:
                        Move(runSpeed);
                        //Debug.Log("Running");
                        break;
                    default:
                        Move(0);
                        break;
                }

                CheckIfReturnIdle();
                CheckMove();

                break;
            case States.DELAYMOVE:
                if (CheckIfDash())
                {
                    dashDown = false;

                    break;
                }
                if (CheckAtaques())
                    break;
                CheckIfReturnIdle();

                if (Time.time - delayMove > delayIdleToMoveTime)
                {
                    CheckMove();
                }
                break;
        }
    }

    IEnumerator CallItemUpdate()
    {
        foreach(ItemList i in items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    public void CallItemOnHit(Enemy1 enemy)
    {
        foreach (ItemList i in items)
        {
            i.item.OnHit(this, enemy, i.stacks);
        }
    }

    public void CallItemOnJump()
    {
        foreach (ItemList i in items)
        {
            i.item.OnJump(this, i.stacks);
        }
    }

    bool CheckAtaques()
    {
        if (attackFinished && (Time.time - comboFinishedTime) >= delayCombos)
        {
            controller.ResetBotonesAtaques();
            passiveCombo.Clear();

            attackFinished = false;
        }
        else if (attackFinished && (Time.time - comboFinishedTime) < delayCombos)
            return false;

        float delay = 0;
        if (currentComboAttack != -1 && currentComboAttacks != null && (currentComboAttack + 1) != currentComboAttacks.attacks.Length)
        {

            delay = currentComboAttacks.attacks[currentComboAttack].delay;

        }

        if ((Time.time - attackStartTime) >= delay)
        {
            
            if (controller.ataqueCuadrado)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }


                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + 0.1f)
                    {
                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if(GetAttacks(ComboAtaques.air1).attacks.Length-1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air1).attacks.Length - 1;
                    }
                    moveDirSaved = new Vector3();
                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air1);
                    PlayAttack();
                }
                else if (states == States.MOVE)
                {
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    if (moves == Moves.RUN)
                    {
                        if ((Time.time - attackStartTime) >= delay+0.1f)
                        {
                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.run1).attacks.Length-1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.run1).attacks.Length - 1;
                        }

                        attacks = Attacks.RUN;
                        currentComboAttacks = GetAttacks(ComboAtaques.run1);
                        PlayAttack();
                    }
                    else
                    {
                        if ((Time.time - attackStartTime) >= delay + 0.1f)
                        {
                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.Quadrat).attacks.Length-1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.Quadrat).attacks.Length - 1;
                        }
                        passiveCombo.Add(PassiveCombo.QUADRATFLOOR);
                        attacks = Attacks.GROUND;
                        currentComboAttacks = GetAttacks(ComboAtaques.Quadrat);
                        PlayAttack();
                    }

                }
                else if (states == States.IDLE)
                {
                    if ((Time.time - attackStartTime) >= delay + 0.1f)
                    {
                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.Quadrat).attacks.Length-1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.Quadrat).attacks.Length - 1;
                        
                        Debug.Log("Attack");
                    }
                    passiveCombo.Add(PassiveCombo.QUADRATFLOOR);
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    attacks = Attacks.GROUND;
                    currentComboAttacks = GetAttacks(ComboAtaques.Quadrat);
                    PlayAttack();
                }

                states = States.ATTACK;

                controller.ResetBotonesAtaques();
                return true;
            }

            if ((controller.ataqueTriangulo))
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + 0.1f)
                    {
                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.air2).attacks.Length-1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air2).attacks.Length - 2;
                    }
                    moveDirSaved = new Vector3();

                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air2);
                    PlayAttack();
                }
                else if (states == States.MOVE)
                {
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    if (moves == Moves.RUN)
                    {
                        if ((Time.time - attackStartTime) >= delay + 0.1f)
                        {
                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.run2).attacks.Length-1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.run2).attacks.Length - 1;

                        }
                        attacks = Attacks.RUN;

                        currentComboAttacks = GetAttacks(ComboAtaques.run2);
                        PlayAttack();
                    }
                    else
                    {
                        if ((Time.time - attackStartTime) >= delay + 0.1f)
                        {
                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.Triangle).attacks.Length-1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.Triangle).attacks.Length - 1;
                        }
                        passiveCombo.Add(PassiveCombo.TRIANGLEFLOOR);
                        attacks = Attacks.GROUND;
                        currentComboAttacks = GetAttacks(ComboAtaques.Triangle);
                        PlayAttack();
                    }

                }
                else if (states == States.IDLE)
                {
                    if ((Time.time - attackStartTime) >= delay + 0.1f)
                    {
                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.Triangle).attacks.Length-1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.Triangle).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.TRIANGLEFLOOR);
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    attacks = Attacks.GROUND;
                    currentComboAttacks = GetAttacks(ComboAtaques.Triangle);
                    PlayAttack();
                }

                states = States.ATTACK;

                controller.ResetBotonesAtaques();
                return true;
            }


            if (controller.ataqueCuadradoCargadoL2 && states != States.JUMP)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if ((Time.time - attackStartTime) >= delay + 0.1f)
                {
                    currentComboAttack = -1;
                    passiveCombo.Clear();
                }
                else if (GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length-1 <= currentComboAttack)
                {
                    currentComboAttack = GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 2;
                }
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                moveDirSaved = new Vector3();
                states = States.ATTACK;
                attacks = Attacks.GROUND;
                currentComboAttacks = GetAttacks(ComboAtaques.HoldQuadrat);
                PlayAttack();
                controller.ResetBotonesAtaques();
                return true;
            }

            if (controller.ataqueCuadradoCargado && states != States.JUMP)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if ((Time.time - attackStartTime) >= delay + 0.1f)
                {
                    currentComboAttack = -1;
                    passiveCombo.Clear();
                }
                else if (GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 1 <= currentComboAttack)
                {
                    currentComboAttack = GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 2;
                }
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                moveDirSaved = new Vector3();
                states = States.ATTACK;
                attacks = Attacks.GROUND;
                currentComboAttacks = GetAttacks(ComboAtaques.combo5);
                PlayAttack();
                controller.ResetBotonesAtaques();
                return true;
            }

            if (controller.ataqueTrianguloCargado && states != States.JUMP)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if ((Time.time - attackStartTime) >= delay + 0.1f)
                {
                    currentComboAttack = -1;
                    passiveCombo.Clear();
                }
                else if (GetAttacks(ComboAtaques.HoldTriangle).attacks.Length-1 <= currentComboAttack)
                {
                    currentComboAttack = GetAttacks(ComboAtaques.HoldTriangle).attacks.Length - 2;
                }

                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                moveDirSaved = new Vector3();

                states = States.ATTACK;
                attacks = Attacks.GROUND;
                currentComboAttacks = GetAttacks(ComboAtaques.HoldTriangle);
                PlayAttack();
                controller.ResetBotonesAtaques();
                return true;
            }
        }
        //else
        //{
        //    if (currentComboAttacks.attacks[currentComboAttack].nextAttack)
        //    {
        //        if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
        //            player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
        //        states = States.ATTACK;
        //        currentComboAttacks.attacks[currentComboAttack].nextAttack = false;
        //        PlayAttack();
        //        return true;
        //    }

        //    if (controller.ataqueTriangulo && (GetCurrentAttackCombo() == ComboAtaques.combo3 || GetCurrentAttackCombo() == ComboAtaques.combo4 || GetCurrentAttackCombo() == ComboAtaques.air2 || GetCurrentAttackCombo() == ComboAtaques.run2))
        //    {
        //        states = States.ATTACK;

        //        PlayAttack();
        //        return true;

        //    }
        //    if (controller.ataqueCuadrado && (GetCurrentAttackCombo() == ComboAtaques.combo1 || GetCurrentAttackCombo() == ComboAtaques.combo2 || GetCurrentAttackCombo() == ComboAtaques.air1 || GetCurrentAttackCombo() == ComboAtaques.run1))
        //    {
        //        states = States.ATTACK;

        //        PlayAttack();
        //        return true;
        //    }
        //}
        return false;

    }
    void CheckNextAttack()
    {
        switch (GetCurrentAttackCombo())
        {
            case ComboAtaques.Quadrat:
                if (controller.ataqueCuadrado)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.HoldQuadrat:
                if (controller.ataqueCuadrado)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.Triangle:
                if (controller.ataqueTriangulo)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.HoldTriangle:
                if (controller.ataqueTriangulo)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.air1:
                if (controller.ataqueCuadrado)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.air2:
                if (controller.ataqueTriangulo)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.run1:
                if (controller.ataqueCuadrado)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
            case ComboAtaques.run2:
                if (controller.ataqueTriangulo)
                    currentComboAttacks.attacks[currentComboAttack].nextAttack = true;
                break;
        }
    }
    void CheckIfStartMove()
    {
        if (controller.StartMove())
        {
            delayMove = Time.time;
            playerAnim.CrossFadeInFixedTime("StartMoving", 0.1f);
            states = States.DELAYMOVE;
        }
    }

    bool CheckIfDash()
    {
        
        if (controller.dash)
        {
            controller.ResetBotonesAtaques();

            if (dashCount < dashConsecutivos)
            {
                if ((Time.time - delayDash) > 0.1f)
                {
                    dashCount = 0;
                }
            }
            else
            {
                if ((Time.time - delayDash) < delayDashes)
                {

                    return false;
                }
                else
                {
                    dashCount = 0;

                }

            }
            if (dashConsecutivos <= dashCount)
            {
                if ((Time.time - delayDash) < delayDashes)
                {
                    dashCount = 0;

                    return false;
                }
                else
                {
                    dashCount = 0;
                }
            }

            playerAnim.speed = 2f;

            dashCount++;
            stopAttack = true;
            if (controller.LeftStickValue().magnitude < 0.2f)
            {
                player.transform.GetChild(3).transform.localPosition += new Vector3(0, 0, 1).normalized;
                dashDirection = new Vector2(0, -1);
                player.transform.LookAt(player.transform.GetChild(3).transform.position);
                //player.transform.LookAt(movementController.transform.position);
                player.transform.GetChild(3).transform.localPosition = new Vector3();
            }
            else
            {
                movementController.transform.localPosition += new Vector3(controller.LeftStickValue().x, 0, controller.LeftStickValue().y).normalized;
                player.transform.LookAt(movementController.transform.position);
                dashDirection = new Vector2(controller.LeftStickValue().x, controller.LeftStickValue().y);

                movementController.transform.localPosition = new Vector3();
            }
            player.transform.GetChild(1).Rotate(new Vector3(0, 1, 0), 90);


            // Smoothly rotate towards the target point.

            controller.ResetBotonesAtaques();

            delayDash = Time.time;
            playerAnim.CrossFadeInFixedTime("Dash", 0.1f);
            states = States.DASH;
            dash = Dash.START;
            return true;
        }
        return false;
    }
    void CheckMove()
    {
        if (controller.StartMove() && states != States.MOVE)
        {
            states = States.MOVE;

            if (controller.RightTriggerPressed())
            {
                moves = Moves.RUN;
                playerAnim.CrossFadeInFixedTime("Run", 0.2f);

            }
            else
            {
                moves = Moves.WALK;
                playerAnim.CrossFadeInFixedTime("Walk", 0.2f);

            }
        }
        else if (states == States.MOVE)
        {
            if (controller.RightTriggerPressed() && moves == Moves.WALK)
            {
                moves = Moves.RUN;
                playerAnim.CrossFadeInFixedTime("Run", 0.2f);

            }
            else if (!controller.RightTriggerPressed() && moves == Moves.RUN)
            {
                moves = Moves.WALK;
                playerAnim.CrossFadeInFixedTime("Walk", 0.2f);

            }
        }


    }
    void CheckIfReturnIdle()
    {
        if (!controller.StartMove() && states != States.IDLE)
        {
            states = States.IDLE;
            moves = Moves.IDLE;
            playerAnim.CrossFadeInFixedTime("Idle", 0.2f);
            doubleJump = false;

        }
    }

    void Move(float velocity)
    {
        movementController.transform.localPosition += new Vector3(controller.LeftStickValue().x, 0, controller.LeftStickValue().y).normalized;
        var targetRotation = Quaternion.LookRotation(movementController.transform.position - player.transform.position);

        // Smoothly rotate towards the target point.
        player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, 10 * Time.deltaTime);
        //player.transform.LookAt(movementController.transform.position);

        moveDir = (movementController.transform.position - this.transform.position).normalized;
        movementController.transform.localPosition = new Vector3();
        if (moveDir.magnitude != 0)
            moveDirSaved = moveDir;
        this.GetComponent<Rigidbody>().AddForce(moveDir * velocity * Time.deltaTime, ForceMode.Force);
    }
    void RotateCamera()
    {
        if (controller.RightStickValue().magnitude > 0.2f)
        {

            camera.transform.Rotate(new Vector3(0, controller.RightStickValue().x, 0) * Time.deltaTime * CameraRotatSpeed);
            //camera.transform.GetChild(2).Rotate(new Vector3(0, 0, controller.RightStickValue().y) * Time.deltaTime * CameraRotatSpeed);

        }
    }


}

