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

    public void AddWeight(string type, int weight)
    {
        list.Add(new StructureWeight(type, weight, weightSum)); 
        weightSum += weight;   
    }

    public string GetStructureTypeByWeights()
    {
        int rand = Random.Range(0, weightSum);

        return list.Find(x => x.IsValueInRange(rand)).GetStructureType();
    }

    public List<string> GetPossibilities()
    {
        List<string> possibilities = new List<string>();

        foreach (StructureWeight sw in list)
        {
            possibilities.Add(sw.GetStructureType());
        }

        return possibilities;
    }
}
