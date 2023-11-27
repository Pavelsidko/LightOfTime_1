using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DungeonCrawler : MonoBehaviour
{
    public Vector2Int Position { get; set; }
    public DungeonCrawler(Vector2Int startPos)
    {
        Position = startPos;
    }

    public Vector2Int Move(Dictionary<Direction, Vector2Int> directionMoventMap)
    {
        Direction toMove = (Direction)Random.Range(0, directionMoventMap.Count);
        Position += directionMoventMap[toMove];
        return Position;
    }



}
