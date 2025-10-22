using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


[System.Serializable]
public class Item
{
    public string name;
    public string description;
    public Sprite itemImage;
}


public class Collection : MonoBehaviour
{

    public Item item;
    public float healthChange;
    public float moveSpeedChange;
    public float attackSpeedChange;
    public float bulletSizeChange;
    public int coinValue;


    private void Start()
    {
        GetComponent<SpriteRenderer>().sprite = item.itemImage;
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (coinValue != 0) CoinManager.instance.ChangeScore(coinValue);
            if (healthChange != 0) GameController.HealPlayer(healthChange);
            if (moveSpeedChange != 0) GameController.MoveSpeedChange(moveSpeedChange);
            if (attackSpeedChange != 0) GameController.FireRateChange(attackSpeedChange);
            if (bulletSizeChange != 0) GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
        }
    } 
}
