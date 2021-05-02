using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureNode
{
    private StructureNode parent;
    private StructureType structureType;
    private RoomType roomType;
    private int childCount = 0;
    private List<StructureNode> children;
    private StructureConfig config;

    private RoomNode parallelRoomNode;
    public HashSet<RoomTry> triedRooms;
    private HashSet<RoomPrefab> allRoomOptions;
    private HashSet<RoomPrefab> roomOptions;

    public StructureNode(StructureType structureType, StructureConfig config, StructureNode parent, HashSet<RoomPrefab> allRoomOptions)
    {
        this.structureType = structureType;
        this.config = config;
        this.parent = parent;
        
        this.roomType = config.GetRoomTypeByStructureType(this.structureType);

        children = new List<StructureNode>();

        parallelRoomNode = null;
        triedRooms = new HashSet<RoomTry>();
        this.allRoomOptions = allRoomOptions;
        roomOptions = new HashSet<RoomPrefab>();
    }

    public StructureType GetStructureType()
    {
        return structureType;
    }

    public RoomType GetRoomType()
    {
        return roomType;
    }

    public StructureNode GenerateChild()
    {
        return new StructureNode(config.GetStructureTypeByWeights(structureType), config, this, allRoomOptions);
    }

    public void AddChild(StructureNode childNode)
    {
        children.Add(childNode);
    }

    public List<StructureNode> GetChildren()
    {
        return children;
    }

    public List<StructureNode> GetNonInstantiatedChildren()
    {
        return children.FindAll(x => x.GetRoomNode() == null);
    }

    public void SetChildCount()
    {
        IntRange childRange = config.GetDoorRange(structureType);
        this.childCount = Random.Range(childRange.GetMin(), childRange.GetMax());

        allRoomOptions.ToList()
            .FindAll(x => x.GetDoorCount() == childCount && x.GetPrefab().GetComponent<RoomConfig>().roomType.GetHashCode() == GetRoomType().GetId())
            .ForEach(x => roomOptions.Add(x));

        // Cut off door representing parent
        if (parent != null)
        {
            this.childCount -= 1; 
        }
    }

    public int GetChildCount()
    {
        return childCount;
    }

    public void SetRoomNode(RoomNode parallelRoomNode)
    {
        this.parallelRoomNode = parallelRoomNode;
    }

    public void SetRoomNodeNullRecursively()
    {
        parallelRoomNode = null;
        triedRooms = new HashSet<RoomTry>(); ;
        
        foreach (StructureNode node in children)
        {
            node.SetRoomNodeNullRecursively();
        }
    }

    public RoomNode GetRoomNode()
    {
        return parallelRoomNode;
    }

    public StructureNode GetParent()
    {
        return parent;
    }

    public override string ToString()
    {
        return structureType.ToString();
    }

    public void AddRoomAsTried(RoomPrefab roomPrefab, Transform roomTransform)
    {
        triedRooms.Add(new RoomTry(roomPrefab, roomTransform));
        Debug.Log("Try " + roomPrefab.GetPrefab().name + " " + roomTransform.position + "/" + roomTransform.rotation);
    }

    public bool HasRoomBeenTried(RoomPrefab roomPrefab, Transform roomTransform)
    {
        return triedRooms.Contains(new RoomTry(roomPrefab, roomTransform));
    }

    public bool HasRoomBeenExhausted(RoomPrefab roomPrefab)
    {
        bool value = triedRooms.ToList().FindAll(x => x.GetRoomPrefab().Equals(roomPrefab)).Count == roomPrefab.GetDoorCount();
        Debug.Log("HasRoomBeenExhausted" + value + " " + triedRooms.Count + " " + triedRooms.ToList().FindAll(x => x.GetRoomPrefab().Equals(roomPrefab)).Count + " " + roomPrefab.GetDoorCount());
        return value;
    }

    public bool HaveAllRoomsBeenExhausted()
    {
        string list = "";
        roomOptions.ToList().ForEach(x => list += x.GetPrefab().name + " ");
        Debug.Log("Options for " + GetStructureType() + " remaining: " + list);
        return !roomOptions.ToList().Exists(x => !HasRoomBeenExhausted(x));
    }

    public RoomPrefab ChooseRoom()
    {
        List<RoomPrefab> validRooms = roomOptions.ToList().FindAll(x => !HasRoomBeenExhausted(x));

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
}
