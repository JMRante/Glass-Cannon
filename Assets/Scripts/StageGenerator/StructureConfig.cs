using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureConfig
{
    private Dictionary<StructureType, StructureWeightList> weightConfig;
    private Dictionary<StructureType, RoomType> roomMappings;
    private Dictionary<StructureType, IntRange> childRange;

    public StructureConfig()
    {
        weightConfig = new Dictionary<StructureType, StructureWeightList>();
        roomMappings = new Dictionary<StructureType, RoomType>();
        childRange = new Dictionary<StructureType, IntRange>();
    }

    public void AddWeight(StructureType structureType, StructureType childStructureType, int weight)
    {
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

    public void AddChildRange(StructureType structureType, int minChildren, int maxChildren)
    {
        this.childRange.Add(structureType, new IntRange(minChildren, maxChildren));
    }

    public IntRange GetChildRange(StructureType structureType)
    {
        return childRange[structureType];
    }
}
