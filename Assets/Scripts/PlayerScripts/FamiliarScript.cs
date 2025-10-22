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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        float horizontal = joystick.Horizontal;
        float vertical = joystick.Vertical;

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
}
