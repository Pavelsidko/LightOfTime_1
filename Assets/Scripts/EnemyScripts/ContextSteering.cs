using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextSteering
{
    private int directionsCount = 8;
    private float[] interest;
    private float[] danger;
    private float[] contextMap;
    private Vector2[] directions;

    private Transform owner;
    private float rayLength;

    public ContextSteering(Transform owner, int directionsCount = 8, float rayLength = 2f)
    {
        this.owner = owner;
        this.directionsCount = directionsCount;
        this.rayLength = rayLength;

        interest = new float[directionsCount];
        danger = new float[directionsCount];
        contextMap = new float[directionsCount];
        directions = new Vector2[directionsCount];

        // Pre-calculate directions
        for (int i = 0; i < directionsCount; i++)
        {
            float angle = i * (360f / directionsCount);
            directions[i] = (Vector2)(Quaternion.Euler(0, 0, angle) * Vector2.right);
        }
    }

    public Vector2 GetDirection(Vector2 targetPosition, LayerMask obstacleMask)
    {
        ResetArrays();

        // Calculate interest (desired direction)
        Vector2 toTarget = (targetPosition - (Vector2)owner.position).normalized;
        for (int i = 0; i < directionsCount; i++)
        {
            float dot = Vector2.Dot(toTarget, directions[i]);
            interest[i] = Mathf.Clamp01(dot);
        }

        // Calculate danger (obstacle avoidance)
        for (int i = 0; i < directionsCount; i++)
        {
            RaycastHit2D hit = Physics2D.Raycast(owner.position, directions[i], rayLength, obstacleMask);
            if (hit.collider != null && !hit.collider.CompareTag("Player"))
            {
                float distance = hit.distance;
                danger[i] = Mathf.Clamp01(1 - (distance / rayLength));

                // Also affect nearby directions for better avoidance
                int left = (i - 1 + directionsCount) % directionsCount;
                int right = (i + 1) % directionsCount;
                danger[left] = Mathf.Max(danger[left], danger[i] * 0.8f);
                danger[right] = Mathf.Max(danger[right], danger[i] * 0.8f);
            }
        }

        // Create context map by subtracting danger from interest
        for (int i = 0; i < directionsCount; i++)
        {
            contextMap[i] = Mathf.Clamp01(interest[i] - danger[i]);
        }

        // Choose best direction
        Vector2 outputDirection = Vector2.zero;
        for (int i = 0; i < directionsCount; i++)
        {
            outputDirection += directions[i] * contextMap[i];
        }

        return outputDirection.normalized;
    }

    private void ResetArrays()
    {
        for (int i = 0; i < directionsCount; i++)
        {
            interest[i] = 0;
            danger[i] = 0;
            contextMap[i] = 0;
        }
    }


    public void DebugDraw(Transform owner)
    {
        for (int i = 0; i < directionsCount; i++)
        {
            // Interest (green)
            Debug.DrawRay(owner.position, directions[i] * interest[i], Color.green);
            // Danger (red)
            Debug.DrawRay(owner.position, directions[i] * danger[i], Color.red);
            // Context (yellow)
            Debug.DrawRay(owner.position, directions[i] * contextMap[i], Color.yellow);
        }
    }

}


