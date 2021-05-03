using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorEdge
{
    private GameObject parentDoorObject;
    private RoomNode parentRoom;

    private GameObject childDoorObject;
    private RoomNode childRoom;

    private HashSet<RoomPrefab> roomsTried;

    public DoorEdge(GameObject parentDoorObject, RoomNode parentRoom)
    {
        this.parentDoorObject = parentDoorObject;
        this.parentRoom = parentRoom;

        this.childDoorObject = null;
        this.childRoom = null;

        roomsTried = new HashSet<RoomPrefab>();
    }

    public void ConnectChildRoom(GameObject childDoorObject, RoomNode childRoom)
    {
        this.childDoorObject = childDoorObject;
        this.childRoom = childRoom;
    }

    public GameObject GetParentDoorObject()
    {
        return parentDoorObject;
    }

    public RoomNode GetParentRoom()
    {
        return parentRoom;
    }

    public GameObject GetChildDoorObject()
    {
        return childDoorObject;
    }

    public RoomNode GetChildRoom()
    {
        return childRoom;
    }

    public bool HasParentRoom()
    {
        return parentRoom != null;
    }

    public bool HasChildRoom()
    {
        return childRoom != null;
    }

    public void DisconnectChildRoom()
    {
        if (this.childRoom != null)
        {
            this.childRoom.GetRoomObject().transform.position = Vector3.zero;
            this.childRoom.GetRoomObject().transform.rotation = Quaternion.identity;
        }

        this.childDoorObject = null;
        this.childRoom = null;
    }

    public bool IsDoorValid()
    {
        return parentDoorObject != null && parentRoom != null;
    }

    public HashSet<RoomPrefab> GetRoomsTried()
    {
        return roomsTried;
    }
}
