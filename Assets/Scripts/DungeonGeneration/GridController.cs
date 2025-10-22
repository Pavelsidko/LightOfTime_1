using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridController : BaseGridController
{
    public Room room;

    protected override void InitializeGridSize()
    {
        room = GetComponentInParent<Room>();
        grid.columns = room.Width - 2;
        grid.rows = room.Height - 2;
    }

    protected override Vector2 GetParentPosition()
    {
        return room.transform.localPosition;
    }

    protected override void OnGridGenerated()
    {
        GetComponentInParent<ObjectRoomSpawner>().InitializedObjectSpawning();
    }
}