using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomType
{
    minorCap,
    majorCap,
    pathway,
    gateway,
    hub,
    arena,
    meta,
    boss,
    bonus,
    deadEndCap
}

public enum StructureType
{
    start,
    end,
    pathway,
    hub
}

public class StructureNode
{
    private StructureType structureType;
    private RoomType roomType;
    private List<StructureNode> children;
    private StructureConfig config;

    public StructureNode(StructureType structureType, StructureConfig config)
    {
        this.structureType = structureType;
        this.config = config;
        
        this.roomType = config.GetRoomTypeByStructureType(this.structureType);

        children = new List<StructureNode>();
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
        return new StructureNode(config.GetStructureTypeByWeights(structureType), config);
    }

    public void AddChild(StructureNode childNode)
    {
        children.Add(childNode);
    }
}
