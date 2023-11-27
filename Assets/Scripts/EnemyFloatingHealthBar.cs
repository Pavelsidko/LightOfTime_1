using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyFloatingHealthBar : MonoBehaviour
{
    [SerializeField] private Slider sliderhealthSlider;
    //[SerializeField] private Camera camera;
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset;

    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        sliderhealthSlider.value = currentValue / maxValue; 
    }
    void Update()
    {
        //transform.rotation = camera.transform.rotation;
        transform.position = target.position + offset;
    }
}
