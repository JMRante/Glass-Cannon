using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureWeightList
{
    private List<StructureWeight> list;
    private int weightSum = 0;

    public StructureWeightList()
    {
        list = new List<StructureWeight>();
    }

    public void AddWeight(StructureType type, int weight)
    {
        list.Add(new StructureWeight(type, weight, weightSum)); 
        weightSum += weight;   
    }

    public StructureType GetStructureTypeByWeights()
    {
        int rand = Random.Range(0, weightSum);
        // Debug.Log(rand + " in " + weightSum);

        return list.Find(x => x.IsValueInRange(rand)).GetStructureType();
    }
}
