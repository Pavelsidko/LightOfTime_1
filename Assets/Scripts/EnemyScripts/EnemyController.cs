using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack,
    Leap,
    KnockedBack
};

public enum EnemyType
{
    Melee,
    Ranged,
    Spider
};

public class EnemyController : MonoBehaviour
{
    GameObject player;
    Rigidbody2D rb;

    [Header("References")]
    public EnemyFloatingHealthBar healthBar;
    [SerializeField] private HitEffect flashEffect;
    [SerializeField] private Color hurtColor = Color.gray;
    [SerializeField] private AudioSource hitSoundEffect;
    [SerializeField] private GameObject exlamationMark;

    [Header("Enemy Info")]
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public bool notInRoom = false;

    [Header("Health")]
    public float health;
    public float maxHealth = 3f;

    [Header("Movement")]
    public float wanderRange;
    public float range;
    public float speed;

    [Header("Attack")]
    public float attackRange;
    public float coolDown;
    public bool coolDownAttack = false;
    [Space(5)]
    public float knockBackForce;
    public float stunTime;
    public float knockBackTime;

    [Header("Spider Settings")]
    public float leapSpeed = 10f;
    public float leapRange = 3f;
    public float leapDuration = 0.25f;
    public float leapCoolDown = 1.0f;
    public bool canLeap = true;
    public bool isLeaping = false;

    [Header("Context Steering")]
    [SerializeField]
    private List<SteeringBehaviour> steeringBehaviours;

    [SerializeField]
    private List<Detector> detectors;

    [SerializeField]
    private AIData aiData;

    [SerializeField]
    private float detectionDelay = 0.05f, aiUpdateDelay = 0.06f;

    [SerializeField]
    private ContextSolver movementDirectionSolver;


    private Animator anim;
    private Weapon weapon;
    private Vector2 wayPoint;
    private Coroutine leapCoroutine;
    private KnockbackFeedBack knockbackFeedBack;



    private void Awake()
    {
        healthBar = GetComponentInChildren<EnemyFloatingHealthBar>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
        anim = GetComponent<Animator>();

        weapon = GetComponentInChildren<Weapon>();
        rb = GetComponent<Rigidbody2D>();
        knockbackFeedBack = GetComponent<KnockbackFeedBack>();

        exlamationMark.SetActive(false);

        //Detecting Player and Obstacles around
        InvokeRepeating("PerformDetection", 0, detectionDelay);
    }
    private void PerformDetection()
    {
        foreach (Detector detector in detectors)
        {
            detector.Detect(aiData);
        }
    }

    void Update()
    {
        if (PauseController.isPaused) { return; }

        if (knockbackFeedBack != null && knockbackFeedBack.IsKnockbackActive)
        {
            if (currState != EnemyState.KnockedBack)
            {
                currState = EnemyState.KnockedBack;
            }
            return;
        }

        switch (currState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Wander:
                anim.SetBool("Is_Walking", true);
                StartCoroutine(Wander());
                break;
            case EnemyState.Follow:
                anim.SetBool("Is_Walking", true);
                StartCoroutine(Follow());
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.KnockedBack:
                break;
            case EnemyState.Leap:
                if (leapCoroutine == null)
                {
                    leapCoroutine = StartCoroutine(Leap());
                }
                break;
        }

        if (!notInRoom && currState is not (EnemyState.Die or EnemyState.Leap or EnemyState.KnockedBack))
        {
            if (enemyType == EnemyType.Spider && (!canLeap && isLeaping))
                return;

            if (enemyType == EnemyType.Spider && IsPlayerInRange(leapRange) && canLeap)
            {
                exlamationMark.SetActive(true);
                currState = EnemyState.Leap;
            }

            else if (IsPlayerInRange(attackRange))
            {
                currState = EnemyState.Attack;
                UpdateSpriteDirection();
            }

            else if (aiData.currentTarget != null)
            {
                currState = EnemyState.Follow;
                exlamationMark.SetActive(true);
                UpdateSpriteDirection();
            }
            else if (aiData.GetTargetsCount() > 0)
            {
                //Target acquisition logic
                aiData.currentTarget = aiData.targets[0];
            }
            else
            {
                aiData.currentTarget = null;
                currState = EnemyState.Wander;
                UpdateSpriteDirection();
                exlamationMark.SetActive(false);
            }
        }
        else
        {
            exlamationMark.SetActive(false);
            currState = EnemyState.Idle;
        }
    }
    private void UpdateSpriteDirection()
    {
        Vector2 velocity = rb.velocity;

        // Только если есть значительное движение по X
        if (Mathf.Abs(velocity.x) > 0.1f)
        {
            float newScaleX = Mathf.Abs(transform.localScale.x);

            // Если движется влево - делаем scaleX отрицательным
            if (velocity.x < 0)
            {
                newScaleX = -newScaleX;
            }

            transform.localScale = new Vector3(newScaleX,
                                             transform.localScale.y,
                                             transform.localScale.z);
        }
    }

