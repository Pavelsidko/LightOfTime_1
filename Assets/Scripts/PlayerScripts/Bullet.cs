using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public float lifeTime = 3;

    public bool isEnemyBullet = false;
    private Vector2 playerPos;

    public float knockBackForce = 20f;
    public float knockBackTime = 0.15f;
    public float stunTime = 0.5f;

    //private GameObject player; Может быть для самонаводящейся пули

    void Start()
    {
        if (!isEnemyBullet)
        {
            transform.localScale = new Vector2(GameController.BulletSize, GameController.BulletSize);
        }
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
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
            GameController.KnockBackEffectPlayer(transform.position, knockBackForce, stunTime, knockBackTime);
            Destroy(gameObject);
        }
        else if ((collision.CompareTag("Enemy") || collision.CompareTag("Boss")) && !isEnemyBullet)
        {
            collision.GetComponent<EnemyController>().TakeDamage(1f);
            KnockBackEffect(collision);
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Vase") && !isEnemyBullet)
        {
            collision.GetComponent<Vase>().TakeHit();
            Destroy(gameObject);
        }
        else if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    void KnockBackEffect(Collider2D collision)
    {
        KnockbackFeedBack knockback = collision.GetComponent<KnockbackFeedBack>();
        if (knockback != null)
        {
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            knockback.ApplyKnockback(knockbackDirection, knockBackForce, stunTime, knockBackTime);
            collision.GetComponent<EnemyController>().currState = EnemyState.KnockedBack;
        }
    }

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
    }
}
