using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseGridController : MonoBehaviour
{
    [System.Serializable]
    public struct GridData
    {
        public int columns, rows;
        public float verticalOffset, horizontalOffset;
    }

    public GridData grid;
    public GameObject gridTile;
    public List<Vector2> availablePoints = new List<Vector2>();

    protected abstract void InitializeGridSize();
    protected abstract Vector2 GetParentPosition();

    private void Awake()
    {
        InitializeGridSize();
        GenerateGrid();
    }

    public void GenerateGrid()
    {
        Vector2 parentPosition = GetParentPosition();
        grid.verticalOffset += parentPosition.y;
        grid.horizontalOffset += parentPosition.x;

        for (int y = 0; y < grid.rows - 1; y++)
        {
            for (int x = 0; x < grid.columns - 1; x++)
            {
                GameObject go = Instantiate(gridTile, transform);
                go.transform.position = new Vector2(
                    x - (grid.columns - grid.horizontalOffset),
                    y - (grid.rows - grid.verticalOffset));
                go.name = $"X: {x}, Y: {y}";
                availablePoints.Add(go.transform.position);
                go.SetActive(false);
            }
        }

        OnGridGenerated();
    }

    protected virtual void OnGridGenerated() { }
}
