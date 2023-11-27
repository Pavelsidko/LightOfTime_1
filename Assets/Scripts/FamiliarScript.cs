using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class FamiliarScript : MonoBehaviour
{
    private float lastFire;
    private GameObject player;
    public FamiliarData familiar;
    private float lastOffSetX;
    private float lastOffSetY;
    public Joystick joystick;
    //public Joystick shootJoystick;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }


    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

        //float shootVert = shootJoystick.Vertical;
        //float shootHor = shootJoystick.Horizontal;

        //if((shootHor != 0 || shootVert != 0) && Time.time > lastFire + familiar.fireDelay)
        //{
        //    Shoot(shootHor, shootVert);
        //    lastFire= Time.time;
        //}

        if(horizontal != 0 || vertical != 0)
        {
            float offsetX = (horizontal < 0) ? Mathf.Floor(horizontal) : Mathf.Ceil(horizontal);
            float offsetY = (vertical < 0) ? Mathf.Floor(vertical) : Mathf.Ceil(vertical);

            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, familiar.speed * Time.deltaTime);
            lastOffSetX = offsetX; 
            lastOffSetY = offsetY;
        }
        else
        {
            if(!(transform.position.x < lastOffSetX + 0.5f) || !(transform.position.y < lastOffSetY + 0.5f))
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(player.transform.position.x - lastOffSetX, player.transform.position.y - lastOffSetY), familiar.speed * Time.deltaTime);
            }
        }


    }

    //void Shoot(float x , float y)
    //{
    //    GameObject bullet = Instantiate(familiar.bulletPrefab, transform.position, Quaternion.identity) as GameObject;

    //    float posX = (x < 0) ? Mathf.Floor(x) * familiar.speed : Mathf.Ceil(x) * familiar.speed;
    //    float posY = (y < 0) ? Mathf.Floor(y) * familiar.speed : Mathf.Ceil(y) * familiar.speed;
    //    bullet.AddComponent<Rigidbody2D>().gravityScale = 0;
    //    bullet.GetComponent<Rigidbody>().velocity = new Vector2(posX, posY);

    //}



}
