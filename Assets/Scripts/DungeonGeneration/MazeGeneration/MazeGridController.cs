using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGridController : BaseGridController
{
    public MazeCell cell;

    protected override void InitializeGridSize()
    {
        cell = GetComponentInParent<MazeCell>();
        grid.columns = 6;
        grid.rows = 6;
    }

    protected override Vector2 GetParentPosition()
    {
        return cell.transform.localPosition;
    }

    protected override void OnGridGenerated()
    {
        GetComponentInParent<ObjectRoomSpawner>().InitializedObjectSpawning();
    }
}
