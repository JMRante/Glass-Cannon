using Tuple = System.Tuple;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private HashSet<RoomPrefab> roomOptions;

    private RoomNode rootRoom;

    private int roomCount = 0;

    private GameObject doorPrefab;

    private StructureConfig config;

    void Start()
    {
        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>("rooms");

        roomOptions = new HashSet<RoomPrefab>();

        foreach (GameObject roomPrefab in roomPrefabs)
            roomOptions.Add(new RoomPrefab(roomPrefab));

        doorPrefab = Resources.Load<GameObject>("Elements/test_door");

        config = new DefaultStructureConfig();

        GenerateStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            GenerateStage();
        }
    }
    
    private void GenerateStage()
    {
        ClearRooms();
        StructureNode structure = GenerateStructure();
        GenerateRooms(structure);
    }
    
    private StructureNode GenerateStructure()
    {
        Stack<StructureNode> dfsStack = new Stack<StructureNode>();

        StructureNode parentNode = new StructureNode(StructureType.start, config);
        dfsStack.Push(parentNode);

        while (dfsStack.Count > 0)
        {
            StructureNode currentNode = dfsStack.Pop();

            IntRange childRange = config.GetChildRange(currentNode.GetStructureType());

            int childCount = UnityEngine.Random.Range(childRange.GetMin(), childRange.GetMax());

            for (int i = 0; i < childCount; i++)
            {
                StructureNode childNode = parentNode.GenerateChild();
                parentNode.AddChild(childNode);
                dfsStack.Push(childNode);
            }
        }

        return parentNode;
    }

    private void GenerateRooms(StructureNode structure)
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();

        roomCount = 0;

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

                    if (newRoomPrefab == null)
                    {
                        newRoom = null;
                        break;
                    }
                    else
                    {
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
                }

                // If no room will work for this door, we need to choose a new room higher up the stack
                if (failedRoom != null || newRoom == null)
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

                    // Add door
                    //Quaternion.Euler(0f, door.GetParentDoorObject().transform.rotation.y + 90, 0f)
                    Instantiate(doorPrefab, door.GetParentDoorObject().transform.position, door.GetParentDoorObject().transform.rotation, newRoom.GetRoomObject().transform);
                }
            }

            FillSlots(currentRoom);
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
        room.name = room.name.Replace("(Clone)", "_" + roomCount);
        roomCount += 1;
        return new RoomNode(room);
    }

    private void FillSlots(RoomNode room)
    {
        foreach (RoomSlot slot in room.GetSlots())
        {
            GameObject slottedPrefab = null;

            if (room.GetParentRoom() == null)
            {
                slottedPrefab = slot.slottedPrefabs.Find(x => x.name == "player");
            }

            if (slottedPrefab != null)
            {
                GameObject slottedObject = Instantiate(slottedPrefab, slot.gameObject.transform.position, Quaternion.identity, transform);
                slottedObject.name = slottedObject.name.Replace("(Clone)", "");
            }
        }
    }
}
