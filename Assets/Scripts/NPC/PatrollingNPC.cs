using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrollingNPC : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private List<Transform> patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waitTimeAtPoint = 1f;

    [Header("Chase Settings")]
    [SerializeField] private float chaseSpeed = 3.5f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private BoxCollider2D detectionCollider;
    [SerializeField] private Transform guardPoint; // Заранее определенная точка

    private int currentPatrolIndex = 0;
    private float waitCounter;
    private bool isWaiting = false;
    private bool isChasing = false;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (detectionCollider == null)
        {
            detectionCollider = GetComponentInChildren<BoxCollider2D>();
            if (detectionCollider == null)
            {
                Debug.LogError("No collider assigned or found on NPC!");
            }
        }

        detectionCollider.isTrigger = true;
    }

    private void Update()
    {
        if (isChasing)
        {
            ChaseBehavior();
        }
        else
        {
            PatrolBehavior();
        }
    }

    private void PatrolBehavior()
    {
        if (patrolPoints.Count == 0) return;

        Vector2 direction = patrolPoints[currentPatrolIndex].position - transform.position;

        if (direction.magnitude < 0.1f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitCounter = waitTimeAtPoint;
            }
            else
            {
                waitCounter -= Time.deltaTime;
                if (waitCounter <= 0)
                {
                    isWaiting = false;
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
                }
            }
        }
        else
        {
            direction.Normalize();
            rb.velocity = direction * patrolSpeed;
        }
    }

    private void ChaseBehavior()
    {
        if (guardPoint == null)
        {
            Debug.LogWarning("No guard point assigned!");
            isChasing = false;
            return;
        }

        Vector2 direction = guardPoint.position - transform.position;

        if (direction.magnitude <= stoppingDistance)
        {
            rb.velocity = Vector2.zero;
            // Дополнительные действия при достижении точки
        }
        else
        {
            direction.Normalize();
            rb.velocity = direction * chaseSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isChasing = false;
            rb.velocity = Vector2.zero;
        }
    }

    // Визуализация в редакторе
    private void OnDrawGizmos()
    {
        // Рисуем точки патрулирования
        if (patrolPoints != null && patrolPoints.Count > 0)
        {
            Gizmos.color = Color.blue;
            for (int i = 0; i < patrolPoints.Count; i++)
            {
                if (patrolPoints[i] != null)
                {
                    Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
                    if (i < patrolPoints.Count - 1 && patrolPoints[i + 1] != null)
                    {
                        Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
                    }
                }
            }

            if (patrolPoints[0] != null && patrolPoints.Count > 1 && patrolPoints[patrolPoints.Count - 1] != null)
            {
                Gizmos.DrawLine(patrolPoints[0].position, patrolPoints[patrolPoints.Count - 1].position);
            }
        }

        // Рисуем зону обнаружения
        if (detectionCollider != null)
        {
            Gizmos.color = Color.red;
        }

        // Рисуем точку охраны
        if (guardPoint != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(guardPoint.position, 0.3f);
            Gizmos.DrawLine(transform.position, guardPoint.position);
        }
    }
}
