using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject hertContainer;
    private float fillValue;


    // Update is called once per frame
    void Update()
    {
        fillValue = (float)GameController.Health;
        fillValue = fillValue / GameController.MaxHealth;
        hertContainer.GetComponent<Image>().fillAmount= fillValue;
        
    }
}
