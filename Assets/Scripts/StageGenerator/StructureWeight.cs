using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWeight
{
    private string structureType;
    private int weight;
    private int rangeStart;

    public StructureWeight(string structureType, int weight, int rangeStart)
    {
        this.structureType = structureType;
        this.weight = weight;
        this.rangeStart = rangeStart;
    }

    public string GetStructureType()
    {
        return structureType;
    }

    public bool IsValueInRange(int value)
    {
        return (value >= rangeStart && value < (weight + rangeStart));
    }
}
