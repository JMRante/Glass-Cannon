using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomDoor
{
    private GameObject door;
    private RoomDoor connectedDoor;
    private RoomNode parentRoom;

    public RoomDoor(GameObject door, RoomNode parentRoom)
    {
        this.door = door;
        this.connectedDoor = null;
        this.parentRoom = parentRoom;
    }

    public void ConnectDoor(RoomDoor connectedDoor)
    {
        if (this.connectedDoor == null)
        {
            this.connectedDoor = connectedDoor;
            connectedDoor.ConnectDoor(this);
        }
    }

    public RoomNode GetParentRoom()
    {
        return parentRoom;
    }

    public GameObject GetDoor()
    {
        return door;
    }

    public RoomDoor GetConnectedDoor()
    {
        return connectedDoor;
    }
}
