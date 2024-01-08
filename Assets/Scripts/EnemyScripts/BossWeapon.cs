using UnityEngine;

public class BossWeapon : MonoBehaviour
{
    public float offset;
    private float time;
    public float fireDelay;
    public float bulletSpeed;

    public GameObject bullet;
    public Transform pointUp;

    private PlayerMovement player;
    private EnemyController enemyController;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        if (!enemyController.notInRoom && time <= 0f)
        {
            Vector2 direction = player.transform.position - transform.position;
            direction.Normalize();
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            angle -= 75f;

            GameObject newBullet = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle));
            Bullet bulletComponent = newBullet.GetComponent<Bullet>();
            bulletComponent.isEnemyBullet = true;
            bulletComponent.speed = bulletSpeed;

            GameObject newBullet1 = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle + 20f));
            GameObject newBullet2 = Instantiate(bullet, pointUp.position, Quaternion.Euler(0f, 0f, angle - 20f));

            newBullet1.GetComponent<Bullet>().isEnemyBullet = true;
            newBullet1.GetComponent<Bullet>().speed = bulletSpeed;

            newBullet2.GetComponent<Bullet>().isEnemyBullet = true;
            newBullet2.GetComponent<Bullet>().speed = bulletSpeed;

            time = fireDelay;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }
}
