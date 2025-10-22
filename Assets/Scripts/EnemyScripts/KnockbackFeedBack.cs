using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AdaptivePerformance.VisualScripting;

[RequireComponent(typeof(Rigidbody2D))]
public class KnockbackFeedBack : MonoBehaviour
{
    [Header("Knockback Settings")]
    [SerializeField] private float dragDuringKnockback = 5f;

    private Rigidbody2D rb;
    private float originalDrag;
    private bool isKnockbackActive;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalDrag = rb.drag;
    }

    public void ApplyKnockback(Vector2 direction, float customForce, float stunTime, float knockBackTime)
    {
        if (direction.magnitude > 1f)
        {
            direction.Normalize();
        }

        rb.velocity = Vector2.zero; 
        rb.AddForce(direction * customForce, ForceMode2D.Impulse);

        rb.drag = dragDuringKnockback;
        isKnockbackActive = true;
        StartCoroutine(StunTimer(stunTime, knockBackTime));
    }

    IEnumerator StunTimer(float stunTime, float knockBackTime)
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(stunTime);
        rb.drag = originalDrag;
        isKnockbackActive = false;
    }

    // Public property to check if knockback is active
    public bool IsKnockbackActive => isKnockbackActive;
}
