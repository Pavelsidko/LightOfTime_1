using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    public static bool start = false;
    public AnimationCurve shakerCurve;
    public static float duration = .3f;

    void Update()
    {
        if (start)
        {
            start = false;
            StartCoroutine(CameraShake());
        }
        
    }

    IEnumerator CameraShake()
    {
        Vector3 startPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float strength = shakerCurve.Evaluate(elapsedTime / duration);
            transform.position = startPosition + UnityEngine.Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = startPosition;
    }
}
