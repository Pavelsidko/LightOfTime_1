using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    public Rigidbody2D rb;
    public TextMeshProUGUI collectedCoinsText; 

    public Joystick joystick;
    public Joystick weaponJoystick;
    private Animator anim;
    private bool facingLeft = true;


    private void OnEnable()
    {
        GameController.onPlayerDeath += DisablePlayerMovement;
    }
    private void OnDisable()
    {
        GameController.onPlayerDeath -= DisablePlayerMovement;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        EnablePlayerMovement();
    }

   
    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;


        speed = GameController.MoveSpeed;
        rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);
        anim.SetFloat("MoveMagnitude", horizontal * vertical);

        if (horizontal != 0 && vertical != 0)
        {
            anim.SetBool("Idle", false);
        }
        else anim.SetBool("Idle", true);


        //float weaponHorizontal = weaponJoystick.Horizontal;
        //float weaponVertical = weaponJoystick.Vertical;

        //anim.SetFloat("ShootX", weaponHorizontal);
        //anim.SetFloat("ShootY", weaponVertical);

        //if (weaponHorizontal != 0 && weaponVertical != 0)
        //{
        //    //anim.SetBool("Idle", false);
        //    anim.SetBool("Shooting", true);
        //}
        //else
        //{
        //    anim.SetBool("Shooting", false);
        //    anim.SetBool("Idle", true);
        //}

    }

    private void DisablePlayerMovement()
    {
        anim.enabled= false;
        rb.bodyType = RigidbodyType2D.Static;
    }
    private void EnablePlayerMovement()
    {
        anim.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
        }
    }



}
