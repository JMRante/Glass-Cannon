using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private HashSet<RoomPrefab> roomOptions;

    private RoomNode rootRoom;
    private Stack<RoomNode> dfsStack;

    void Start()
    {
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>("rooms");

        roomOptions = new HashSet<RoomPrefab>();

        foreach (GameObject roomPrefab in roomPrefabs)
            roomOptions.Add(new RoomPrefab(roomPrefab));

        GenerateStage();

        dfsStack = new Stack<RoomNode>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ClearRooms();
            GenerateStage();
        }
    }

    private void GenerateStage()
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();

        // Place a starting room (any qualifying end room) at the center of the scene.
        RoomPrefab rootRoomChoice = ChooseRoomByDoorCount(new HashSet<RoomPrefab>(), 1);
        rootRoom = CreateRoom(rootRoomChoice);

        dfsStack.Push(rootRoom);

        while (dfsStack.Count > 0)
        {
            RoomNode currentRoom = dfsStack.Pop();

            // Go through each door of the parent node and create a room for it.
            int roomsPutOnStack = 0;

            foreach (DoorEdge door in currentRoom.GetUnconnectedChildDoors())
            {
                RoomPrefab newRoomPrefab = null;
                RoomNode newRoom = null;
                RoomNode failedRoom = null;
                HashSet<RoomPrefab> roomsTried = door.GetRoomsTried();

                while (roomsTried.Count < roomOptions.Count)
                {
                    newRoomPrefab = ChooseRoom(dfsStack.Count, roomsTried);
                    newRoom = CreateRoom(newRoomPrefab);
                    failedRoom = currentRoom.ConnectRoom(door, newRoom);

                    // Unable to fit new room by any of its doors
                    if (failedRoom != null)
                    {
                        roomsTried.Add(newRoomPrefab);
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
                    RoomPrefab currentRoomPrefab = ChooseRoomByName(currentRoom.GetRoomObject().name);
                    currentRoom.GetParentDoor().GetRoomsTried().Add(currentRoomPrefab);
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

    private RoomPrefab ChooseRoomByName(string roomName)
    {
        return roomOptions.ToList().Find(x => x.ToString() == roomName);
    }

    private RoomPrefab ChooseRoomByDoorCount(HashSet<RoomPrefab> roomsTried, int doorCount)
    {
        List<RoomPrefab> validRooms = roomOptions.Except(roomsTried).ToList().FindAll(x => x.GetDoorCount() == doorCount);

        if (validRooms.Count > 0)
        {
            RoomPrefab chosenRoom = validRooms[Random.Range(0, validRooms.Count)];
            return chosenRoom;
        }
        else
        {
            return null;
        }
    }

    private RoomPrefab ChooseRoom(int depth, HashSet<RoomPrefab> roomsTried)
    {
        if (depth >= 12)
        {
            return ChooseRoomByDoorCount(roomsTried, 1);
        }

        List<RoomPrefab> validRooms = roomOptions.Except(roomsTried).ToList();

        if (validRooms.Count > 0)
        {
            RoomPrefab chosenRoom = validRooms[Random.Range(0, validRooms.Count)];
            return chosenRoom;
        }
        else
        {
            return null;
        }
    }

    private RoomNode CreateRoom(RoomPrefab roomPrefab)
    {
        GameObject room = Instantiate(roomPrefab.GetPrefab(), new Vector3(0, 0, 0), Quaternion.identity, transform);
        room.name = room.name.Replace("(Clone)", "");
        return new RoomNode(room);
    }
}
