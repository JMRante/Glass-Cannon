using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private RoomOptions roomOptions;

    private RoomNode rootRoom;

    private List<RoomPrefab> roomsAdded;

    private GameObject doorPrefab;

    private StructureConfig config;

    void Start()
    {
        config = new DefaultStructureConfig();

        GameObject[] roomPrefabs = Resources.LoadAll<GameObject>("rooms");
        roomOptions = new RoomOptions(roomPrefabs, config);
        
        doorPrefab = Resources.Load<GameObject>("Elements/test_door");

        GenerateStage();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Generate();
        }
    }

    public void Generate()
    {
        ClearRooms();
        roomOptions.Refresh();
        GenerateStage();
    }

    private void GenerateStage()
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();
        
        roomsAdded = new List<RoomPrefab>();
        int iterations = 0;
        int openDoorsLeft = 0;

        // Place a starting room (any qualifying end room) at the center of the scene.
        string rootStructureType = "start";
        RoomChoice rootRoomChoice = roomOptions.GetRandomRoomOption(rootStructureType);
        rootRoom = CreateRoom(rootRoomChoice);

        dfsStack.Push(rootRoom);

        while (dfsStack.Count > 0)
        {
            RoomNode currentRoom = dfsStack.Pop();

            // Go through each door of the parent node and create a room for it.
            int roomsPutOnStack = 0;

            iterations++; if (iterations > 4000) { Debug.Log("BROKE WHOOPSIE " + dfsStack.Count); return; }

            List<DoorEdge> unconnectedChildDoors = currentRoom.GetUnconnectedChildDoors();
            Utility.Shuffle(unconnectedChildDoors);

            foreach (DoorEdge door in currentRoom.GetUnconnectedChildDoors())
            {
                RoomChoice newRoomChoice = null;
                RoomNode newRoom = null;
                RoomNode failedRoom = null;

                Queue<RoomChoice> roomOptionsLeft;

                roomOptionsLeft = roomOptions.GetRoomOptionQueue(currentRoom.GetStructureType(), roomsAdded, currentRoom.GetRoomPrefab(), openDoorsLeft);

                door.SetRoomOptionsLeft(roomOptionsLeft);

                while (roomOptionsLeft.Count > 0)
                {
                    newRoomChoice = roomOptionsLeft.Dequeue();

                    newRoom = CreateRoom(newRoomChoice);
                    failedRoom = currentRoom.ConnectRoom(door, newRoom);

                    // Unable to fit new room by any of its doors
                    if (failedRoom != null)
                    {
                        failedRoom.DestroyRoom(roomsAdded);
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
                        RoomNode poppedRoom = dfsStack.Pop();
                        openDoorsLeft -= poppedRoom.GetDoorCount();
                    }

                    dfsStack.Push(currentRoom.GetParentRoom());
                    openDoorsLeft += currentRoom.GetUnconnectedChildDoors().Count();
                    currentRoom.DestroyRoom(roomsAdded);
                    break;
                }
                else
                {
                    dfsStack.Push(newRoom);
                    roomsPutOnStack++;
                    openDoorsLeft += newRoom.GetDoorCount();
                    openDoorsLeft -= 1;

                    // Add door
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

    private RoomPrefab ChooseRoom(string roomType, HashSet<RoomPrefab> roomsTried)
    {
        List<RoomPrefab> validRooms = roomOptions.GetRoomOptions(roomType).Except(roomsTried).ToList();

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

    private RoomNode CreateRoom(RoomChoice roomChoice)
    {
        GameObject room = Instantiate(roomChoice.GetRoomPrefab().GetPrefab(), new Vector3(0, 0, 0), Quaternion.identity, transform);
        room.name = room.name.Replace("(Clone)", "_" + roomsAdded.FindAll(x => x.GetPrefab().name == roomChoice.GetRoomPrefab().GetPrefab().name).Count);
        roomsAdded.Add(roomChoice.GetRoomPrefab());
        return new RoomNode(room, roomChoice.GetRoomPrefab(), roomChoice.GetStructureType());
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

            if (room.GetStructureType() == "exit")
            {
                slottedPrefab = slot.slottedPrefabs.Find(x => x.name == "balloon_end");
            }

            if (slottedPrefab != null)
            {
                GameObject slottedObject = Instantiate(slottedPrefab, slot.gameObject.transform.position, slottedPrefab.transform.rotation, transform);
                slottedObject.name = slottedObject.name.Replace("(Clone)", "");
            }
        }
    }
}
