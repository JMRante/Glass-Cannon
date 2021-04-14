using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomNode
{
    private GameObject roomObject;
    private RoomNode parentRoom;
    private DoorEdge parentDoor;
    private List<DoorEdge> childDoors;

    public RoomNode(GameObject roomObject)
    {
        this.roomObject = roomObject;
        this.parentRoom = null;

        childDoors = new List<DoorEdge>();
        GetPrefabDoors();
    }

    public RoomNode ConnectRoom(DoorEdge door, RoomNode roomNode)
    {
        List<DoorEdge> theirConnectingDoorOptions = roomNode.childDoors;

        // Try each door on the room to connect to see if it will fit
        foreach (DoorEdge doorOption in theirConnectingDoorOptions)
        {
            // Connect their child room to our door
            door.ConnectChildRoom(doorOption.GetParentDoorObject(), roomNode);

            AdjustRoomToConnectDoors(door);

            // If adjusted room does not fit, reject it, undo its promotion, and try another
            if (!DoesRoomFit(roomNode))
            {
                door.DisconnectChildRoom();
                continue;
            }
            else
            {
                // Promote child door of ours to parent door of theirs
                roomNode.parentRoom = this;
                roomNode.parentDoor = door;
                roomNode.childDoors.Remove(doorOption);

                return null;
            }
        }

        return roomNode;
    }

    private void AdjustRoomToConnectDoors(DoorEdge door)
    {
        Transform steadyRoomTransform = door.GetParentRoom().GetRoomObject().transform;
        Transform transformingRoomTransform = door.GetChildRoom().GetRoomObject().transform;

        // Get rotation from next door to previous door inverted and apply rotation to entire room.
        float steadyDoorZRotation = door.GetParentDoorObject().transform.rotation.eulerAngles.y;
        float transformingDoorZRotation = door.GetChildDoorObject().transform.eulerAngles.y;
        float rotation = Mathf.DeltaAngle(transformingDoorZRotation, steadyDoorZRotation + 180);

        transformingRoomTransform.rotation = Quaternion.Euler(0f, rotation, 0f);

        // Move transform of room to previous door transform.
        transformingRoomTransform.position = door.GetParentDoorObject().transform.position;

        // Subtract new door position from room transform.
        Vector3 transformingRoomDoorTranslation = door.GetChildDoorObject().transform.position - transformingRoomTransform.position;

        // Translate room by subtraction vector.
        transformingRoomTransform.position -= transformingRoomDoorTranslation;
    }

    public bool DoesRoomFit(RoomNode roomNode)
    {
        bool doesRoomFit = true;

        BoxCollider[] newColliders = roomNode.GetRoomObject().GetComponentsInChildren<BoxCollider>();

        foreach (BoxCollider newCollider in newColliders)
        {
            newCollider.gameObject.SetActive(false);
        }

        foreach (BoxCollider newCollider in newColliders)
        {
            Collider[] existingCollidersOverlapping = Physics.OverlapBox(newCollider.transform.position + newCollider.center, newCollider.size / 2, newCollider.transform.rotation, LayerMask.GetMask("RoomArea"), QueryTriggerInteraction.Collide);

            if (existingCollidersOverlapping.Length > 0)
            {
                doesRoomFit = false;
                break;
            }
        }

        foreach (BoxCollider newCollider in newColliders)
        {
            newCollider.gameObject.SetActive(true);
        }

        return doesRoomFit;
    }

    public GameObject GetRoomObject()
    {
        return roomObject;
    }

    private void GetPrefabDoors()
    {
        childDoors = new List<DoorEdge>();

        foreach (Transform child in roomObject.transform)
        {
            if (child.gameObject.tag.Equals("DoorNode"))
            {
                childDoors.Add(new DoorEdge(child.gameObject, this));
            }
        }
    }

    public List<DoorEdge> GetChildDoors()
    {
        return childDoors;
    }

    public DoorEdge GetRandomChildDoor()
    {
        return childDoors[Random.Range(0, childDoors.Count)];
    }

    public DoorEdge GetRandomUnconnectedChildDoor()
    {
        List<DoorEdge> unconnectedDoors = childDoors.FindAll(x => !x.HasChildRoom());
        return unconnectedDoors[Random.Range(0, unconnectedDoors.Count)];
    }

    public List<DoorEdge> GetUnconnectedChildDoors()
    {
        return childDoors.FindAll(x => !x.HasChildRoom());
    }

    public RoomNode GetParentRoom()
    {
        return parentRoom;
    }

    public DoorEdge GetParentDoor()
    {
        return parentDoor;
    }

    public int GetDoorCount()
    {
        return childDoors.Count + (parentDoor != null ? 1 : 0);
    }

    public void DestroyRoom()
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();
        dfsStack.Push(this);

        while (dfsStack.Count > 0)
        {
            RoomNode currentRoom = dfsStack.Pop();

            foreach (DoorEdge door in currentRoom.childDoors)
            {
                if (door.HasChildRoom())
                {
                    dfsStack.Push(door.GetChildRoom());
                }
            }

            if (currentRoom.parentDoor != null)
            {
                currentRoom.parentDoor.DisconnectChildRoom();
            }

            currentRoom.roomObject.SetActive(false);
            Object.Destroy(currentRoom.roomObject);
        }
    }
}
