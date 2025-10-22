using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;  // Объект, за которым следует камера (игрок)
    public float smoothSpeed = 5f;  // Плавность перемещения
    public Vector3 offset = new Vector3(0, 0, -10);  // Смещение камеры (по Z - расстояние от 2D плоскости)

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
