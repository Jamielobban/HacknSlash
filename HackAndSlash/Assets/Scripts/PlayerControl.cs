using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Feedbacks;
using System.Linq;
using System;
using UnityEngine.SceneManagement;
using DG.Tweening;
using DamageNumbersPro;

public class PlayerControl : MonoBehaviour
{
    [SerializeField]
    LayerMask layerSuelo;

    public PlayerHealthSystem healthSystem;
    public DamageNumber healPixel;
    public ParticleSystem flash;
    public UnityEngine.UI.Slider healthSlider;

    public Animator cameraAAnims;
    public List<ItemList> items = new List<ItemList>();
    public enum HealthState { FROZEN, BURNED, POSIONED, AMPED, WEAKENED, NORMAL };

    public enum States { MOVE, DASH, JUMP, ATTACK, IDLE, DELAYMOVE, HIT, DEATH, TELEPORT };

    enum Attacks { GROUND, AIR, RUN, FALL, LAND };

    enum Moves { WALK, RUN, IDLE };

    enum Dash { START, DASH, DOUBLEDASH, AIRDASH, END };

    enum Jump { JUMP, FALL, LAND };

    public States states;
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

    public bool HasDoubleJump;

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
        public MMFeedbacks[] effects;

        public string name;

        public float transition;

        public AnimationCurve curvaDeVelocidadMovimiento;
        public float velocidadMovimiento;

        public AnimationCurve curvaDeVelocidadMovimientoY;
        public float velocidadMovimientoY;

        public float delayFinal;

        public GameObject[] collider;


        public float[] delayRepeticionGolpes;

