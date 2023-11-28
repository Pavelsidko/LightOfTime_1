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
            if (item.name == "Coin")
            {
                PlayerMovement.collectedCoinsAmount++;
                GameController.CoinAmountChange(coinValue);
            }
            GameController.HealPlayer(healthChange);
            GameController.MoveSpeedChange(moveSpeedChange);
            GameController.FireRateChange(attackSpeedChange);
            GameController.BulletSizeChange(bulletSizeChange);
            GameController.instance.UpdateCollectedItems(this);
            Destroy(gameObject);
        }
    }


    
}
