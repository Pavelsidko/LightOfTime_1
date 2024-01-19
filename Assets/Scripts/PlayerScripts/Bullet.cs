using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float distance;
    public float lifeTime = 3;
    //public LayerMask layerMask;
    public bool isEnemyBullet = false;
    private Vector2 lastPos;
    private Vector2 currPos;
    private Vector2 playerPos;
    public float knockBackForce = 20f;
    //GameObject bullet;
    private GameObject player;

    void Start()
    {
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
       
    }

    void Update()
    {
        RaycastHit2D other = Physics2D.Raycast(transform.position, transform.up, distance);
        
        if (other.collider != null)
        {
            if (other.collider.CompareTag("Enemy") && !isEnemyBullet || other.collider.CompareTag("Boss") && !isEnemyBullet)
            {
                other.collider.GetComponent<EnemyController>().TakeDamage(1.5f);
                Destroy(gameObject);
            }
        }

        transform.Translate(Vector2.up * speed * Time.deltaTime);
        if (other.collider != null)
        {
            if (other.collider.CompareTag("Wall"))
            {
                Destroy(gameObject);
            }
        }
    }


    public void GetPlayer(Transform player)
    { 
        playerPos = new Vector2(player.position.x, player.position.y);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && isEnemyBullet)
        {
            GameController.DamagePlayer(0.5f);
            Destroy(gameObject);
        }
        else if ((collision.CompareTag("Enemy") || collision.CompareTag("Boss")) && !isEnemyBullet)
        {
            collision.GetComponent<EnemyController>().TakeDamage(1.5f);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    
}
