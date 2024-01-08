using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Wander,
    Follow,
    Die,
    Attack
};


public enum EnemyType
{
    Melee,
    Ranged
};



public class EnemyController : MonoBehaviour
{
    GameObject player;
    public EnemyState currState = EnemyState.Idle;
    public EnemyType enemyType;
    public float health, maxHealth = 3f;
    public EnemyFloatingHealthBar healthBar;
    public float range;
    public float speed;
    public float attackRange;
    private bool coolDownAttack = false;
    public float coolDown;
    private Animator anim;
    private Weapon weapon;
    public bool notInRoom = false;
    [SerializeField] private HitEffect flashEffect;
    Vector2 wayPoint;
    [SerializeField] float wanderRange;

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
        weapon = GameObject.FindObjectOfType(typeof(Weapon)) as Weapon;
    }


    void Update()
    {
        switch (currState)
        {
            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Wander:
                Wander();
                break;
            case EnemyState.Follow:
                Follow();
                break;
            case EnemyState.Die:
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }

        if (!notInRoom)
        {
            if (IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Follow;

                // Calculate the direction to the player
                Vector3 direction = player.transform.position - transform.position;

                // Flip the sprite based on the direction
                if (direction.x < 0 && transform.localScale.x > 0)
                {
                    // If the player is on the left and the sprite is not already flipped, flip the sprite
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (direction.x > 0 && transform.localScale.x < 0)
                {
                    // If the player is on the right and the sprite is flipped, revert the flip
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
            else if (!IsPlayerInRange(range) && currState != EnemyState.Die)
            {
                currState = EnemyState.Wander;
            }

            if (Vector3.Distance(transform.position, player.transform.position) <= attackRange)
            {
                currState = EnemyState.Attack;

                // Flip the sprite based on the direction to the player
                Vector3 direction = player.transform.position - transform.position;
                if (direction.x < 0 && transform.localScale.x > 0)
                {
                    // If the player is on the left and the sprite is not already flipped, flip the sprite
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
                else if (direction.x > 0 && transform.localScale.x < 0)
                {
                    // If the player is on the right and the sprite is flipped, revert the flip
                    transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
                }
            }
        }
        else
        {
            currState = EnemyState.Idle;
        }
    }


    public bool IsPlayerInRange(float range)
    {
        if (transform == null || player == null || player.transform == null)
        {
            // Здесь вы можете обработать ситуацию, когда один из объектов равен null
            // Например, вы можете записать ошибку в журнал или выбросить исключение
            return false;
        }
        return Vector3.Distance(transform.position, player.transform.position) <= range;
    }


    void Wander()
    {
        transform.position = Vector2.MoveTowards(transform.position, wayPoint, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, wayPoint) < wanderRange)
        {
            SetNewDestination();
        }
        if (IsPlayerInRange(range))
        {
            currState = EnemyState.Follow;
        }
    }

    void SetNewDestination()
    {
        wayPoint = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
    }



    void Follow()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
    }

    private void Attack()
    {
        if (!coolDownAttack && !notInRoom)
        {
            switch (enemyType)
            {
                case (EnemyType.Melee):
                    GameController.DamagePlayer(1);
                    StartCoroutine(CoolDown());
                    break;
                case (EnemyType.Ranged):
                    StartCoroutine(CoolDown());
                    break;
            }
        }

    }

    private void Idle()
    {

    }

    private bool isAlive = true;
    public void TakeDamage(float damageAmount)
    {
        if (gameObject != null)
        {
            flashEffect.Flash();
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
        float animDuration = 0.3f;

        anim.SetBool("Is_Alive", false);
        yield return new WaitForSeconds(animDuration);
        GetComponent<LootBag>().InstantiateLoot(transform.position);
        RoomController.instance.StartCoroutine(RoomController.instance.RoomCoroutine());

        if (gameObject.CompareTag("Boss"))
        {
            Debug.Log("Daun");
        }
        StopAllCoroutines();
        Destroy(gameObject);
    }


    private IEnumerator CoolDown()
    {
        coolDownAttack = true;
        yield return new WaitForSeconds(coolDown);
        coolDownAttack = false;
    }
}
