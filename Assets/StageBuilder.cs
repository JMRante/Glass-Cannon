using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private GameObject[] roomPrefabs;
    private GameObject[] room1DoorPrefabs;
    private GameObject[] room2DoorPrefabs;
    private GameObject[] room3DoorPrefabs;

    private RoomNode parentRoom;

    void Start()
    {
        room1DoorPrefabs = Resources.LoadAll<GameObject>("1_door");
        room2DoorPrefabs = Resources.LoadAll<GameObject>("2_door");
        room3DoorPrefabs = Resources.LoadAll<GameObject>("3_door");

        // Place a starting room (any qualifying end room) at the center of the scene.
        GameObject startRoomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
        GameObject startRoom = Instantiate(startRoomPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        parentRoom = new RoomNode(startRoom);

        CreateChildRooms(parentRoom, 0);
    }

    private void CreateChildRooms(RoomNode parentNode, int depth)
    {
        // Go through each door of the parent node and create a room for it.
        foreach (RoomDoor door in parentNode.GetUnconnectedDoors())
        {
            RoomNode newRoom = ChooseRoom(depth);
            parentNode.ConnectRoom(newRoom);
            
            CreateChildRooms(newRoom, depth + 1);
        }
    }

    private RoomNode ChooseRoom(int depth)
    {
        GameObject roomPrefab;

        if (depth > 10)
        {
            roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
        }
        else
        {
            // Choose number of doors
            switch (Random.Range(1, 4))
            {
                // Choose room
                case 1:
                    roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
                    break;
                case 2:
                    roomPrefab = room2DoorPrefabs[Random.Range(0, room2DoorPrefabs.Length)];
                    break;
                case 3:
                    roomPrefab = room3DoorPrefabs[Random.Range(0, room3DoorPrefabs.Length)];
                    break;
                default:
                    roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
                    break;
            }
        }

        GameObject room = Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        return new RoomNode(room);
    }
}