        public HitState []hitState;
        public GameObject[] slash;



    }

    [System.Serializable]
    public class ListaAtaques
    {
        public ComboAtaques combo;
        public Ataques[] attacks;
    }

    public PlayerHUDSystem hud;

    public Blessing[] blessing;

    public ListaAtaques[] ataques;
    ListaAtaques currentComboAttacks;
    public ListaAtaques[] airCombo;
    public ListaAtaques[] runCombo;

    public int currentComboAttack;

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
    public ControllerManager controller;
    float delayDash;
    public float dashSpeed;
    Vector2 dashDirection;
    public float delayDashes;
    public GetEnemies enemieTarget;
    public GetEnemies attackTeleport;
    public GetEnemies attackTeleport2;
    public GetEnemies remate;


    bool dashDown;

    float landHeight;

    bool stopAttack;

    public DoubleJumpVFXController doubleJumpVFX;
    public LandingVFXController landVFX;
    public SingleJumpVFXController singleJumpVFX;

    public int dashConsecutivos;
    public int dashCount;

    float TeleportTime;

    bool OnAir;
    public bool GetOnAir => OnAir;
    public enum HitState {DEBIL = 5,MEDIO = 10,FUERTE = 15};

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

    public PlayerData stats;

    public float critChance;
    public float attackDamage;
    public float healthRegen;
    public float critDamageMultiplier;
    

    // Start is called before the first frame update
    void Start()
    {
        critChance = stats.critChance;
        attackDamage = stats.attackDamage;
        healthRegen = stats.healthRegen;
        critDamageMultiplier = stats.critDamageMultiplier;
        OnAir = false;
        hud = GetComponent<PlayerHUDSystem>();
        repeticionGolpe = 0;
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

    //public void SetHealth()
    //{
    //    if(currentHealth > maxHealth)
    //    {
    //        currentHealth = maxHealth;
    //    }
    //    currentHealthText.text = currentHealth.ToString();
    //    maxHealthText.text = maxHealth.ToString();
    //    healthSlider.value = currentHealth / maxHealth;
    //}




    public ListaAtaques GetAttacks(ComboAtaques combo)
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

        //if (currentComboAttacks.combo == ComboAtaques.air2)
        //{

        //    if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].ataque && attacks == Attacks.AIR)
        //    {
        //        attacks = Attacks.FALL;
        //        playerAnim.CrossFadeInFixedTime("FallAttack", 0.1f);

        //    }
        //    else if ((Time.time - attackStartTime) >= currentComboAttacks.attacks[currentComboAttack].ataque && attacks == Attacks.FALL)
        //    {
        //        RaycastHit hit;

        //        if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
        //        {
        //            if (hit.distance < 0.5f)
        //            {
        //                playerAnim.speed = 1;

        //                StartCoroutine(DelayGolpe(0, 0, 1, damageMult));

        //                playerAnim.CrossFadeInFixedTime("LandAttack", 0.1f);
        //                doubleJump = false;

        //                comboFinishedTime = Time.time;
        //                attackFinished = true;
        //                delayCombos = currentComboAttacks.attacks[currentComboAttack].delayFinal;
        //                dealyAttackFall = Time.time;
        //                attacks = Attacks.LAND;
        //                currentComboAttack = -1;
        //                moveDirSaved = new Vector3();
        //                landHeight = hit.point.y;


        //                return false;
        //            }

        //        }
        //    }
        //    if (CheckIfDash())
        //    {
        //        dashDown = true;
        //    }
        //    return false;
        //}

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


        this.GetComponent<Rigidbody>().AddForce(gravity * Time.deltaTime, ForceMode.Force);
        

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

    private IEnumerator DelayGolpe(float time, int golpe, float damageMult, float potenciado, int slash)
    {

        yield return new WaitForSeconds(time);

        if ((states == States.ATTACK && !stopAttack))
        {
            if (currentComboAttacks.attacks[golpe].effects != null)
            {
                currentComboAttacks.attacks[golpe].effects[slash].PlayFeedbacks();
            }
            repeticionGolpe++;

            if (currentComboAttacks.combo == ComboAtaques.HoldTriangleL2 && repeticionGolpe == 1)
            {
                currentComboAttacks.attacks[golpe].slash[slash].transform.GetChild(0).localEulerAngles = new Vector3(0, 180, 0);
                currentComboAttacks.attacks[golpe].slash[slash].transform.GetChild(0).localPosition += new Vector3(0, 0, 2);

            }

            currentComboAttacks.attacks[golpe].slash[slash].transform.GetChild(0).gameObject.SetActive(true);

            StartCoroutine(Slash(2, currentComboAttacks.attacks[golpe].slash[slash], currentComboAttacks.attacks[golpe].slash[slash].transform.GetChild(0).gameObject));

            currentComboAttacks.attacks[golpe].slash[slash].transform.GetChild(0).parent = GameObject.FindGameObjectWithTag("Slashes").transform;

            currentComboAttacks.attacks[golpe].collider[slash].GetComponent<AttackCollider>().state = currentComboAttacks.attacks[golpe].hitState[slash];
                currentComboAttacks.attacks[golpe].collider[slash].SetActive(true);
                StartCoroutine(DesactivarCollisionGolpe(0.05f, currentComboAttacks.attacks[golpe].collider[slash]));
            
        }


    }

    private IEnumerator DesactivarCollisionGolpe(float time, GameObject col)
    {

        yield return new WaitForSeconds(time);
       col.SetActive(false);

    }

    private IEnumerator DesactivarCollisionGolpeBlessing(float time, Collider golpe)
    {

        yield return new WaitForSeconds(time);
        golpe.enabled = false;

    }

    Vector3 enemy;
    public Collider[] colliders;
    void Aparecer()
    {
        playerAnim.Play("Aparecer", 1);

    }
    void PlayAttack()
    {

        float damageMultiplier = 1;

        if (attackTeleport.GetEnemie(this.transform.position) != Vector3.zero && Vector3.Distance(this.transform.position, attackTeleport.GetEnemiePos(this.transform.position)) < 6 && currentComboAttacks.combo != ComboAtaques.Teleport)
        {
            Vector3 d = this.transform.position + ((camera.transform.forward * controller.LeftStickValue().y * 6) + (camera.transform.right * controller.LeftStickValue().x * 6));

            enemy = attackTeleport.GetEnemie(d);
            Vector3 enem = enemy;
            enem.y = player.transform.position.y;
            enemy += (enem - player.transform.position).normalized * 1f;
            Vector3 dir = new Vector3(0, 0, 0);

            //if (currentComboAttack == -1 || controller.LeftStickValue().magnitude == 0)
            //{
                dir = -(enem - player.transform.position);
            //}
            //else
            //{
            //    dir = (camera.transform.position - (camera.transform.position + (camera.transform.forward * -controller.LeftStickValue().y) + (camera.transform.right * -controller.LeftStickValue().x)));

            //}
            dir.Normalize();

            if (currentComboAttacks.combo == ComboAtaques.HoldQuadratL2)
                dir = attackTeleport.GetEnemiePos(d) + (dir * 7);
            else
                dir = attackTeleport.GetEnemiePos(d) + (dir * 3);

            if (currentComboAttacks.combo == ComboAtaques.air2 || currentComboAttacks.combo == ComboAtaques.air1)
                dir.y = player.transform.position.y;

            RaycastHit hit;
            RaycastHit hit2;

            if (Physics.Raycast(dir + new Vector3(0, 1, 0), transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7))
            {
                colliders = Physics.OverlapSphere(dir + new Vector3(0, 1, 0), 0.01f, 1 << 7);

                // Comprueba si hay colliders intersectando la esfera


                if (!(colliders.Length > 0))
                {
                   if (currentComboAttacks.combo != ComboAtaques.air1 && currentComboAttacks.combo != ComboAtaques.air2 && currentComboAttacks.combo != ComboAtaques.air3 && currentComboAttacks.combo != ComboAtaques.air4)
                        dir.y = hit.point.y;


                    playerAnim.Play("Desaparecer", 1);
                    Invoke("Aparecer", 0.1f);

                    enemy = attackTeleport.GetEnemie(d);
                    enem = enemy;
                    enem.y = dir.y;
                    enemy += (enem - dir).normalized * 3f;


                    this.GetComponent<Rigidbody>().DOMove(dir, 0.3f, false);
                }

            }


        }

        playerAnim.speed = 1.75f;
        currentComboAttack++;
        if (currentComboAttacks.attacks[currentComboAttack].collider != null )
        {

            for (int i = 0; i < currentComboAttacks.attacks[currentComboAttack].delayRepeticionGolpes.Length; i++)
            {
                stopAttack = false;
                if(currentComboAttacks.combo != ComboAtaques.air2)
                    StartCoroutine(DelayGolpe((currentComboAttacks.attacks[currentComboAttack].delayRepeticionGolpes[i]), currentComboAttack, damageMultiplier, damageMult, i));
                else
                {
                    RaycastHit hit;
                    Physics.Raycast(this.transform.position, transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7);
                    float a = (this.transform.position.y - hit.point.y) / 10;
                    float time = (currentComboAttacks.attacks[currentComboAttack].delayRepeticionGolpes[i]) + (a * 0.1f);
                    slashSuelo = currentComboAttacks.attacks[currentComboAttack].slash[i].transform.GetChild(0).gameObject;
                    Invoke("GuarradaSlashTrianguloAire", time - 0.15f);
                    StartCoroutine(DelayGolpe(time, currentComboAttack, damageMultiplier, damageMult, i));

                }


            }
        }
        damageMult = 1;

        playerAnim.CrossFadeInFixedTime(currentComboAttacks.attacks[currentComboAttack].name, currentComboAttacks.attacks[currentComboAttack].transition);

        attackStartTime = Time.time;
    }
    GameObject slashSuelo;
    void GuarradaSlashTrianguloAire()
    {
        slashSuelo.SetActive(true);

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



    #region Funcion que devuelve true si estas cayendo (por un bordillo, al acabar un ataque en el aire, o cualquier cosa) y se encarga de hacer todas las cosas al empezar a caer, cambia al estado JUMP y dentro de JUMP al estado FALL

    bool CheckIfIsFalling()
    {
        RaycastHit hit;
        RaycastHit hit2;

        if (Physics.Raycast(transform.position + new Vector3(0.2f, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, layerSuelo))
        {
            if (Physics.Raycast(transform.position + new Vector3(-0.2f, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit2, 200, layerSuelo))
            {
                if (hit.distance > distanceFloor && hit2.distance > distanceFloor)
                {

                    fallStartTime = Time.time;
                    OnAir = true;

                    states = States.JUMP;
                    jump = Jump.FALL;
                    if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                        playerAnim.CrossFadeInFixedTime("Fall", 0.2f);

                    return true;

                }
                else if (hit.distance < 0 && hit2.distance < 0)
                {
                    //this.transform.position = new Vector3(this.transform.position.x, hit.point.y+0.1f, this.transform.position.z);
                    doubleJump = false;

                    return CheckIfLand();
                }
                else
                {
                    //this.transform.position = new Vector3(this.transform.position.x, hit.point.y + 0.1f, this.transform.position.z);
                    doubleJump = false;

                    if (states == States.JUMP && currentComboAttacks.combo != ComboAtaques.air2)
                        landFeedback.PlayFeedbacks();

                    return false;
                }
            }
        }
        return false;



    }
    #endregion

    #region Cosas de items del Jamie

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

    public bool CheckIfHasItem(string itemName)
    {
        foreach (ItemList i in items)
        {
            if (i.name == itemName)
            {
                return true;
            }
        }
        return false;
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

    public Enums.RarityType GetItemRarity(string itemName)
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
    #endregion

    #region Funcion que devuelve el ataque actual que estas haciendo (enum ComboAtaques)

    ComboAtaques GetCurrentAttackCombo()
    {
        return currentComboAttacks.combo;
    }
    #endregion

    #region Funcion que devuelve true si pulsas saltar, se encarga de hacer todas las cosas al empezar el salto, cambia al estado JUMP y dentro de JUMP cambia al estado JUMP tambien
    bool CheckIfJump()
    {


        if (controller.CheckIfJump())
        {
            playerAnim.speed = 1f;

            controller.ResetBotonesAtaques();

            this.GetComponent<Rigidbody>().drag = 5;

            if (!OnAir)
            {
                //singleJumpVFX.PlaySingleJumpVFX(this.transform.position);
                timeJumping = Time.time;
                fallStartTime = Time.time;
                states = States.JUMP;
                OnAir = true;

                jump = Jump.JUMP;
                playerAnim.CrossFadeInFixedTime("Jump", 0.1f);
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().AddForce(this.transform.up * jumpForce, ForceMode.Impulse);
                jumpFeedback.PlayFeedbacks();

                return true;

            }
            else if (!doubleJump && HasDoubleJump)
            {
                Move(1);
                doubleJumpVFX.PlayDoubleJumpVFX(this.transform.position + new Vector3(0f, 4f, 0f));
                doubleJump = true;
                timeJumping = Time.time;
                fallStartTime = Time.time;
                OnAir = true;

                states = States.JUMP;
                jump = Jump.JUMP;
                this.GetComponent<Rigidbody>().velocity = Vector3.zero;
                this.GetComponent<Rigidbody>().AddForce(this.transform.up * jumpForce * 1f, ForceMode.Impulse);
                playerAnim.CrossFadeInFixedTime("DoubleJump", 0.2f);
                doubleJumpFeedback.PlayFeedbacks();

                return true;

            }


        }

        return false;
    }
    #endregion

    #region Funcion que devuelve true si el player aterriza
    bool CheckIfLand()
    {
        RaycastHit hit;
        RaycastHit hit2;

        if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, layerSuelo))
        {
            if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit2, 200, layerSuelo))
            {
                landHeight = hit.point.y;

                float a = 3f * ((gravity * (Time.time - fallStartTime)) / gravity);
                if (((hit.distance < a && hit2.distance < a) || (hit.distance < 0.5f && hit2.distance < 0.5f)))
                {

                    timeLanding = Time.time;
                    if (!playerAnim.GetCurrentAnimatorStateInfo(0).IsName("Land"))
                        playerAnim.CrossFadeInFixedTime("Land", 0.3f);
                    states = States.JUMP;
                    OnAir = false;

                    jump = Jump.LAND;
                    Invoke("ActiveDoubleJump", 0.1f);
                    moveDirSaved = new Vector3();
                    //landFeedback.PlayFeedbacks();

                    return true;
                }
            }
        }
        return false;
    }

    void ActiveDoubleJump()
    {
        doubleJump = false;

    }
    #endregion

    ////////////

    ////////////

    //       UPDATE

    ////////////

    ////////////

    #region En general no hay que tocar nada del UPDATE casi todo lo que hay que tocar es de las funciones

    Vector3 dirDash;
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
                this.gameObject.layer = 3;

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
                RotatePlayer(3);
                movementController.transform.localPosition = new Vector3();


                //if (enemy != Vector3.zero)
                //{
                //    Vector3 enem = enemy;
                //    enem.y = player.transform.position.y;
                //    player.transform.LookAt(enem);

                //}

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
                        if (CheckIfJump())
                            break;
                        break;
                    case Attacks.AIR:
                        if (CheckIfDash())
                        {
                            dashDown = true;
                            break;
                        }
                        if (CheckIfJump())
                            break;
                        break;
                    case Attacks.FALL:
                        if (CheckIfDash())
                        {
                            dashDown = true;
                            break;
                        }
                        if (CheckIfJump())
                            break;
                        break;
                    case Attacks.RUN:
                        if (CheckIfDash())
                        {
                            dashDown = false;
                            break;
                        }
                        if (CheckIfJump())
                            break;
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
                this.gameObject.layer = 12;

                this.GetComponent<Rigidbody>().drag = 15;
                this.transform.GetChild(0).transform.localEulerAngles = new Vector3(0, this.transform.GetChild(0).transform.localEulerAngles.y, 0);
                if (CheckIfJump())
                    break;
                if (CheckAtaques())
                    break;

                switch (dash)
                {
                    case Dash.START:
                        if ((Time.time - delayDash) > 0.02f)
                        {
                            playerAnim.speed = 1;



                            Vector3 dir;
                            if (dashDirection == new Vector2(0, -1))
                            {
                                player.transform.GetChild(3).transform.localPosition += new Vector3(dashDirection.x, 0, dashDirection.y).normalized;
                                dir = this.transform.position - player.transform.GetChild(3).transform.position;

                                player.transform.GetChild(3).transform.localPosition = new Vector3();
                            }
                            else
                            {
                                movementController.transform.localPosition -= new Vector3(dashDirection.x, 0, dashDirection.y).normalized;
                                dir = this.transform.position - movementController.transform.position;

                                movementController.transform.localPosition = new Vector3();
                            }

                            dirDash = dir;


                            delayDash = Time.time;
                            dash = Dash.DASH;

                        }
                        break;
                    case Dash.DASH:
                        if ((Time.time - delayDash) > dashDuration)
                        {
                            delayDash = Time.time;

                            dash = Dash.END;




                        }
                        if ((Time.time - delayDash) > dashDuration- (dashDuration/4))
                        {

                            if (CheckIfDash())
                                break;



                        }
                        this.GetComponent<Rigidbody>().AddForce(dirDash * dashSpeed * Time.deltaTime, ForceMode.Impulse);

                        if (dashDown)
                        {
                            RaycastHit hit;

                            if (Physics.Raycast(transform.position + new Vector3(0, 0.3f, 0), transform.TransformDirection(-this.transform.up), out hit, 200, 1 << 7))
                            {
                                if ((hit.distance > 0.25f))
                                {
                                    this.GetComponent<Rigidbody>().AddForce(-this.transform.up * 10000 * Time.fixedDeltaTime, ForceMode.Force);
                                }
                                else
                                {
                                    OnAir = false;
                                }
                            }
                            else
                            {
                                OnAir = false;
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
                            OnAir = false;


                            delayDash = Time.time;
                            moveDirSaved = new Vector3();

                            if (CheckIfDash())
                                break;

                            if (CheckIfIsFalling())
                            {
                                OnAir = true;

                                break;
                            }
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
                this.gameObject.layer = 3;


                if (CheckIfDash())
                {
                    dashDown = true;

                    break;
                }
                if (CheckIfJump())
                    break;

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
                        if ((Time.time - timeLanding) > 0.05f)
                        {
                            //Debug.Log("Landed");
                            //landVFX.transform.position = this.transform.position;
                            //landVFX.Play();
                            //landVFX.PlayDustVFX(this.transform.position);
                            CallItemOnJump();
                            player.transform.GetChild(1).Rotate(new Vector3(0, 1, 0), -90);
                            playerAnim.CrossFadeInFixedTime("Idle", 0.3f);
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
                this.gameObject.layer = 3;

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
                this.gameObject.layer = 3;

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
                this.gameObject.layer = 3;

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
    #endregion

    ////////////

    ////////////

    //       UPDATE

    ////////////

    ////////////


    IEnumerator CallItemUpdate()
    {
        foreach (ItemList i in items)
        {
            i.item.Update(this, i.stacks);
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(CallItemUpdate());
    }

    public void CallItemOnKill(Enemy enemy)
    {
        foreach (ItemList i in items)
        {
            i.item.OnKill(this, enemy, i.stacks);
        }
    }
    public void CallItemOnCrit(Enemy enemy)
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

    public void CallItemOnHit()
    {
        foreach (ItemList i in items)
        {
            i.item.OnHit(this, i.stacks);
        }
    }


    public void CallItemOnJump()
    {
        foreach (ItemList i in items)
        {
            Debug.Log("agdsh");
            i.item.OnJump(this, i.stacks);
        }
    }
    public void CallItemOnPickup()
    {
        foreach (ItemList i in items)
        {
            i.item.OnItemPickup(this, i.stacks, i.statType);
        }
    }
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
        if (Time.timeScale == 0)
            return false;

        float delay = 0;
        if (currentComboAttack != -1 && currentComboAttacks != null && (currentComboAttack + 1) != currentComboAttacks.attacks.Length)
        {
            if (currentComboAttack < currentComboAttacks.attacks.Length)
            {
                delay = currentComboAttacks.attacks[currentComboAttack].delay;

            }

        }

        //if (controller.Teleport1)
        //{

        //    Vector3 pos2 = attackTeleport2.GetEnemiePos(this.transform.position);

        //    if (pos2 != Vector3.zero)
        //    {
                
        //        Vector3 pos = (pos2 - this.transform.position).normalized;

        //        pos = attackTeleport2.GetEnemiePos(this.transform.position) - (pos * 2);

        //        RaycastHit hit;

        //        if (Physics.Raycast(pos, transform.TransformDirection(-this.transform.up), out hit, 2000, 1 << 7))
        //        {
        //            this.GetComponent<Rigidbody>().DOMove(pos, 0.25f);
        //            moveDir = Vector3.zero;
        //            moveDirSaved = Vector3.zero;

        //            playerAnim.Play("Desaparecer", 1);
        //            Invoke("Aparecer", 0.1f);

        //            enemy = attackTeleport2.GetEnemiePos(this.transform.position);

        //            this.GetComponent<Rigidbody>().velocity = Vector3.zero;

        //            TeleportTime = Time.time;

        //            if ((Time.time - attackStartTime) >= delay + delayDamage)
        //            {
        //                damageMult = 1;

        //                currentComboAttack = -1;
        //                passiveCombo.Clear();
        //            }
        //            else if (GetAttacks(ComboAtaques.Teleport).attacks.Length - 1 <= currentComboAttack)
        //            {
        //                currentComboAttack = GetAttacks(ComboAtaques.Teleport).attacks.Length - 1;
        //            }
        //            passiveCombo.Add(PassiveCombo.TELEPORT);
        //            attacks = Attacks.GROUND;
        //            currentComboAttacks = GetAttacks(ComboAtaques.Teleport);
        //            PlayAttack();

        //            states = States.ATTACK;

        //            controller.ResetBotonesAtaques();
        //            return true;
        //        }


        //    }

        //}


        if (attackFinished && (Time.time - comboFinishedTime) >= delayCombos)
        {
            if (currentComboAttacks.combo != ComboAtaques.HoldQuadrat && currentComboAttacks.combo != ComboAtaques.HoldQuadratL2 && currentComboAttacks.combo != ComboAtaques.HoldTriangle && currentComboAttacks.combo != ComboAtaques.HoldTriangleL2 && currentComboAttacks.combo != ComboAtaques.Teleport)
                controller.ResetBotonesAtaques();


            passiveCombo.Clear();

            attackFinished = false;
        }
        else if (attackFinished && (Time.time - comboFinishedTime) < delayCombos)
            return false;

        if ((Time.time - attackStartTime) >= delay+0.2f)
        {
            currentComboAttack = -1;
        }

        if ((Time.time - attackStartTime) >= delay)
        {
            enemy = Vector3.zero;



            if (controller.ataqueCuadradoCargado)
            {
                //this.gameObject.layer = 12;
                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    //if ((Time.time - attackStartTime) >= delay + delayDamage)
                    //{
                    //    damageMult = 1;

                    //    currentComboAttack = -1;
                    //    passiveCombo.Clear();
                    //}
                    //else if (GetAttacks(ComboAtaques.air3).attacks.Length - 1 <= currentComboAttack)
                    //{
                    //    currentComboAttack = GetAttacks(ComboAtaques.air3).attacks.Length - 1;
                    //}
                    //passiveCombo.Add(PassiveCombo.HOLDQUADRATAIR);

                    //moveDirSaved = new Vector3();
                    //attacks = Attacks.AIR;
                    //currentComboAttacks = GetAttacks(ComboAtaques.air3);
                    //PlayAttack();
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
                //this.gameObject.layer = 12;

                if (enemieTarget.GetEnemie(this.transform.position) != Vector3.zero)
                {
                    //Vector3 pos = (this.transform.position - enemieTarget.GetEnemie(this.transform.position)).normalized;
                    //pos = enemieTarget.GetEnemie(this.transform.position) + (pos * 2);
                    //this.transform.position = pos;
                    player.transform.LookAt(enemieTarget.GetEnemie(this.transform.position));

                }

                if (states == States.JUMP)
                {
                    //if ((Time.time - attackStartTime) >= delay + delayDamage)
                    //{
                    //    damageMult = 1;

                    //    currentComboAttack = -1;
                    //    passiveCombo.Clear();
                    //}
                    //else if (GetAttacks(ComboAtaques.air4).attacks.Length - 1 <= currentComboAttack)
                    //{
                    //    currentComboAttack = GetAttacks(ComboAtaques.air4).attacks.Length - 1;
                    //}
                    //passiveCombo.Add(PassiveCombo.HOLDTRIANGLEAIR);

                    //moveDirSaved = new Vector3();
                    //attacks = Attacks.AIR;
                    //currentComboAttacks = GetAttacks(ComboAtaques.air4);
                    //PlayAttack();
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

                if (states == States.JUMP && (currentComboAttack == -1 || currentComboAttacks.combo == ComboAtaques.air1))
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
                else if (states != States.JUMP || states == States.JUMP && currentComboAttack != -1)
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
            states = States.DELAYMOVE;
        }
    }
    float dashDuration;
    bool CheckIfDash()
    {

        if (controller.GetDash() && Time.timeScale != 0)
        {
            this.gameObject.layer = 12;

            controller.ResetBotonesAtaques();

            if (dashCount < dashConsecutivos)
            {
                if ((Time.time - delayDash) > 0.35f)
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
            if(!OnAir)
            {
                playerAnim.CrossFadeInFixedTime("Dash", 0.1f);
                dashDuration = 0.25f;
            }
            else
            {
                playerAnim.CrossFadeInFixedTime("DashAir", 0.1f);
                dashDuration = 0.2f;
            }

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

            if (controller.RightTriggerPressed())
            {
                Invoke("StartRun", 0.25f);

                moves = Moves.RUN;
                playerAnim.CrossFadeInFixedTime("Run", 0.2f);
                StartCoroutine(RunFeedback(0.3f));
                StartCoroutine(RunFeedback(0));

            }
            else
            {
                Invoke("EndRun", 0.25f);

                moves = Moves.WALK;
                playerAnim.CrossFadeInFixedTime("Walk", 0.2f);
                StartCoroutine(WalkFeedbak(0.42f));
                StartCoroutine(WalkFeedbak(0));


            }
        }
        else if (states == States.MOVE)
        {
            if (controller.RightTriggerPressed() && moves == Moves.WALK)
            {
                Invoke("StartRun", 0.25f);
                moves = Moves.RUN;
                playerAnim.CrossFadeInFixedTime("Run", 0.2f);
                StartCoroutine(RunFeedback(0.3f));
                StartCoroutine(RunFeedback(0));

            }
            else if (!controller.RightTriggerPressed() && moves == Moves.RUN)
            {
                Invoke("EndRun", 0.25f);

                moves = Moves.WALK;
                playerAnim.CrossFadeInFixedTime("Walk", 0.2f);
                StartCoroutine(WalkFeedbak(0.42f));
                StartCoroutine(WalkFeedbak(0));

            }
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
    void RotatePlayer(float speed)
    {
        if (controller.LeftStickValue().magnitude > 0.2f)
        {
            movementController.transform.localPosition += new Vector3(controller.LeftStickValue().x, 0, controller.LeftStickValue().y).normalized;
            var targetRotation = Quaternion.LookRotation(movementController.transform.position - player.transform.position);

            // Smoothly rotate towards the target point.
            player.transform.rotation = Quaternion.Slerp(player.transform.rotation, targetRotation, speed * Time.deltaTime);

        }
    }
    void Move(float velocity)
    {
        RotatePlayer(10);

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

        }
    }
    //public void GetDamage(float damage)
    //{
    //    if ((states != States.HIT) && states != States.DEATH)
    //    {
    //        currentHealth -= damage;
    //        SetHealth();
    //        if (currentHealth <= 0)
    //        {
    //            states = States.DEATH;
    //            deathTime = Time.time;
    //            RoomManager.Instance.ResetRoomManager();
    //            playerAnim.CrossFadeInFixedTime("Death", 0.2f);
    //            cameraAAnims.CrossFadeInFixedTime("Death", 0.2f);
    //        }
    //        else
    //        {
    //            states = States.HIT;
    //            hitTime = Time.time;
    //            playerAnim.speed = 1;
    //            playerAnim.CrossFadeInFixedTime("Hit1", 0.2f);
    //            hitFeedback.PlayFeedbacks();
    //        }

    //    }
    //}
    private void OnTriggerEnter(Collider other)
    {

    }

    public void HitEffect()
    {
        hitTime = Time.time;
        playerAnim.speed = 1;
        playerAnim.CrossFadeInFixedTime("Hit1", 0.2f);
        hitFeedback.PlayFeedbacks();
    }

    public void DeadEffect()
    {
        deathTime = Time.time;
        playerAnim.CrossFadeInFixedTime("Death", 0.2f);
        cameraAAnims.CrossFadeInFixedTime("Death", 0.2f);
    }
}

