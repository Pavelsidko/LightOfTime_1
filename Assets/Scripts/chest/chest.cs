using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chest : MonoBehaviour
{
    public int costToOpen = 5;
    private void OnMouseDown()
    {
        if(CoinManager.instance.CanBuyChest(costToOpen))
        {
            GetComponent<ChestLootBag>().InstantiateLoot(transform.position);
            Debug.Log("irtnjgweirotgnweo");
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Not money");
        }
    }
}
