using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using UnityEngine.VFX;
using System.Linq;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using System;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class PlayerControl : MonoBehaviour
{
    public ParticleSystem flash;
    public TextMeshProUGUI currentHealthText;
    public TextMeshProUGUI maxHealthText;
    public UnityEngine.UI.Slider healthSlider;

    public Animator cameraAAnims;
    public List<ItemList> items = new List<ItemList>();

    public enum HealthState { FROZEN, BURNED, POSIONED, AMPED, WEAKENED, NORMAL };

    enum States { MOVE, DASH, JUMP, ATTACK, IDLE, DELAYMOVE, HIT, DEATH, TELEPORT };

    enum Attacks { GROUND, AIR, RUN, FALL, LAND };

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

    public enum ComboAtaques { Quadrat, HoldQuadrat, Triangle, HoldTriangle, combo5, air1, air2, run1, run2, QuadratL2, HoldQuadratL2, TriangleL2, HoldTriangleL2, Teleport, run3, run4, air3, air4 };

    public int currentScroll;
    [System.Serializable]
    public struct Ataques
    {
        public float ataque;
        public float delay;
        public bool nextAttack;
        public MMFeedbacks effects;

        public MMFeedbacks enemyFeedback;
        public MMFeedbacks enemyFeedbackPotenciado;

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
        public int damage;

        public GameObject slash;

        //1 is light, 2 is heavy
        //public GameObject BlessingToSpawnHeavy;
        //public GameObject BlessingToSpawnHeavy;
        //public GameObject BlessingToSpawnHeavy;
        //pickup
    }
    [System.Serializable]
    public class ListaAtaques
    {
        public ComboAtaques combo;
        public Ataques[] attacks;
    }
    public Blessing[] blessing;

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
    public GetEnemies attackTeleport;
    public GetEnemies attackTeleport2;
    public GetEnemies remate;

    public GameObject[] dashEffects;

    bool dashDown;

    float landHeight;

    bool stopAttack;

    public DoubleJumpVFXController doubleJumpVFX;
    public LandingVFXController landVFX;
    public SingleJumpVFXController singleJumpVFX;

    public int dashConsecutivos;
    public int dashCount;

    float TeleportTime;


    public enum PassiveCombo
    {
        QUADRATFLOOR,
        TRIANGLEFLOOR,
        HOLDQUADRATFLOOR,
        HOLDTRIANGLEFLOOR,
        QUADRATAIR,
        TRIANGLEAIR,
        QUADRATRUN,
        TRIANGLERUN,
        QUADRATFLOORL2,
        TRIANGLEFLOORL2,
        HOLDQUADRATFLOORL2,
        HOLDTRIANGLEFLOORL2,
        TELEPORT,
        QUADRATRUNL2,
        TRIANGLERUNL2,
        HOLDQUADRATAIR,
        HOLDTRIANGLEAIR,

    }
    public List<PassiveCombo> passiveCombo = new List<PassiveCombo>();

    public MMFeedbacks jumpFeedback;
    public MMFeedbacks doubleJumpFeedback;

    public MMFeedbacks landFeedback;
    public MMFeedbacks walkFeedback;
    public MMFeedbacks runFeedback;
    public MMFeedbacks runStartFeedback;

    public MMFeedbacks dashFeedback;
    public MMFeedbacks idleFeedback;

    public MMFeedbacks hitFeedback;
    public MMFeedbacks critFeedback;
    public MMFeedbacks playerHurt;

    float hitTime;
    float deathTime;
    int repeticionGolpe;

    public Animator desaparecer;

    [Header("Combat Stats")]
    [Space]
    [SerializeField]
    public float currentHealth;
    [SerializeField]
    public float maxHealth = 100;
    [SerializeField]
    public float critChance;
    public float maxCritChance = 100;
    [SerializeField]
    public float attackDamage;
    [SerializeField]
    public float healthRegen;
    public float critDamageMultiplier;
    public int slowForce;
    public bool isCrit;

    // Start is called before the first frame update
    void Start()
    {
        repeticionGolpe = 0;
        currentHealth = maxHealth;
        attackDamage = 1;
        //HealingArea item = new HealingArea();
        //items.Add(new ItemList(item, item.GiveName(), 1));
        StartCoroutine(CallItemUpdate());
        dashCount = 0;
        attackFinished = false;
        attackFinished = false;
        controller = GameObject.FindObjectOfType<ControllerManager>();
        currentComboAttack = -1;
        playerAnim = player.GetComponent<Animator>();

        states = States.IDLE;
        moves = Moves.IDLE;
    }

    public void SetHealth()
    {
        currentHealthText.text = currentHealth.ToString();
        maxHealthText.text = maxHealth.ToString();
        healthSlider.value = currentHealth / maxHealth;
    }
    public bool ReturnIfCrit()
    {
        return isCrit;
    }
    public float GetCritOrDamage(float damage)
    {
        float baseDamage = damage * attackDamage;

        bool isCrit = IsCriticalHit();

        float finalDamage = CalculateDamage(baseDamage, isCrit);
        // Debug.Log("Final Damage: " + finalDamage);

        return finalDamage;
    }

    private bool IsCriticalHit()
    {
        float randomValue = UnityEngine.Random.Range(0f, 100f);
        isCrit = randomValue <= critChance;
        if (isCrit)
        {
            critFeedback.PlayFeedbacks();
        }


        Debug.Log("Is Critical Hit: " + isCrit);

        return isCrit;
    }

    private float CalculateDamage(float baseDamage, bool isCrit)
    {
        float finalDamage = isCrit ? baseDamage * (2f * critDamageMultiplier) : baseDamage;

        //Debug.Log("Final Damage: " + finalDamage);

        return finalDamage;
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
                this.gameObject.layer = 10;
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

                        StartCoroutine(DelayGolpe(0, 0, 1, damageMult));

                        playerAnim.CrossFadeInFixedTime("LandAttack", 0.1f);
                        doubleJump = false;
                        this.gameObject.layer = 3;

                        comboFinishedTime = Time.time;
                        attackFinished = true;
                        delayCombos = currentComboAttacks.attacks[currentComboAttack].delayFinal;
                        dealyAttackFall = Time.time;
                        attacks = Attacks.LAND;
                        currentComboAttack = -1;
                        moveDirSaved = new Vector3();
                        landHeight = hit.point.y;

                        //this.transform.position = new Vector3(this.transform.position.x, landHeight + 0.2f, this.transform.position.z);

                        return false;
                    }

                }
            }
            if (CheckIfDash())
            {
                dashDown = true;
            }
            return false;
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
        this.GetComponent<Rigidbody>().AddForce(gravity * Time.deltaTime, ForceMode.Force);
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

    public void SpawnObjectWithDelay(float delay, GameObject blessing, float distance, float upDistance, Action<GameObject> callback)
    {
        StartCoroutine(DelayedSpawn(delay, blessing, distance, upDistance, callback));
    }

    private IEnumerator DelayedSpawn(float delay, GameObject blessing, float distance, float upDistance, Action<GameObject> callback)
    {
        yield return new WaitForSeconds(delay);

        GameObject spawnedObject = Instantiate(blessing, player.transform.position + (player.transform.forward * distance) + (player.transform.up * upDistance), Quaternion.identity);
        callback(spawnedObject);
    }

    private IEnumerator Slash(float time, GameObject slashSave, GameObject slash)
    {
        yield return new WaitForSeconds(time);

        slash.transform.parent = slashSave.transform;
        slash.transform.localPosition = Vector3.zero;
        slash.transform.localEulerAngles = Vector3.zero;
        slash.SetActive(false);
    }

    private IEnumerator DelayGolpe(float time, int golpe, float damageMult, float potenciado)
    {

        yield return new WaitForSeconds(time);

        if ((states == States.ATTACK && !stopAttack) || currentComboAttacks.combo == ComboAtaques.air2)
        {
            if (currentComboAttacks.attacks[golpe].effects != null)
            {
                currentComboAttacks.attacks[golpe].effects.PlayFeedbacks();
            }
            repeticionGolpe++;

            if (currentComboAttacks.combo == ComboAtaques.HoldTriangleL2 && repeticionGolpe == 1)
            {
                currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).localPosition += new Vector3(0, 0, 2);

            }

            currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).gameObject.SetActive(true);

            StartCoroutine(Slash(2, currentComboAttacks.attacks[golpe].slash, currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).gameObject));

            currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;



            bool hasApplied = false;
            if (blessing != null)
            {
                foreach (Blessing i in blessing)
                {
                    if (AreListsEqual(i.passiveCombo, passiveCombo))
                    {
                        hasApplied = true;
                        SpawnObjectWithDelay(i.visualEffect.GetComponent<AttackCollider>().spawnDelay, i.visualEffect, i.visualEffect.GetComponent<AttackCollider>().spawnDistance,
                            i.visualEffect.GetComponent<AttackCollider>().upDistance, (spawnedObject) =>
                            {
                                // Use the spawnedObject reference here
                                //GameObject effect = spawnedObject;
                                Collider effectCollider = spawnedObject.GetComponent<Collider>();
                                spawnedObject.GetComponent<AttackCollider>().attack = passiveCombo.Last();

                                spawnedObject.GetComponent<AttackCollider>().healthState = i.healthState;
                                spawnedObject.GetComponent<AttackCollider>().enemyHitAnim = i.hit;
                                spawnedObject.GetComponent<AttackCollider>().KnockbackY = i.knockback.y;
                                spawnedObject.GetComponent<AttackCollider>().enemyStandUp = i.EnemyStandUp;


                                spawnedObject.GetComponent<AttackCollider>().damage = GetCritOrDamage(i.damage * damageMult * potenciado);


                                spawnedObject.GetComponent<AttackCollider>().Knockback = i.knockback.x;


                                spawnedObject.GetComponent<AttackCollider>().SetFeedback(i.enemyFeedback);

                                spawnedObject.tag = currentComboAttacks.attacks[golpe].colliderTag;
                                effectCollider.enabled = true;
                                StartCoroutine(DesactivarCollisionGolpeBlessing(0.05f, effectCollider));
                            });


                    }
                }
            }
            if (!hasApplied)
            {
                if (currentComboAttacks.combo == ComboAtaques.HoldTriangleL2 && repeticionGolpe == 1)
                {
                    currentComboAttacks.attacks[golpe].collider.tag = currentComboAttacks.attacks[golpe].colliderTag;

                    currentComboAttacks.attacks[golpe].slash.transform.GetChild(0).localEulerAngles += new Vector3(0, 0, 90);
                    //currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyHitAnim = "GolpeSalto";
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().KnockbackY = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.y;
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyStandUp = false;

                }
                else if (currentComboAttacks.combo == ComboAtaques.HoldTriangleL2 && repeticionGolpe == 2)
                {
                    currentComboAttacks.attacks[golpe].collider.tag = "GolpeRebote";

                   // currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyHitAnim = "HitFuerte";
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().KnockbackY = -currentComboAttacks.attacks[golpe].EnemyKnockBackForce.y;
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyStandUp = true;

                }
                else
                {
                    currentComboAttacks.attacks[golpe].collider.tag = currentComboAttacks.attacks[golpe].colliderTag;

                    //currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyHitAnim = currentComboAttacks.attacks[golpe].enemyHitAnim;
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().KnockbackY = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.y;
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().enemyStandUp = currentComboAttacks.attacks[golpe].EnemyStandUp;

                }
                if(currentComboAttacks.combo == ComboAtaques.HoldQuadrat)
                {
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().toAir = true;
                }
                else
                {
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().toAir = false;

                }

                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().attack = passiveCombo.Last();
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().healthState = HealthState.NORMAL;
                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().Knockback = currentComboAttacks.attacks[golpe].EnemyKnockBackForce.x;




                currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().damage = GetCritOrDamage(currentComboAttacks.attacks[golpe].damage * damageMult * potenciado);


                if (potenciado == 1)
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().SetFeedback(currentComboAttacks.attacks[golpe].enemyFeedback);
                else
                    currentComboAttacks.attacks[golpe].collider.GetComponent<AttackCollider>().SetFeedback(currentComboAttacks.attacks[golpe].enemyFeedbackPotenciado);

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

    Vector3 enemy;

    void Aparecer()
    {
        playerAnim.Play("Aparecer", 1);

    }
    void PlayAttack()
    {

        float damageMultiplier = 1;

        if (attackTeleport.GetEnemie(this.transform.position) != Vector3.zero && Vector3.Distance(this.transform.position, attackTeleport.GetEnemiePos(this.transform.position)) < 6 && currentComboAttacks.combo != ComboAtaques.Teleport)
        {

            enemy = attackTeleport.GetEnemie(this.transform.position);
            Vector3 enem = enemy;
            enem.y = player.transform.position.y;
            enemy += (enem - player.transform.position).normalized * 1f;
            Vector3 dir = new Vector3(0, 0, 0);

            if (currentComboAttack == -1 || controller.LeftStickValue().magnitude == 0)
            {
                dir = -(enem - player.transform.position);
            }
            else
            {
                dir = (camera.transform.position - (camera.transform.position + (camera.transform.forward * -controller.LeftStickValue().y) + (camera.transform.right * -controller.LeftStickValue().x)));

            }
            dir.Normalize();

            if (currentComboAttacks.combo == ComboAtaques.HoldQuadratL2)
                dir = attackTeleport.GetEnemiePos(this.transform.position) + (dir * 7);
            else
                dir = attackTeleport.GetEnemiePos(this.transform.position) + (dir * 3);

            if (currentComboAttacks.combo == ComboAtaques.air2 || currentComboAttacks.combo == ComboAtaques.air1)
                dir.y = player.transform.position.y;

            RaycastHit hit;

            if (Physics.Raycast(dir + new Vector3(0, 1, 0), transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7))
            {
                if (currentComboAttacks.combo != ComboAtaques.air1 && currentComboAttacks.combo != ComboAtaques.air2 && currentComboAttacks.combo != ComboAtaques.air3 && currentComboAttacks.combo != ComboAtaques.air4)
                    dir.y = hit.point.y;


                playerAnim.Play("Desaparecer", 1);
                Invoke("Aparecer", 0.1f);

                enemy = attackTeleport.GetEnemie(this.transform.position);
                enem = enemy;
                enem.y = dir.y;
                enemy += (enem - dir).normalized * 3f;
                this.GetComponent<Rigidbody>().DOMove(dir, 0.1f, false);

            }


        }

        if (remate.GetEnemyDebil(this.transform.position) != null && (currentComboAttacks.combo == ComboAtaques.HoldTriangle || currentComboAttacks.combo == ComboAtaques.HoldQuadrat))
        {
            GameObject enemyObject = remate.GetEnemyDebil(this.transform.position);
            enemyObject.GetComponent<EnemySkeletonSword>().EndDebil();

            Vector3 pos = new Vector3(enemyObject.transform.position.x, player.transform.position.y, enemyObject.transform.position.z);
            damageMultiplier = 5;
            enemy = pos;
            Vector3 enem = enemy;
            enem.y = player.transform.position.y;
            enemy += (enem - player.transform.position).normalized * 0.5f;
            Vector3 dir = new Vector3(0, 0, 0);

            if (currentComboAttack == -1 || controller.LeftStickValue().magnitude == 0)
            {
                dir = -(enem - player.transform.position);
            }
            else
            {
                dir = (camera.transform.position - (camera.transform.position + (camera.transform.forward * -controller.LeftStickValue().y) + (camera.transform.right * -controller.LeftStickValue().x)));

            }
            dir.Normalize();
            if (currentComboAttacks.combo == ComboAtaques.HoldTriangle)
                dir = pos + (dir * 7);
            else
                dir = pos + (dir * 3);

            if (currentComboAttacks.combo == ComboAtaques.air2 || currentComboAttacks.combo == ComboAtaques.air1)
                dir.y = player.transform.position.y;

            RaycastHit hit;

            if (Physics.Raycast(dir + new Vector3(0, 1, 0), transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7))
            {
                if (currentComboAttacks.combo != ComboAtaques.air1 && currentComboAttacks.combo != ComboAtaques.air2 && currentComboAttacks.combo != ComboAtaques.air3 && currentComboAttacks.combo != ComboAtaques.air4)
                    dir.y = hit.point.y;


                playerAnim.Play("Desaparecer", 1);
                Invoke("Aparecer", 0.1f);

                enemy = attackTeleport.GetEnemie(this.transform.position);
                enem = enemy;
                enem.y = dir.y;
                enemy += (enem - dir).normalized * 3f;
                this.GetComponent<Rigidbody>().DOMove(dir, 0.1f, false);

            }

            switch (currentComboAttacks.combo)
            {
                case ComboAtaques.HoldQuadrat:
                    camera.GetComponent<Remate>().remate("HoldQuadrat");

                    break;
                case ComboAtaques.HoldTriangle:
                    camera.GetComponent<Remate>().remate("HoldTriangle");

                    break;

            }

            camera.transform.DORotate(player.transform.eulerAngles, 0.1f);
            camera.transform.GetChild(0).DOLocalRotate(new Vector3(360, 0, 0), 0.1f);
        }



        playerAnim.speed = 1.75f;
        currentComboAttack++;
        if (currentComboAttacks.attacks[currentComboAttack].collider != null && currentComboAttacks.combo != ComboAtaques.air2)
        {

            for (int i = 0; i <= currentComboAttacks.attacks[currentComboAttack].repeticionGolpes; i++)
            {
                stopAttack = false;
                StartCoroutine(DelayGolpe(currentComboAttacks.attacks[currentComboAttack].delayGolpe + (i * currentComboAttacks.attacks[currentComboAttack].delayRepeticionGolpes), currentComboAttack, damageMultiplier, damageMult));


            }
        }
        damageMult = 1;

        playerAnim.CrossFadeInFixedTime(currentComboAttacks.attacks[currentComboAttack].name, currentComboAttacks.attacks[currentComboAttack].transition);

        attackStartTime = Time.time;
    }
    void AttackMovement()
    {
        if (currentComboAttack == -1)
            currentComboAttack = 0;
        this.GetComponent<Rigidbody>().AddForce(this.transform.up * currentComboAttacks.attacks[currentComboAttack].curvaDeVelocidadMovimientoY.Evaluate(Time.time - attackStartTime) * currentComboAttacks.attacks[currentComboAttack].velocidadMovimientoY * Time.deltaTime, ForceMode.Force);

        player.transform.GetChild(3).transform.localPosition += new Vector3(0, 0, 1).normalized;

        RaycastHit hit;

        if (Physics.Raycast(player.transform.GetChild(3).transform.position + new Vector3(0, 1f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
        {
            if (states == States.MOVE)
            {
                Vector3 pos;



                if (Mathf.Abs(hit.point.y - this.transform.position.y) < 2)
                    player.transform.GetChild(3).transform.position = new Vector3(movementController.transform.position.x, hit.point.y, movementController.transform.position.z);

            }

        }
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
                this.gameObject.layer = 10;

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
                this.gameObject.layer = 3;

                if (states == States.JUMP)
                    landFeedback.PlayFeedbacks();

                return false;
            }

        }
        return false;



    }
    // SUPER IMPORTANT LES SEGUENTS 3 FUNCIONS
    public int GetItemStacks(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.stacks;
            }
        }
        return 0;
    }

    public string GetItemDescription(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.itemDescription;
            }
        }
        return null;
    }
    public Sprite GetItemImage(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.itemImage;
            }
        }
        return null;
    }

    public RarityType GetItemRarity(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return i.rarity;
            }
        }
        return 0;
    }
    //ASDAS

    ComboAtaques GetCurrentAttackCombo()
    {
        return currentComboAttacks.combo;
    }
    bool CheckIfJump()
    {

        if (controller.CheckIfJump())
        {
            this.gameObject.layer = 10;

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
                jumpFeedback.PlayFeedbacks();

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
                doubleJumpFeedback.PlayFeedbacks();

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
                this.gameObject.layer = 3;

                timeLanding = Time.time;
                if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                    playerAnim.CrossFadeInFixedTime("Land", 0.2f);
                states = States.JUMP;
                jump = Jump.LAND;
                doubleJump = false;
                moveDirSaved = new Vector3();
                //landFeedback.PlayFeedbacks();

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
        CheckPerfectHit();

        RotateCamera();

        switch (states)
        {
            case States.IDLE:
                var a = this.transform.position;

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


                if (enemy != Vector3.zero)
                {
                    Vector3 enem = enemy;
                    enem.y = player.transform.position.y;
                    player.transform.LookAt(enem);

                }

                if (CheckIfNextAttack())
                {
                    CheckIfIsFalling();

                    break;
                }
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
                    case Attacks.LAND:
                        if ((Time.time - dealyAttackFall) < 0.5f)
                            break;
                        else if ((Time.time - dealyAttackFall) >= 0.5f)
                        {

                            CheckIfReturnIdle();
                            CheckMove();

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
                            this.gameObject.layer = 3;

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


                switch (jump)
                {

                    case Jump.JUMP:
                        switch (moves)
                        {
                            case Moves.IDLE:
                                Move(walkSpeedAir / 2);
                                break;
                            case Moves.WALK:
                                Move(walkSpeedAir);

                                break;
                            case Moves.RUN:
                                Move(runSpeedAir);
                                break;
                        }
                        if (((Time.time - timeJumping) > 0.2f && !doubleJump) || ((Time.time - timeJumping) > 0.4f && doubleJump))
                        {
                            playerAnim.CrossFadeInFixedTime("Fall", 0.2f);

                            jump = Jump.FALL;
                        }

                        break;
                    case Jump.FALL:
                        if ((Time.time - fallStartTime) > 0.2f)
                        {
                            switch (moves)
                            {
                                case Moves.IDLE:
                                    Move(walkSpeedAir / 2);
                                    break;
                                case Moves.WALK:
                                    Move(walkSpeedAir);

                                    break;
                                case Moves.RUN:
                                    Move(runSpeedAir);
                                    break;
                            }
                        }

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
            case States.HIT:
                if ((Time.time - hitTime) > 0.25f)
                {
                    if (CheckIfDash())
                    {
                        dashDown = false;

                        break;
                    }
                }
                if ((Time.time - hitTime) > 0.5f)
                {
                    CheckIfReturnIdle();
                    CheckIfStartMove();
                }

                break;
            case States.DEATH:
                if ((Time.time - deathTime) > 2)
                {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                break;
            case States.TELEPORT:
                if ((Time.time - TeleportTime) > 0.25f)
                {
                    this.GetComponent<Rigidbody>().useGravity = true;

                    if (CheckIfIsFalling())
                        break;
                    CheckIfReturnIdle();
                    CheckMove();
                }
                break;
        }
    }

    IEnumerator CallItemUpdate()
    {
        foreach (ItemList i in items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    public void CallItemOnKill(EnemySkeletonSword enemy)
    {
        foreach (ItemList i in items)
        {
            i.item.OnKill(this, enemy, i.stacks);
        }
    }
    public void CallItemOnCrit(EnemySkeletonSword enemy)
    {
        foreach (ItemList i in items)
        {
            i.item.OnCrit(this, enemy, i.stacks);
        }
    }

    public void CallItemOnPickup(StatType desiredStatType)
    {
        foreach (ItemList i in items)
        {
            if (i.statType == desiredStatType)
            {
                i.item.OnItemPickup(this, i.stacks, i.statType);
            }
        }
    }

    public void CallItemOnHit(EnemySkeletonSword enemy)
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
    //public void CallItemOnPickup()
    //{
    //    foreach (ItemList i in items)
    //    {
    //        i.item.OnItemPickup(this, i.stacks, i.statType);
    //    }
    //}
    float damageMult = 1;
    public float delayDamage = 0.5f;
    bool atackPress = false;
    void CheckPerfectHit()
    {
        if (currentComboAttacks == null || currentComboAttack == -1 || currentComboAttack >= currentComboAttacks.attacks.Length)
        {


            return;

        }
        if ((Time.time - attackStartTime) >= 0f && atackPress && (Time.time - attackStartTime) <= 0.1f)
        {
            atackPress = false;
        }


        if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].delay && !flash.isPlaying && !atackPress && damageMult == 1 && (Time.time - attackStartTime) <= currentComboAttacks.attacks[currentComboAttack].delay + delayDamage)
        {
            flash.startLifetime = delayDamage;
            for (int i = 0; i < flash.gameObject.transform.childCount; i++)
            {
                flash.gameObject.transform.GetChild(i).GetComponent<ParticleSystem>().startLifetime = delayDamage;
            }


            flash.Play();
        }
        if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].delay + delayDamage && flash.isPlaying)
        {

        }

        if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].delay && damageMult == 1 && (Time.time - attackStartTime) <= currentComboAttacks.attacks[currentComboAttack].delay + delayDamage)
        {
            if ((controller.ataqueCuadradoPress) || controller.ataqueCuadradoCargadoPress || controller.ataqueCuadradoL2Press || controller.ataqueCuadradoCargadoL2Press || controller.ataqueTrianguloPress || controller.ataqueTrianguloCargadoPress || controller.ataqueTrianguloL2Press || controller.ataqueTrianguloCargadoL2Press)
            {
                if (!atackPress)
                {
                    damageMult = 1.5f;

                }

            }
        }

        if ((Time.time - attackStartTime) <= currentComboAttacks.attacks[currentComboAttack].delay && !atackPress)
        {
            if ((controller.ataqueCuadradoPress) || controller.ataqueCuadradoCargadoPress || controller.ataqueCuadradoL2Press || controller.ataqueCuadradoCargadoL2Press || controller.ataqueTrianguloPress || controller.ataqueTrianguloCargadoPress || controller.ataqueTrianguloL2Press || controller.ataqueTrianguloCargadoL2Press)
            {
                atackPress = true;

            }
        }
        if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].delay + delayDamage && atackPress)
        {
            atackPress = false;

        }


    }
    bool CheckAtaques()
    {
        //if(controller.Box.action != null)
        //{
        //    if (controller.Box.action.WasPressedThisFrame() && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Cargar") && (states == States.IDLE|| states == States.MOVE))
        //    {
        //        playerAnim.CrossFadeInFixedTime("Cargar", 0.1f);
        //    }
        //}
        //if (controller.Triangle.action != null)
        //{
        //    if (controller.Triangle.action.WasPressedThisFrame() && !playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Cargar") && (states == States.IDLE || states == States.MOVE))
        //    {
        //        playerAnim.CrossFadeInFixedTime("Cargar", 0.1f);
        //    }
        //}
        float delay = 0;
        if (currentComboAttack != -1 && currentComboAttacks != null && (currentComboAttack + 1) != currentComboAttacks.attacks.Length)
        {
            if (currentComboAttack < currentComboAttacks.attacks.Length)
                delay = currentComboAttacks.attacks[currentComboAttack].delay;

        }
        if (controller.Teleport1)
        {

            Vector3 pos2 = attackTeleport2.GetEnemiePos(this.transform.position);

            if (pos2 != Vector3.zero)
            {


                Vector3 pos = (pos2 - this.transform.position).normalized;

                pos = attackTeleport2.GetEnemiePos(this.transform.position) - (pos * 2);

                RaycastHit hit;

                if (Physics.Raycast(pos, transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7))
                {
                    this.GetComponent<Rigidbody>().DOMove(pos, 0.25f);
                    moveDir = Vector3.zero;
                    moveDirSaved = Vector3.zero;

                    playerAnim.Play("Desaparecer", 1);
                    Invoke("Aparecer", 0.1f);

                    enemy = attackTeleport2.GetEnemiePos(this.transform.position);

                    this.GetComponent<Rigidbody>().velocity = Vector3.zero;

                    TeleportTime = Time.time;

                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.Teleport).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.Teleport).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.TELEPORT);
                    attacks = Attacks.GROUND;
                    currentComboAttacks = GetAttacks(ComboAtaques.Teleport);
                    PlayAttack();

                    states = States.ATTACK;

                    controller.ResetBotonesAtaques();
                    return true;
                }


            }

        }


        if (attackFinished && (Time.time - comboFinishedTime) >= delayCombos)
        {
            if (currentComboAttacks.combo != ComboAtaques.HoldQuadrat && currentComboAttacks.combo != ComboAtaques.HoldQuadratL2 && currentComboAttacks.combo != ComboAtaques.HoldTriangle && currentComboAttacks.combo != ComboAtaques.HoldTriangleL2 && currentComboAttacks.combo != ComboAtaques.Teleport)
                controller.ResetBotonesAtaques();


            passiveCombo.Clear();

            attackFinished = false;
        }
        else if (attackFinished && (Time.time - comboFinishedTime) < delayCombos)
            return false;



        if ((Time.time - attackStartTime) >= delay)
        {
            enemy = Vector3.zero;


            if (controller.ataqueCuadradoCargado)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.air3).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air3).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.HOLDQUADRATAIR);

                    moveDirSaved = new Vector3();
                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air3);
                    PlayAttack();
                }
                else
                {

                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.HOLDQUADRATFLOOR);
                    attacks = Attacks.GROUND;
                    currentComboAttacks = GetAttacks(ComboAtaques.HoldQuadrat);
                    PlayAttack();
                }

                states = States.ATTACK;

                controller.ResetBotonesAtaques();
                return true;
            }




            if (controller.ataqueCuadrado)
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    moveDirSaved = new Vector3();
                    attacks = Attacks.AIR;
                }


                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.air1).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air1).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.QUADRATAIR);

                    moveDirSaved = new Vector3();
                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air1);
                    PlayAttack();
                }
                else if (states == States.MOVE || currentComboAttacks.combo == ComboAtaques.run1)
                {
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    if (moves == Moves.RUN || currentComboAttacks.combo == ComboAtaques.run1)
                    {
                        if ((Time.time - attackStartTime) >= delay + delayDamage)
                        {
                            damageMult = 1;

                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.run1).attacks.Length - 1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.run1).attacks.Length - 1;
                        }
                        if (currentComboAttack == -1)
                            passiveCombo.Add(PassiveCombo.QUADRATRUN);
                        else
                            passiveCombo.Add(PassiveCombo.QUADRATFLOOR);
                        attacks = Attacks.RUN;
                        currentComboAttacks = GetAttacks(ComboAtaques.run1);
                        PlayAttack();
                    }
                    else
                    {
                        if ((Time.time - attackStartTime) >= delay + delayDamage)
                        {
                            damageMult = 1;

                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.Quadrat).attacks.Length - 1 <= currentComboAttack)
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

                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.Quadrat).attacks.Length - 1 <= currentComboAttack)
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
            if ((controller.ataqueTrianguloCargado))
            {
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.air4).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air4).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.HOLDTRIANGLEAIR);

                    moveDirSaved = new Vector3();
                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air4);
                    PlayAttack();
                }
                else
                {
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.HoldTriangle).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.HoldTriangle).attacks.Length - 1;
                    }
                    passiveCombo.Add(PassiveCombo.HOLDTRIANGLEFLOOR);
                    attacks = Attacks.GROUND;
                    currentComboAttacks = GetAttacks(ComboAtaques.HoldTriangle);
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
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.air2).attacks.Length - 1 <= currentComboAttack)
                    {
                        currentComboAttack = GetAttacks(ComboAtaques.air2).attacks.Length - 2;
                    }
                    passiveCombo.Add(PassiveCombo.TRIANGLEAIR);

                    moveDirSaved = new Vector3();

                    attacks = Attacks.AIR;
                    currentComboAttacks = GetAttacks(ComboAtaques.air2);
                    PlayAttack();
                }
                else if (states == States.MOVE || currentComboAttacks.combo == ComboAtaques.run2)
                {
                    if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                        player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
                    if (moves == Moves.RUN || currentComboAttacks.combo == ComboAtaques.run2)
                    {
                        if ((Time.time - attackStartTime) >= delay + delayDamage)
                        {
                            damageMult = 1;

                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.run2).attacks.Length - 1 <= currentComboAttack)
                        {
                            currentComboAttack = GetAttacks(ComboAtaques.run2).attacks.Length - 1;

                        }
                        if (currentComboAttack == -1)
                            passiveCombo.Add(PassiveCombo.TRIANGLERUN);
                        else
                            passiveCombo.Add(PassiveCombo.TRIANGLEFLOOR);

                        attacks = Attacks.RUN;

                        currentComboAttacks = GetAttacks(ComboAtaques.run2);
                        PlayAttack();
                    }
                    else
                    {
                        if ((Time.time - attackStartTime) >= delay + delayDamage)
                        {
                            damageMult = 1;

                            currentComboAttack = -1;
                            passiveCombo.Clear();
                        }
                        else if (GetAttacks(ComboAtaques.Triangle).attacks.Length - 1 <= currentComboAttack)
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
                    if ((Time.time - attackStartTime) >= delay + delayDamage)
                    {
                        damageMult = 1;

                        currentComboAttack = -1;
                        passiveCombo.Clear();
                    }
                    else if (GetAttacks(ComboAtaques.Triangle).attacks.Length - 1 <= currentComboAttack)
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

            //    if (controller.ataqueCuadradoCargadoL2 && states != States.JUMP)
            //    {
            //        this.gameObject.layer = 10;
            //        if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //        {
            //            //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //            //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //            //this.transform.position = pos;
            //            player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //        }

            //        if ((Time.time - attackStartTime) >= delay + delayDaño)
            //        {
            //            damageMult = 1;

            //            currentComboAttack = -1;
            //            passiveCombo.Clear();
            //        }
            //        else if (GetAttacks(ComboAtaques.HoldQuadratL2).attacks.Length - 1 <= currentComboAttack)
            //        {
            //            currentComboAttack = GetAttacks(ComboAtaques.HoldQuadratL2).attacks.Length - 2;
            //        }
            //        if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //        moveDirSaved = new Vector3();
            //        states = States.ATTACK;
            //        passiveCombo.Add(PassiveCombo.HOLDQUADRATFLOORL2);
            //        attacks = Attacks.GROUND;
            //        currentComboAttacks = GetAttacks(ComboAtaques.HoldQuadratL2);
            //        PlayAttack();
            //        controller.ResetBotonesAtaques();
            //        return true;

            //    }

            //    if (controller.ataqueCuadradoCargado)
            //    {
            //        if (states != States.JUMP)
            //        {
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            {
            //                //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //                //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //                //this.transform.position = pos;
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //            }

            //            if ((Time.time - attackStartTime) >= delay + delayDaño)
            //            {
            //                damageMult = 1;

            //                currentComboAttack = -1;
            //                passiveCombo.Clear();
            //            }
            //            else if (GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 1 <= currentComboAttack)
            //            {
            //                currentComboAttack = GetAttacks(ComboAtaques.HoldQuadrat).attacks.Length - 2;
            //            }
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //            moveDirSaved = new Vector3();
            //            states = States.ATTACK;
            //            passiveCombo.Add(PassiveCombo.HOLDQUADRATFLOOR);
            //            attacks = Attacks.GROUND;
            //            currentComboAttacks = GetAttacks(ComboAtaques.HoldQuadrat);
            //            PlayAttack();
            //            controller.ResetBotonesAtaques();
            //            return true;
            //        }
            //        else
            //        {
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            {
            //                //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //                //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //                //this.transform.position = pos;
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //            }

            //            if ((Time.time - attackStartTime) >= delay + delayDaño)
            //            {
            //                damageMult = 1;

            //                currentComboAttack = -1;
            //                passiveCombo.Clear();
            //            }
            //            else if (GetAttacks(ComboAtaques.air3).attacks.Length - 1 <= currentComboAttack)
            //            {
            //                currentComboAttack = GetAttacks(ComboAtaques.air3).attacks.Length - 2;
            //            }
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //            moveDirSaved = new Vector3();
            //            states = States.ATTACK;
            //            passiveCombo.Add(PassiveCombo.HOLDQUADRATAIR);
            //            attacks = Attacks.AIR;
            //            currentComboAttacks = GetAttacks(ComboAtaques.air3);
            //            PlayAttack();
            //            controller.ResetBotonesAtaques();
            //            return true;
            //        }
            //    }
            //    if (controller.ataqueTrianguloCargadoL2 && states != States.JUMP)
            //    {
            //        repeticionGolpe = 0;
            //        if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //        {
            //            //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //            //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //            //this.transform.position = pos;
            //            player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //        }

            //        if ((Time.time - attackStartTime) >= delay + delayDaño)
            //        {
            //            damageMult = 1;

            //            currentComboAttack = -1;
            //            passiveCombo.Clear();
            //        }
            //        else if (GetAttacks(ComboAtaques.HoldTriangleL2).attacks.Length - 1 <= currentComboAttack)
            //        {
            //            currentComboAttack = GetAttacks(ComboAtaques.HoldTriangleL2).attacks.Length - 2;
            //        }

            //        if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //        moveDirSaved = new Vector3();
            //        passiveCombo.Add(PassiveCombo.HOLDTRIANGLEFLOORL2);
            //        states = States.ATTACK;
            //        attacks = Attacks.GROUND;
            //        currentComboAttacks = GetAttacks(ComboAtaques.HoldTriangleL2);
            //        PlayAttack();
            //        controller.ResetBotonesAtaques();
            //        return true;
            //    }
            //    if (controller.ataqueTrianguloCargado)
            //    {
            //        if (states != States.JUMP)
            //        {
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            {
            //                //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //                //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //                //this.transform.position = pos;
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //            }

            //            if ((Time.time - attackStartTime) >= delay + delayDaño)
            //            {
            //                damageMult = 1;

            //                currentComboAttack = -1;
            //                passiveCombo.Clear();
            //            }
            //            else if (GetAttacks(ComboAtaques.HoldTriangle).attacks.Length - 1 <= currentComboAttack)
            //            {
            //                currentComboAttack = GetAttacks(ComboAtaques.HoldTriangle).attacks.Length - 2;
            //            }

            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //            moveDirSaved = new Vector3();
            //            passiveCombo.Add(PassiveCombo.HOLDTRIANGLEFLOOR);
            //            states = States.ATTACK;
            //            attacks = Attacks.GROUND;
            //            currentComboAttacks = GetAttacks(ComboAtaques.HoldTriangle);
            //            PlayAttack();
            //            controller.ResetBotonesAtaques();
            //            return true;


            //            enemy = Vector3.zero;
            //        }
            //        else
            //        {
            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //            {
            //                //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
            //                //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
            //                //this.transform.position = pos;
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

            //            }

            //            if ((Time.time - attackStartTime) >= delay + delayDaño)
            //            {
            //                damageMult = 1;

            //                currentComboAttack = -1;
            //                passiveCombo.Clear();
            //            }
            //            else if (GetAttacks(ComboAtaques.air4).attacks.Length - 1 <= currentComboAttack)
            //            {
            //                currentComboAttack = GetAttacks(ComboAtaques.air4).attacks.Length - 2;
            //            }

            //            if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
            //                player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));
            //            moveDirSaved = new Vector3();
            //            passiveCombo.Add(PassiveCombo.HOLDTRIANGLEAIR);
            //            states = States.ATTACK;
            //            attacks = Attacks.AIR;
            //            currentComboAttacks = GetAttacks(ComboAtaques.air4);
            //            PlayAttack();
            //            controller.ResetBotonesAtaques();
            //            return true;
            //        }
            //    }
        }

        if ((Time.time - attackStartTime) >= delay + delayDamage)
        {
            currentComboAttacks = GetAttacks(ComboAtaques.air1);

        }

        return false;

    }
    void CheckNextAttack()
    {

        if (currentComboAttacks.attacks.Length <= currentComboAttack || currentComboAttack < 0)
            return;
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

        if (controller.GetDash())
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
            this.gameObject.layer = 8;

            dashFeedback.PlayFeedbacks();

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
    private IEnumerator WalkFeedbak(float time)
    {

        yield return new WaitForSeconds(time);
        walkFeedback.StopFeedbacks();
        if (states == States.MOVE && moves == Moves.WALK)
        {
            walkFeedback.PlayFeedbacks();
            StartCoroutine(WalkFeedbak(0.42f));
        }
    }
    private IEnumerator RunFeedback(float time)
    {

        yield return new WaitForSeconds(time);
        runFeedback.StopFeedbacks();
        if (states == States.MOVE && moves == Moves.RUN)
        {
            runFeedback.PlayFeedbacks();
            StartCoroutine(RunFeedback(0.3f));
        }

    }
    void StartRun()
    {
        if (states == States.MOVE && moves == Moves.RUN)
            runStartFeedback.PlayFeedbacks();

    }
    void EndRun()
    {
        if ((states == States.MOVE && moves == Moves.WALK) || states == States.IDLE)
            idleFeedback.PlayFeedbacks();

    }
    void CheckMove()
    {
        if (controller.StartMove() && states != States.MOVE)
        {
            states = States.MOVE;

            //if (controller.RightTriggerPressed())
            //{
            //    Invoke("StartRun", 0.25f);

            //    moves = Moves.RUN;
            //    playerAnim.CrossFadeInFixedTime("Run", 0.2f);
            //    StartCoroutine(RunFeedback(0.3f));
            //    //StartCoroutine(RunFeedback(0));

            //}
            //else
            //{
            //    Invoke("EndRun", 0.25f);


            moves = Moves.WALK;
            playerAnim.CrossFadeInFixedTime("Walk", 0.2f);
            StartCoroutine(WalkFeedbak(0.42f));
            //StartCoroutine(WalkFeedbak(0));


            //}
        }
        else if (states == States.MOVE)
        {
            if (controller.RightTriggerPressed() && moves == Moves.WALK)
            {
                Invoke("StartRun", 0.25f);
                moves = Moves.RUN;
                playerAnim.CrossFadeInFixedTime("Run", 0.2f);
                StartCoroutine(RunFeedback(0.3f));
                //StartCoroutine(RunFeedback(0));

            }
            //else if (!controller.RightTriggerPressed() && moves == Moves.RUN)
            //{
            //    Invoke("EndRun", 0.25f);

            //    moves = Moves.WALK;
            //    playerAnim.CrossFadeInFixedTime("Walk", 0.2f);
            //    StartCoroutine(WalkFeedbak(0.42f));
            //    //StartCoroutine(WalkFeedbak(0));

            //}
        }


    }
    void CheckIfReturnIdle()
    {
        if (!controller.StartMove() && states != States.IDLE)
        {
            Invoke("EndRun", 0.25f);
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
        RaycastHit hit;

        if (Physics.Raycast(movementController.transform.position + new Vector3(0, 1f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
        {
            if (states == States.MOVE || states == States.JUMP)
            {
                Vector3 pos;

                moveDir = (movementController.transform.position - this.transform.position).normalized;
                if (moveDir.magnitude != 0)
                    moveDirSaved = moveDir;

                if (Mathf.Abs(hit.point.y - this.transform.position.y) < 2)
                    movementController.transform.position = new Vector3(movementController.transform.position.x, hit.point.y, movementController.transform.position.z);
                else
                {
                    int sdf = 0;

                }

                pos = (movementController.transform.position - this.transform.position).normalized;
                movementController.transform.localPosition = new Vector3();

                this.GetComponent<Rigidbody>().AddForce(pos * velocity * Time.deltaTime, ForceMode.Force);
            }

        }


    }
    void RotateCamera()
    {
        if (controller.RightStickValue().magnitude > 0.2f && states != States.DEATH)
        {

            camera.transform.Rotate(new Vector3(0, controller.RightStickValue().x, 0) * Time.deltaTime * CameraRotatSpeed);
            camera.transform.GetChild(0).Rotate(new Vector3(-controller.RightStickValue().y, 0, 0) * Time.deltaTime * CameraRotatSpeed * 0.5f);

            float rotation = camera.transform.GetChild(0).localEulerAngles.x;
            if (rotation < 300)
            {
                rotation += 360;

            }


            if (rotation < 330)
            {
                camera.transform.GetChild(0).localEulerAngles = new Vector3(330, 0, 0);

            }
            if (rotation > 390)
            {
                camera.transform.GetChild(0).localEulerAngles = new Vector3(390, 0, 0);
            }
            //camera.transform.GetChild(2).Rotate(new Vector3(0, 0, controller.RightStickValue().y) * Time.deltaTime * CameraRotatSpeed);

        }
    }
    public void GetDamage(float damage)
    {
        if ((states != States.HIT) && states != States.DEATH)
        {
            currentHealth -= damage;
            SetHealth();
            if (currentHealth <= 0)
            {
                states = States.DEATH;
                deathTime = Time.time;
                playerAnim.CrossFadeInFixedTime("Death", 0.2f);

                cameraAAnims.CrossFadeInFixedTime("Death", 0.2f);
            }
            else
            {
                states = States.HIT;
                hitTime = Time.time;
                playerAnim.CrossFadeInFixedTime("Hit1", 0.2f);
                hitFeedback.PlayFeedbacks();
            }

        }
    }
    private void OnTriggerEnter(Collider other)
    {

    }
}

