using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRoomSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct RandomSpawner
    {
        public string name;
        public SpawnerData spawnerData;
    }

    public BaseGridController grid;

    public RandomSpawner[] spawnerData;

    public void InitializedObjectSpawning()
    {
        if (grid == null)
        {
            Debug.LogWarning("Grid not assigned in ObjectRoomSpawner!");
            return;
        }

        foreach (RandomSpawner rs in spawnerData)
        {
            SpawnObjects(rs);
        }
    }

    void SpawnObjects(RandomSpawner data)
    {
        int randomIteration = Random.Range(data.spawnerData.minSpawn, data.spawnerData.maxSpawn + 1);

        for (int i = 0; i < randomIteration; i++)
        {
            if (grid.availablePoints.Count == 0) break;

            int randomPos = Random.Range(0, grid.availablePoints.Count);
            GameObject go = Instantiate(data.spawnerData.itemToSpawn, grid.availablePoints[randomPos], Quaternion.identity, transform);
            grid.availablePoints.RemoveAt(randomPos);

            Debug.Log("Spawned Object!");
        }
    }
}
