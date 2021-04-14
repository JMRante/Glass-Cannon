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
    private Stack<RoomNode> dfsStack;

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

        dfsStack = new Stack<RoomNode>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
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
            int roomsPutOnStack = 0;

            foreach (DoorEdge door in currentRoom.GetUnconnectedChildDoors())
            {
                RoomNode newRoom = null;
                RoomNode failedRoom = null;
                HashSet<string> roomsTried = door.GetRoomsTried();

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
                    for (int i = 0; i < roomsPutOnStack; i++)
                    {
                        dfsStack.Pop();
                    }

                    dfsStack.Push(currentRoom.GetParentRoom());
                    currentRoom.GetParentDoor().GetRoomsTried().Add(currentRoom.GetRoomObject().name);
                    currentRoom.DestroyRoom();
                    break;
                }
                else
                {
                    dfsStack.Push(newRoom);
                    roomsPutOnStack++;
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

    private RoomNode ChooseRoom(int depth, HashSet<string> roomsTried)
    {
        GameObject roomPrefab;

        do
        {
            if (depth > 30)
            {
                roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
            }
            else
            {
                // Choose number of doors
                int randRoomNumber = Random.Range(1, 20);

                if (randRoomNumber == 1)
                {
                    roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];
                }
                else if (randRoomNumber >= 2 && randRoomNumber < 15)
                {
                    roomPrefab = room2DoorPrefabs[Random.Range(0, room2DoorPrefabs.Length)];
                }
                else if (randRoomNumber >= 15 && randRoomNumber < 19)
                {
                    roomPrefab = room3DoorPrefabs[Random.Range(0, room3DoorPrefabs.Length)];
                }
                else
                {
                    roomPrefab = room1DoorPrefabs[Random.Range(0, room1DoorPrefabs.Length)];                    
                }
            }
        }
        while (roomsTried.Contains(roomPrefab.name) && roomsTried.Count < roomOptions.Count);

        GameObject room = Instantiate(roomPrefab, new Vector3(0, 0, 0), Quaternion.identity, transform);
        room.name = room.name.Replace("(Clone)", "");
        return new RoomNode(room);
    }
}
