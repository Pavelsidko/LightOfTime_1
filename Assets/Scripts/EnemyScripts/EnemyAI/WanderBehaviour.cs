using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : SteeringBehaviour
{
    [SerializeField]
    private float directionChangeInterval = 1.5f;

    [SerializeField]
    private float wanderStrength = 0.5f;

    [SerializeField]
    private float dangerThreshold = 0.3f;

    private Vector2 currentWanderDirection;
    private float timeSinceLastDirectionChange = 0f;

    private void Start()
    {
        PickNewDirection();
    }

    private void Update()
    {
        timeSinceLastDirectionChange += Time.deltaTime;
        if (timeSinceLastDirectionChange >= directionChangeInterval)
        {
            PickNewDirection();
            timeSinceLastDirectionChange = 0f;
        }
    }

    private void PickNewDirection()
    {
        currentWanderDirection = Directions.eightDirections[Random.Range(0, Directions.eightDirections.Count)];
    }

    public override (float[] danger, float[] interest) GetSteering(float[] danger, float[] interest, AIData aiData)
    {
        int bestDirectionIndex = -1;
        float bestScore = float.MinValue;

        // Ќаходим индекс текущего направлени€
        for (int i = 0; i < Directions.eightDirections.Count; i++)
        {
            float dot = Vector2.Dot(currentWanderDirection.normalized, Directions.eightDirections[i]);
            if (dot > bestScore)
            {
                bestScore = dot;
                bestDirectionIndex = i;
            }
        }

        // ѕроверка: если текущее направление слишком опасное Ч сменить его
        if (bestDirectionIndex != -1 && danger[bestDirectionIndex] > dangerThreshold)
        {
            List<int> safeDirections = new List<int>();
            for (int i = 0; i < danger.Length; i++)
            {
                if (danger[i] < dangerThreshold)
                {
                    safeDirections.Add(i);
                }
            }

            if (safeDirections.Count > 0)
            {
                int randomSafeIndex = safeDirections[Random.Range(0, safeDirections.Count)];
                currentWanderDirection = Directions.eightDirections[randomSafeIndex];
            }
            else
            {
                // Ќет безопасных направлений Ч оставл€ем текущее
            }
        }

        // ƒобавл€ем интерес к текущему wander-направлению
        for (int i = 0; i < interest.Length; i++)
        {
            float result = Vector2.Dot(currentWanderDirection.normalized, Directions.eightDirections[i]);
            if (result > 0)
            {
                float valueToPutIn = result * wanderStrength;
                if (valueToPutIn > interest[i])
                {
                    interest[i] = valueToPutIn;
                }
            }
        }

        return (danger, interest);
    }
}

