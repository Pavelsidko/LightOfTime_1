using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Weapon : MonoBehaviour
{
    public float offset;
    private float time;
    public float fireDelay;
    private EnemyController enemyController;
    public GameObject bullet;
    public Transform point;
    private PlayerMovement player;
    private bool canShoot; // Added flag to control shooting

    public Joystick weaponJoystick;
    public GunType gunType;

    [SerializeField] private AudioSource shootingSoundEffect;

    public enum GunType
    {
        Player,
        Enemy
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        enemyController = GameObject.FindObjectOfType(typeof(EnemyController)) as EnemyController;

        // Initialize canShoot to false
        canShoot = false;

        // Start a coroutine to enable shooting after 5 seconds
        StartCoroutine(EnableShootingAfterDelay());
    }

    IEnumerator EnableShootingAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        canShoot = true;
    }

    void Update()
    {
        if (gunType == GunType.Player)
        {
            fireDelay = GameController.FireRate;

            float rotateZ = Mathf.Atan2(weaponJoystick.Vertical, weaponJoystick.Horizontal) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotateZ + offset);

            if (canShoot && time <= 0f)
            {
                if (weaponJoystick.Horizontal > 0 || weaponJoystick.Vertical > 0 || weaponJoystick.Horizontal < 0 || weaponJoystick.Vertical < 0)
                {
                    shootingSoundEffect.Play();
                    Instantiate(bullet, point.position, transform.rotation);
                    bullet.GetComponent<Bullet>().isEnemyBullet = false;
                    time = fireDelay;
                }
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
        else if (gunType == GunType.Enemy)
        {
            EnemyShooting();
        }
    }

    public void EnemyShooting()
    {
        if (canShoot && enemyController.IsPlayerInRange(enemyController.range) && enemyController.enemyType == EnemyType.Ranged)
        {
            Vector3 difference = player.transform.position - transform.position;
            float rotZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rotZ + offset);

            if (time <= 0f)
            {
                shootingSoundEffect.Play();
                Instantiate(bullet, point.position, transform.rotation);
                bullet.GetComponent<Bullet>().isEnemyBullet = true;
                time = fireDelay;
            }
            else
            {
                time -= Time.deltaTime;
            }
        }
    }
}
