using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    [SerializeField] private MazeCell mazeCell;
    [SerializeField] private MazeCell startMazeCell;
    [SerializeField] private MazeCell teleportMazeCell;
    [SerializeField] private int mazeWidth;
    [SerializeField] private int mazeHeight;
    [SerializeField] private int extraWallsToRemove = 5;
    [SerializeField] private int minDistanceFromStart = 5;

    private const float CellWidth = 6f;
    private const float CellHeight = 6f;
    private Vector2Int teleportPosition;
    private MazeCell[,] mazeGrid;
    private List<MazeCell> activeCells = new List<MazeCell>();

    void Start()
    {
        GenerateMazeImmediately();
    }

    public void GenerateMazeImmediately()
    {
        mazeGrid = new MazeCell[mazeWidth, mazeHeight];
        teleportPosition = GetRandomTeleportPosition();

        // —оздаем все €чейки
        for (int x = 0; x < mazeWidth; x++)
        {
            for (int y = 0; y < mazeHeight; y++)
            {
                mazeGrid[x, y] = (x == teleportPosition.x && y == teleportPosition.y)
                    ? CreateTeleportCell(x, y)
                    : CreateMazeCell(x, y);
            }
        }

        // ¬ыбираем стартовую €чейку
        MazeCell startCell = mazeGrid[Random.Range(0, mazeWidth), Random.Range(0, mazeHeight)];
        activeCells.Add(startCell);
        startCell.Visit();

        // √енерируем лабиринт
        GenerateMazeWithRandomizedPrim();

        // ”дал€ем лишние стены
        RemoveExtraWalls();
    }

    private MazeCell CreateMazeCell(int x, int y)
    {
        Vector2 position = new Vector2(x * CellWidth, y * CellHeight);

        MazeCell prefab = (x == 0 && y == 0) ? startMazeCell : mazeCell;
        MazeCell cell = Instantiate(prefab, position, Quaternion.identity);
        cell.name = (x == 0 && y == 0) ? "StartCell" : $"Cell_{x}_{y}";
        return cell;
    }

    private MazeCell CreateTeleportCell(int x, int y)
    {
        Vector2 position = new Vector2(x * CellWidth, y * CellHeight);
        MazeCell cell = Instantiate(teleportMazeCell, position, Quaternion.identity);
        cell.name = $"Teleport Cell_{x}_{y}";
        return cell;
    }

    private void GenerateMazeWithRandomizedPrim()
    {
        while (activeCells.Count > 0)
        {
            int randomIndex = Random.Range(0, activeCells.Count);
            MazeCell currentCell = activeCells[randomIndex];

            int x = Mathf.RoundToInt(currentCell.transform.position.x / CellWidth);
            int y = Mathf.RoundToInt(currentCell.transform.position.y / CellHeight);

            List<MazeCell> unvisitedNeighbors = GetUnvisitedNeighbors(x, y).ToList();

            if (unvisitedNeighbors.Count > 0)
            {
                MazeCell chosenNeighbor = unvisitedNeighbors[Random.Range(0, unvisitedNeighbors.Count)];

                int nx = Mathf.RoundToInt(chosenNeighbor.transform.position.x / CellWidth);
                int ny = Mathf.RoundToInt(chosenNeighbor.transform.position.y / CellHeight);

                ClearWallsBetween(currentCell, chosenNeighbor);

                chosenNeighbor.Visit();
                activeCells.Add(chosenNeighbor);
            }
            else
            {
                activeCells.RemoveAt(randomIndex);
            }
        }
    }

    private IEnumerable<MazeCell> GetUnvisitedNeighbors(int x, int y)
    {
        if (x > 0 && !mazeGrid[x - 1, y].IsVisited) yield return mazeGrid[x - 1, y];
        if (x < mazeWidth - 1 && !mazeGrid[x + 1, y].IsVisited) yield return mazeGrid[x + 1, y];
        if (y > 0 && !mazeGrid[x, y - 1].IsVisited) yield return mazeGrid[x, y - 1];
        if (y < mazeHeight - 1 && !mazeGrid[x, y + 1].IsVisited) yield return mazeGrid[x, y + 1];
    }

    private void RemoveExtraWalls()
    {
        for (int i = 0; i < extraWallsToRemove; i++)
        {
            int x = Random.Range(0, mazeWidth);
            int y = Random.Range(0, mazeHeight);
            MazeCell cell = mazeGrid[x, y];

            int direction = Random.Range(0, 4);
            int nx = x, ny = y;

            switch (direction)
            {
                case 0: nx = x + 1; break; // right
                case 1: nx = x - 1; break; // left
                case 2: ny = y + 1; break; // up
                case 3: ny = y - 1; break; // down
            }

            if (nx >= 0 && nx < mazeWidth && ny >= 0 && ny < mazeHeight)
            {
                MazeCell neighbor = mazeGrid[nx, ny];
                ClearWallsBetween(cell, neighbor);
            }
            else
            {
                i--;
            }
        }
    }

    private void ClearWallsBetween(MazeCell a, MazeCell b)
    {
        int ax = Mathf.RoundToInt(a.transform.position.x / CellWidth);
        int ay = Mathf.RoundToInt(a.transform.position.y / CellHeight);
        int bx = Mathf.RoundToInt(b.transform.position.x / CellWidth);
        int by = Mathf.RoundToInt(b.transform.position.y / CellHeight);

        if (ax < bx) // b right to a
        {
            a.ClearRightWall();
            b.ClearLeftWall();
        }
        else if (ax > bx) // b left to a
        {
            a.ClearLeftWall();
            b.ClearRightWall();
        }
        else if (ay < by) // b up to a
        {
            a.ClearTopWall();
            b.ClearBottomWall();
        }
        else if (ay > by) // b down to a
        {
            a.ClearBottomWall();
            b.ClearTopWall();
        }
    }

    private Vector2Int GetRandomTeleportPosition()
    {
        int x, y;
        int attempts = 0;
        const int maxAttempts = 100;

        do
        {
            x = Random.Range(0, mazeWidth);
            y = Random.Range(0, mazeHeight);
            attempts++;

            if (attempts > maxAttempts) break;
        }
        while ((x == 0 && y == 0) ||
                 Vector2Int.Distance(new Vector2Int(x, y), Vector2Int.zero) < minDistanceFromStart);

        return new Vector2Int(x, y);
    }
}