    public bool IsPlayerInRange(float range)
    {
        if (transform == null || player == null || player.transform == null)
        {
            return false;
        }
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }

    IEnumerator Wander()
    {
        Vector2 moveDirection = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        rb.velocity = moveDirection * speed;
        yield return new WaitForSeconds(aiUpdateDelay);
    }

    IEnumerator Follow()
    {
        //Chase logic
        Vector2 moveDirection = movementDirectionSolver.GetDirectionToMove(steeringBehaviours, aiData);
        rb.velocity = moveDirection * speed * 2;
        yield return new WaitForSeconds(aiUpdateDelay);

        // Двигаемся к запомненной позиции
        // Vector2 moveDirection = contextSteering.GetDirection(lastKnownPlayerPosition.Value, obstacleMask);


        UpdateSpriteDirection();
    }

    private void Attack()
    {
        if (!coolDownAttack && !notInRoom)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    GameController.KnockBackEffectPlayer(transform.position, knockBackForce, stunTime, knockBackTime);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Spider):
                    GameController.DamagePlayer(1);
                    GameController.KnockBackEffectPlayer(transform.position, knockBackForce, stunTime, knockBackTime);
                    StartCoroutine(CoolDown());
                    break;
            }
        }
    }

    private IEnumerator Leap()
    {
        if (!canLeap || isLeaping) yield break;

        canLeap = false;
        isLeaping = true;
        Debug.Log("Прыгает!");

        Vector2 direction = (player.transform.position - transform.position).normalized;

        // Apply force instead of setting velocity directly
        rb.AddForce(direction * leapSpeed * rb.mass, ForceMode2D.Impulse);

        yield return new WaitForSeconds(leapDuration);

        // Gradually reduce velocity instead of instantly stopping
        float brakeTime = 0.2f;
        float brakeSpeed = rb.velocity.magnitude / brakeTime;
        while (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, Time.deltaTime * brakeSpeed);
            yield return null;
        }
        rb.velocity = Vector2.zero;

        isLeaping = false;
        StartCoroutine(CoolDown());
        yield return new WaitForSeconds(leapCoolDown);
        canLeap = true;
        leapCoroutine = null;

        // State transition
        if (IsPlayerInRange(attackRange))
        {
            currState = EnemyState.Attack;
        }
        else if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
        else
        {
            currState = EnemyState.Wander;
        }
    }

    private void Idle()
    {
    }

    private bool isAlive = true;
    public void TakeDamage(float damageAmount)
    {
        CameraShaker.duration = 0.2f;
        CameraShaker.start = true;
        if (gameObject != null)
        {
            anim.SetTrigger("Is_Hurt");
            hitSoundEffect.Play();
            if (flashEffect != null)
                flashEffect.Flash(hurtColor);

            health -= damageAmount;
            healthBar.UpdateHealthBar(health, maxHealth);
            if (health <= 0 && isAlive)
            {
                isAlive = false;
                StartCoroutine(DeathCoroutine());
            }
        }
    }

    public IEnumerator DeathCoroutine()
    {
        float animDuration = 0.8f;
        anim.SetBool("Is_Alive", false);
        if (!gameObject.CompareTag("Boss"))
        {
            yield return new WaitForSeconds(animDuration);
        }
        else
        {
            notInRoom = true;
            yield return new WaitForSeconds(3f);
        }
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        if (RoomController.instance != null) { RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine()); }
        StopAllCoroutines();
        Destroy(gameObject);
    }

    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }

    //void OnDrawGizmosSelected()
    //{

    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, attackRange);

    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawWireSphere(transform.position, leapRange);

    //    Gizmos.color = Color.green;
    //    Gizmos.DrawWireSphere(transform.position, range);
    //}
}
