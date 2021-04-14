using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private GameObject[] roomPrefabs;
    private GameObject[] room1DoorPrefabs;
    private GameObject[] room2DoorPrefabs;
    private GameObject[] room3DoorPrefabs;
    private HashSet<string> roomOptions;

    private RoomNode rootRoom;

    void Start()
    {
        room1DoorPrefabs = Resources.LoadAll<GameObject>("1_door");
        room2DoorPrefabs = Resources.LoadAll<GameObject>("2_door");
        room3DoorPrefabs = Resources.LoadAll<GameObject>("3_door");
        
        roomOptions = new HashSet<string>();

        foreach (GameObject roomPrefab in room1DoorPrefabs)
            roomOptions.Add(roomPrefab.name);

        foreach (GameObject roomPrefab in room2DoorPrefabs)
            roomOptions.Add(roomPrefab.name);

        foreach (GameObject roomPrefab in room3DoorPrefabs)
            roomOptions.Add(roomPrefab.name);     

        GenerateStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClearRooms();
            GenerateStage();
        }
    }

    private void GenerateStage()
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();

        // Place a starting room (any qualifying end room) at the center of the scene.
        GameObject rootRoomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
        GameObject rootRoomInstance = Instantiate(rootRoomPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        rootRoomInstance.name = rootRoomInstance.name.Replace("(Clone)", "");
        rootRoom = new RoomNode(rootRoomInstance);

        dfsStack.Push(rootRoom);

        while (dfsStack.Count > 0)
        {
            RoomNode currentRoom = dfsStack.Pop();

            // Go through each door of the parent node and create a room for it.
            foreach (DoorEdge door in currentRoom.GetUnconnectedChildDoors())
            {
                RoomNode newRoom = null;
                RoomNode failedRoom = null;
                HashSet<string> roomsTried = new HashSet<string>();

                while (roomsTried.Count < roomOptions.Count)
                {
                    newRoom = ChooseRoom(dfsStack.Count, roomsTried);
                    failedRoom = currentRoom.ConnectRoom(door, newRoom);

                    // Unable to fit new room by any of its doors
                    if (failedRoom != null)
                    {
                        roomsTried.Add(failedRoom.GetRoomObject().name);
                        failedRoom.DestroyRoom();
                    }
                    else
                    {
                        break;
                    }
                }

                // If no room will work for this door, we need to choose a new room higher up the stack
                if (failedRoom != null)
                {
                    dfsStack.Push(currentRoom.GetParentRoom());
                    currentRoom.DestroyRoom();
                    break;
                }
                else
                {
                    dfsStack.Push(newRoom);
                }
            }
        }
    }

    private void ClearRooms()
    {
        foreach (Transform childRoom in gameObject.GetComponentInChildren<Transform>())
        {
            childRoom.gameObject.SetActive(false);
            Object.Destroy(childRoom.gameObject);
        }
    }

    // private RoomNode CreateChildRooms(RoomNode parentNode, int depth)
    // {
    //     // Go through each door of the parent node and create a room for it.
    //     foreach (DoorEdge door in parentNode.GetUnconnectedDoors())
    //     {
    //         RoomNode newRoom;
    //         RoomNode failedRoom;
    //         HashSet<string> roomsTried = new HashSet<string>();

    //         // Try different rooms until one fits
    //         do
    //         {
    //             newRoom = ChooseRoom(depth, roomsTried);
    //             failedRoom = parentNode.ConnectRoom(newRoom);

    //             if (failedRoom != null)
    //             {
    //                 roomsTried.Add(failedRoom.GetRoom().name);
    //                 failedRoom.GetRoom().SetActive(false);
    //                 Object.Destroy(failedRoom.GetRoom().gameObject);

    //                 if (roomsTried.SetEquals(roomOptions))
    //                 {
    //                     return parentNode;
    //                 }
    //             }
    //             else
    //             {
    //                 // If no child room fits, this is a failed room
    //                 failedRoom = CreateChildRooms(newRoom, depth + 1);

    //                 if (failedRoom != null)
    //                 {
    //                     roomsTried.Add(failedRoom.GetRoom().name);
    //                     failedRoom.GetRoom().SetActive(false);
    //                     Object.Destroy(failedRoom.GetRoom().gameObject);

    //                     if (roomsTried.SetEquals(roomOptions))
    //                     {
    //                         return parentNode;
    //                     }
    //                 }
    //             }
    //         }
    //         while (failedRoom != null);
    //     }

    //     return null;
    // }

    private RoomNode ChooseRoom(int depth, HashSet<string> roomsTried)
    {
        GameObject roomPrefab;

        do
        {
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
        }
        while (roomsTried.Contains(roomPrefab.name) && roomsTried.Count < roomOptions.Count);

        GameObject room = Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        room.name = room.name.Replace("(Clone)", "");
        return new RoomNode(room);
    }
}
