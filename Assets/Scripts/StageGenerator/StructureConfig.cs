using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureConfig
{
    private List<RoomType> roomTypes;
    private List<StructureType> structureTypes;

    private Dictionary<StructureType, StructureWeightList> weightConfig;
    private Dictionary<StructureType, RoomType> roomMappings;
    private Dictionary<StructureType, IntRange> doorRange;

    private int roomLimit = 2;

    public StructureConfig()
    {
        roomTypes = new List<RoomType>
        {
            new RoomType("minorCap"),
            new RoomType("majorCap"),
            new RoomType("pathway"),
            new RoomType("gateway"),
            new RoomType("hub"),
            new RoomType("arena"),
            new RoomType("meta"),
            new RoomType("boss"),
            new RoomType("bonus"),
            new RoomType("deadEndCap"),
        };
        structureTypes = new List<StructureType>();

        weightConfig = new Dictionary<StructureType, StructureWeightList>();
        roomMappings = new Dictionary<StructureType, RoomType>();
        doorRange = new Dictionary<StructureType, IntRange>();
    }

    public RoomType GetRoomType(string name)
    {
        return roomTypes.Find(x => x.GetName() == name);
    }

    public StructureType GetStructureType(string name)
    {
        return structureTypes.Find(x => x.GetName() == name);
    }

    public void AddStructureTypeDefinition(StructureType structureType)
    {
        structureTypes.Add(structureType);
    }

    public void AddWeight(StructureType structureType, StructureType childStructureType, int weight)
    {
        if (!weightConfig.ContainsKey(structureType))
        {
            weightConfig[structureType] = new StructureWeightList();
        }

        weightConfig[structureType].AddWeight(childStructureType, weight);
    }

    public StructureType GetStructureTypeByWeights(StructureType structureType)
    {
        return weightConfig[structureType].GetStructureTypeByWeights();
    }

    public void AddRoomMapping(StructureType structureType, RoomType roomType)
    {
        roomMappings.Add(structureType, roomType);
    }

    public RoomType GetRoomTypeByStructureType(StructureType structureType)
    {
        return roomMappings[structureType];
    }

    public void AddDoorRange(StructureType structureType, int minChildren, int maxChildren)
    {
        this.doorRange.Add(structureType, new IntRange(minChildren, maxChildren));
    }

    public IntRange GetDoorRange(StructureType structureType)
    {
        return doorRange[structureType];
    }

    public void SetRoomLimit(int roomLimit)
    {
        this.roomLimit = roomLimit;
    }

    public int GetRoomLimit()
    {
        return roomLimit;
    }
}
