using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StageBuilder : MonoBehaviour
{
    private RoomOptions roomOptions;

    private RoomNode rootRoom;

    private int roomCount = 0;

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
        GenerateStage();
    }

    private void GenerateStage()
    {
        Stack<RoomNode> dfsStack = new Stack<RoomNode>();

        roomCount = 0;

        // Place a starting room (any qualifying end room) at the center of the scene.
        string rootStructureType = "start";
        string rootRoomType = config.GetRoomTypeByStructureType(rootStructureType);
        RoomPrefab rootRoomChoice = ChooseRoom(rootRoomType, new HashSet<RoomPrefab>());
        rootRoom = CreateRoom(rootRoomChoice, rootStructureType);

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
                HashSet<RoomPrefab> roomsTriedTotal = door.GetRoomsTried();

                while (roomsTriedTotal.Count < roomOptions.GetRoomOptionCountByParentStructure(currentRoom.GetStructureType()))
                {
                    // Choose structure and room type and try to fit one. If none work, try another.
                    string structureType;

                    if (roomCount > config.GetRoomLimit())
                    {
                        structureType = config.GetCapStructureType();
                    }
                    else
                    {
                        structureType = config.GetStructureTypeByWeights(currentRoom.GetStructureType());
                    }

                    string roomType = config.GetRoomTypeByStructureType(structureType);

                    HashSet<RoomPrefab> roomsTriedForType = new HashSet<RoomPrefab>(door.GetRoomsTried().Intersect(roomOptions.GetRoomOptions(roomType)));

                    while (roomsTriedForType.Count < roomOptions.GetRoomOptions(roomType).Count)
                    {
                        newRoomPrefab = ChooseRoom(roomType, roomsTriedForType);

                        if (newRoomPrefab == null)
                        {
                            newRoom = null;
                            break;
                        }
                        else
                        {
                            newRoom = CreateRoom(newRoomPrefab, structureType);
                            failedRoom = currentRoom.ConnectRoom(door, newRoom);

                            // Unable to fit new room by any of its doors
                            if (failedRoom != null)
                            {
                                roomsTriedForType.Add(newRoomPrefab);
                                roomsTriedTotal.Add(newRoomPrefab);
                                failedRoom.DestroyRoom();
                            }
                            else
                            {
                                break;
                            }
                        }
                    }

                    if (newRoom != null && failedRoom == null)
                    {
                        break;
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
                    currentRoom.GetParentDoor().GetRoomsTried().Add(currentRoom.GetRoomPrefab());
                    currentRoom.DestroyRoom();
                    break;
                }
                else
                {
                    dfsStack.Push(newRoom);
                    roomsPutOnStack++;

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

    private RoomNode CreateRoom(RoomPrefab roomPrefab, string structureType)
    {
        GameObject room = Instantiate(roomPrefab.GetPrefab(), new Vector3(0, 0, 0), Quaternion.identity, transform);
        room.name = room.name.Replace("(Clone)", "_" + roomCount);
        roomCount += 1;
        return new RoomNode(room, roomPrefab, structureType);
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
