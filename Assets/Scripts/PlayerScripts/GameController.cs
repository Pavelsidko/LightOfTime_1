using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{

    public static GameController instance;
    private static PlayerMovement player;
    //public static Vector2 knockBack = ;
    private static float health = 6;
    private static int maxHealth = 6;
    private static float moveSpeed = 5f;
    private static float fireRate = 0.6f;
    private static float maxFireRate = 0.1f;
    public TextMeshProUGUI healthText;
    private static float bulletSize = 0.5f;
    private bool bootCollected = false;
    private bool screwCollected = false;
    private bool potionCollected = false;
    private static int coinsCollected = 0;

    public static event Action onPlayerDeath;


    public List<string> collectedNames = new List<string>();

    public static float Health { get => health; set => health = value; }
    public static int MaxHealth { get => maxHealth; set => maxHealth = value; }
    public static float MoveSpeed { get => moveSpeed; set => moveSpeed = value; }
    public static float FireRate { get => fireRate; set => fireRate = value; }
    public static float BulletSize { get => bulletSize; set => bulletSize = value; }
    public static int CoinsCollected { get => coinsCollected; set => coinsCollected = value; }



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

    }


    void Update()
    {
        healthText.text = "Health: " + health;
    }

    public static void DamagePlayer(float damage)
    {
        health -= damage;


        if (health <= 0)
        {
            KillPlayer();
        }

    }



    public static void HealPlayer(float healAmount)
    { 
        health = Mathf.Min(maxHealth, Health += healAmount);
    }

    public static void MoveSpeedChange(float speed)
    {
        
        moveSpeed += speed;
    }
    public static void FireRateChange(float rate)
    {
        fireRate -= rate;
    }

    public static void BulletSizeChange(float size)
    {
        bulletSize += size;
    }

    public static void CoinAmountChange(int amount)
    {
        coinsCollected += amount;
    }

    //public static void KnockBack(Vector2 kb)
    //{
    //    player.rb.AddForce(kb, ForceMode2D.Impulse);
    //}


    public void UpdateCollectedItems(Collection item)
    {
        collectedNames.Add(item.item.name);

        foreach(string i in collectedNames)
        {
            switch (i)
            {
                case "Boot":
                    bootCollected = true; break;
                case "Screw":
                    screwCollected = true; break;
                case "Potion":
                    potionCollected = true; break;
            }
        }


        if(bootCollected && screwCollected)
        {   
            bootCollected= false;
            screwCollected= false;
            FireRateChange(0.1f);
            if (fireRate < maxFireRate)
            {
                
                fireRate = maxFireRate;
            }
        }
    }


    private static void KillPlayer()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        //Destroy(player);

        Debug.Log("Youre dead");
        onPlayerDeath?.Invoke();
    }

}
