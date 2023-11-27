using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class IteamSpawner : MonoBehaviour
{

    [System.Serializable]
    public struct Spawnable
    {
        public GameObject gameObject;
        public float weight;
    }

    public List<Spawnable> items = new List<Spawnable>();
    float totalWeight;

    private void Awake()
    {
        totalWeight= 0f;
        foreach(var spawnable in items)
        {
            totalWeight += spawnable.weight;
        }
    }
    void Start()
    {
        float pick = Random.value * totalWeight;
        int chosenIndex = 0;
        float cumulativeWeight = items[0].weight;



        while(pick > cumulativeWeight && chosenIndex < items.Count - 1) 
        {
            chosenIndex++;
            cumulativeWeight += items[chosenIndex].weight;
        }
        GameObject i = Instantiate(items[chosenIndex].gameObject, transform.position, Quaternion.identity, transform) as GameObject;


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
