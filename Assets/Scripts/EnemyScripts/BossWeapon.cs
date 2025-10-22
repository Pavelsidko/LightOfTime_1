using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    
    public float fireDelay;


    public Animator anim;
    public GameObject bullet;
    public Transform pointUp;

    private float time;
    private PlayerMovement player;
    private EnemyController enemyController;
    [SerializeField] private AudioSource shootSoundEffect;



    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        enemyController = GetComponent<EnemyController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (PauseController.isPaused) { return; }

        if (!enemyController.notInRoom && time <= 0f)
        {
            anim.SetBool("IsAttack", true);
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 75f;

            shootSoundEffect.Play();
            GameObject newBullet = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle));
            GameObject newBullet1 = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle + 30f));
            GameObject newBullet2 = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle - 30f));

            time = fireDelay;
            anim.SetBool("IsAttack", false);
        }
        else
        {
            anim.SetBool("IsAttack", false);
            time -= Time.deltaTime;
        }
    }
}
