using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    private List<RoomNode> connectedRooms;
    private List<RoomDoor> doors;
    private GameObject room;

    public RoomNode(GameObject room)
    {
        this.room = room;

        connectedRooms = new List<RoomNode>();
        GetPrefabDoors();
    }

    public void ConnectRoom(RoomNode roomNode)
    {
        if (connectedRooms.Count < doors.Count)
        {
            RoomDoor myConnectingDoor = GetRandomUnconnectedDoor();
            RoomDoor theirConnectingDoor = roomNode.getRandomDoor();

            AdjustRoomToConnectDoors(myConnectingDoor, theirConnectingDoor);

            myConnectingDoor.ConnectDoor(theirConnectingDoor);

            connectedRooms.Add(roomNode);
            roomNode.connectedRooms.Add(this);
        }
    }

    private void AdjustRoomToConnectDoors(RoomDoor steadyDoor, RoomDoor transformingDoor)
    {
        Transform steadyRoomTransform = steadyDoor.GetParentRoom().GetRoom().transform;
        Transform transformingRoomTransform = transformingDoor.GetParentRoom().GetRoom().transform;

        // Get rotation from next door to previous door inverted and apply rotation to entire room.
        float steadyDoorZRotation = steadyDoor.GetDoor().transform.rotation.eulerAngles.y;
        float transformingDoorZRotation = transformingDoor.GetDoor().transform.eulerAngles.y;
        float rotation = Mathf.DeltaAngle(transformingDoorZRotation, steadyDoorZRotation + 180);

        transformingRoomTransform.rotation = Quaternion.Euler(0f, rotation, 0f);

        // Move transform of room to previous door transform.
        transformingRoomTransform.position = steadyDoor.GetDoor().transform.position;

        // Subtract new door position from room transform.
        Vector3 transformingRoomDoorTranslation = transformingDoor.GetDoor().transform.position - transformingRoomTransform.position;

        // Translate room by subtraction vector.
        transformingRoomTransform.position -= transformingRoomDoorTranslation;
    }

    public GameObject GetRoom()
    {
        return room;
    }

    private void GetPrefabDoors()
    {
        doors = new List<RoomDoor>();

        foreach (Transform child in room.transform)
        {
            if (child.gameObject.tag.Equals("DoorNode"))
            {
                doors.Add(new RoomDoor(child.gameObject, this));
            }
        }
    }

    public List<RoomDoor> GetDoors()
    {
        return doors;
    }

    public RoomDoor getRandomDoor()
    {
        return doors[Random.Range(0, doors.Count)];
    }

    public RoomDoor GetRandomUnconnectedDoor()
    {
        List<RoomDoor> unconnectedDoors = doors.FindAll(x => x.GetConnectedDoor() == null);
        return unconnectedDoors[Random.Range(0, unconnectedDoors.Count)];
    }

    public List<RoomDoor> GetUnconnectedDoors()
    {
        return doors.FindAll(x => x.GetConnectedDoor() == null);
    }

    public int GetDoorCount()
    {
        return doors.Count;
    }
}
