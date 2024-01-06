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

    public static int collectedCoinsAmount = 0;
    public Joystick joystick;
    public Joystick weaponJoystick;
    private Animator anim;
    private bool facingLeft = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

   
    void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;


        speed = GameController.MoveSpeed;
        rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedCoinsText.text = ": " +collectedCoinsAmount;

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

   

}