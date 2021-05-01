using System.Collections;
using System.Collections.Generic;
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

    public StructureNode(StructureType structureType, StructureConfig config, StructureNode parent)
    {
        this.structureType = structureType;
        this.config = config;
        this.parent = parent;
        
        this.roomType = config.GetRoomTypeByStructureType(this.structureType);

        children = new List<StructureNode>();

        parallelRoomNode = null;
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
        return new StructureNode(config.GetStructureTypeByWeights(structureType), config, this);
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
        IntRange childRange = config.GetChildRange(structureType);
        this.childCount = Random.Range(childRange.GetMin(), childRange.GetMax());

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
        this.parallelRoomNode = null;
        
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
}
