using UnityEngine;


[CreateAssetMenu(fileName = "DungeonGenerationData.asset", menuName = "DangeonGenerationData/Dungeon Data")]




public class DungeonGenerationData : ScriptableObject
{
    public int numberOfCrawlers;
    public int iterationMin;
    public int iterationMax;
 
}
