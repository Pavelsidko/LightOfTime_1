using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Room currRoom;
    public float moveSpeedWhenRoomChange;


    private void Awake()
    {
        instance= this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePosition();
    }


    void UpdatePosition()
    {
        if(currRoom == null)
        {
            return;
        }
        Vector3 targetPos = GetCameraTargerPosition();

        transform.position = Vector3.MoveTowards(transform.position, targetPos, Time.deltaTime * moveSpeedWhenRoomChange);
    }

    Vector3 GetCameraTargerPosition()
    {
        if(currRoom == null)
        {
            return Vector3.zero;
        }

        Vector3 targetPos = currRoom.GetRoomCentre();
        targetPos.z = transform.position.z;

        return targetPos;
    }


    public bool IsSwitchingScene()
    {
        return transform.position.Equals(GetCameraTargerPosition()) == false;
    }

}
