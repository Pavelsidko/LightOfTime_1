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
        
        

        rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);
        collectedCoinsText.text = "COINS COLLECTED: " + collectedCoinsAmount;
        speed = GameController.MoveSpeed;

        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);
        anim.SetFloat("MoveMagnitude", horizontal * vertical);
        if (horizontal != 0 && vertical != 0)
        {
            anim.SetBool("Idle", false);
        }
        else anim.SetBool("Idle", true);
        

    }

   

}
