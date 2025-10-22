using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public static GameController instance;
    private static GameObject player;

    private static float health = 20;
    private static int maxHealth = 20;
    private static float moveSpeed = 5f;

    private static float fireRate = 0.6f;
    private static float maxFireRate = 0.1f;
    private static float bulletSize = 0.5f;

    public TextMeshProUGUI healthText;

    private int bootCount = 0;
    private int screwCount = 0;
    private bool potionCollected = false;

    [SerializeField] private HitEffect playerHitEffect;
    [SerializeField] private Color damageColor = Color.gray;
    [SerializeField] private Color healColor = Color.red;
    [SerializeField] private Color BootColor = Color.yellow;
    [SerializeField] private Color ScrewColor = Color.blue;

    public static event Action onPlayerDeath;

    public List<string> collectedNames = new List<string>();

    public static float Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        player = GameObject.FindGameObjectWithTag("Player");

        if (playerHitEffect == null && player != null)
            playerHitEffect = player.GetComponent<HitEffect>();

        ResetProgress();
    }

    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(float damage)
    {
        CameraShaker.duration = 0.3f;
        CameraShaker.start = true;
        health -= damage;

        if (instance.playerHitEffect != null)
        {
            instance.playerHitEffect.Flash(instance.damageColor);
        }
            
        if (health <= 0)
        {
            KillPlayer();
        }
    }

    public static void KnockBackEffectPlayer(Vector3 attackerPosition, float knockBackForce, float stunTime, float knockBackTime)
    {
        KnockbackFeedBack knockback = player.GetComponent<KnockbackFeedBack>();
        if (knockback != null)
        {
            Debug.Log("Player Knocked back!");
            Vector2 knockbackDirection = (player.transform.position - attackerPosition).normalized;
            knockback.ApplyKnockback(knockbackDirection, knockBackForce, stunTime, knockBackTime);
        }
    }

    public static void HealPlayer(float healAmount)
    {
        health = Mathf.Min(maxHealth, Health += healAmount);

        if (instance.playerHitEffect != null)
        {
            instance.playerHitEffect.Flash(instance.healColor);
        }  
    }

    public static void MoveSpeedChange(float speed)
    {   
        moveSpeed += speed;

        if (instance.playerHitEffect != null)
        {
            instance.playerHitEffect.Flash(instance.BootColor);
        }
    }

    public static void FireRateChange(float rate)
    {
        fireRate -= rate;

        if (fireRate < maxFireRate)
            fireRate = maxFireRate;

        if (instance.playerHitEffect != null)
            instance.playerHitEffect.Flash(instance.ScrewColor);
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;

        if (instance.playerHitEffect != null)
            instance.playerHitEffect.Flash(instance.ScrewColor);
    }

    public void UpdateCollectedItems(Collection item)
    {
        collectedNames.Add(item.item.name);

        switch (item.item.name)
        {
            case "Boot":
                bootCount++;
                break;
            case "Screw":
                screwCount++;
                break;
            case "Potion":
                potionCollected = true;
                break;
        }

        // Синергия Boot + Screw → уменьшение fireRate
        while (bootCount > 0 && screwCount > 0)
        {
            FireRateChange(0.1f);
            bootCount--;
            screwCount--;

            if (fireRate <= maxFireRate)
                break;
        }
    }

    public void ResetProgress()
    {
        bootCount = 0;
        screwCount = 0;
        potionCollected = false;

        fireRate = 0.6f;
        moveSpeed = 5f;
        bulletSize = 0.5f;
        health = maxHealth;

        collectedNames.Clear();
    }

    private static void KillPlayer()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //Destroy(player);

        Debug.Log("You're dead");
        onPlayerDeath?.Invoke();
    }
}
