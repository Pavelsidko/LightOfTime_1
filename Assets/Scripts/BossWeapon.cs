using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.Mathematics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class BossWeapon : MonoBehaviour
{
    public float offset;
    private float time;
    public float fireDelay;

    public GameObject bullet;
    public Transform pointUp;
  

    private PlayerMovement player;

   
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }


    void Update()
    {
        
        if (time <= 0f)
        {
            Instantiate(bullet, pointUp.position, transform.rotation);
            bullet.GetComponent<Bullet>().isEnemyBullet = true;
            time = fireDelay;
        }
        else
        {
            time -= Time.deltaTime;
        }
    }






}
