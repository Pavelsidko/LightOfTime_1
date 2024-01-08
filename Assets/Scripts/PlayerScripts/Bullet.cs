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

    // Update is called once per frame
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
            if (other.collider.CompareTag("Block"))
            {
                Destroy(gameObject);
            }
        }

        //if (isEnemyBullet)
        //{
        //    currPos = transform.position;
        //    transform.position = Vector2.MoveTowards(transform.position, playerPos, 5f * Time.deltaTime);
        //    if (currPos == lastPos)
        //    {
        //        Destroy(gameObject);
        //    }
        //    lastPos = currPos;
        //}
    }


    public void GetPlayer(Transform player)
    { 
        playerPos = new Vector2(player.position.x, player.position.y);

    }





    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isEnemyBullet)
        {
            GameController.DamagePlayer(0.5f);
            //Vector2 direction = (collision.transform.position - transform.position).normalized;
            //Vector2 knockBack = direction * knockBackForce;
            //GameController.KnockBack(knockBack);
            //collision.gameObject.GetComponent<EnemyController>().Death();
            Destroy(gameObject);
        }
        //if (collision.tag == "Enemy" && !isEnemyBullet)
        //{
        //    collision.gameObject.GetComponent<EnemyController>().Death();
        //    Destroy(gameObject);
        //}


    }



    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    
}
