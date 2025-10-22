using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    public Rigidbody2D rb;
    public TextMeshProUGUI collectedCoinsText; 

    public Joystick joystick;
    public Joystick weaponJoystick;

    private KnockbackFeedBack knockbackFeedBack;
    private Animator anim;
    private float horizontal, vertical;
    [SerializeField] private AudioSource walkingSoundEffect;
    [SerializeField] private AudioSource coinSoundEffect;
    [SerializeField] private AudioSource flasksSoundEffect;


    [Header("Dash Settings")]
    public Button dashButton;
    private float dashSpeed;
    [SerializeField] private float dashSpeedMultiplier = 1.5f;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashCoolDown = 1f;
    [SerializeField] bool isDashing;
    [SerializeField] bool canDash = true;

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
        knockbackFeedBack = GetComponent<KnockbackFeedBack>();
        dashButton.onClick.AddListener(OnButtonClick);
        EnablePlayerMovement();
    }

   
    void Update()
    {
        if (PauseController.isPaused)
        {
            DisablePlayerMovement();
        }
        else EnablePlayerMovement();

        if (isDashing) return;

        if (knockbackFeedBack != null && knockbackFeedBack.IsKnockbackActive)
            return;

        horizontal = joystick.Horizontal;
        vertical = joystick.Vertical;


        speed = GameController.MoveSpeed;
        rb.velocity = new Vector3(horizontal * speed, vertical * speed, 0);

        anim.SetFloat("MoveX", horizontal);
        anim.SetFloat("MoveY", vertical);
        anim.SetFloat("MoveMagnitude", horizontal * vertical);

        if (horizontal != 0 && vertical != 0)
        {
            anim.SetBool("Idle", false);
            if (!walkingSoundEffect.isPlaying) { walkingSoundEffect.Play(); }
        }
        else
        {
            walkingSoundEffect.Stop();
            anim.SetBool("Idle", true);
        }


        float weaponHorizontal = weaponJoystick.Horizontal;
        float weaponVertical = weaponJoystick.Vertical;

        anim.SetFloat("ShootX", weaponHorizontal);
        anim.SetFloat("ShootY", weaponVertical);

        if (weaponHorizontal != 0 && weaponVertical != 0)
        {
            anim.SetBool("Shooting", true);
        }
        else
        {
            anim.SetBool("Shooting", false);
        }


        if ((weaponHorizontal != 0 && weaponVertical != 0) && (horizontal != 0 && vertical != 0))
        {
            anim.SetBool("WalkAndShoot", true);
        }
        else
        {
            anim.SetBool("WalkAndShoot", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
        }

    }

    public void DisablePlayerMovement()
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
            coinSoundEffect.Play();
            Destroy(other.gameObject);
        }
        if (other.gameObject.CompareTag("Flasks"))
        {
            flasksSoundEffect.Play();
        }
    }

    void OnButtonClick()
    {
        if (canDash)
        {
            Debug.Log("Dash!");
            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        dashSpeed = GameController.MoveSpeed * dashSpeedMultiplier;
        Debug.Log("Player Dashed!");
        canDash = false;
        isDashing = true;
        anim.SetBool("Dash", true);
        rb.velocity = new Vector2(horizontal * dashSpeed, vertical * dashSpeed);
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        anim.SetBool("Dash", false);
        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}
