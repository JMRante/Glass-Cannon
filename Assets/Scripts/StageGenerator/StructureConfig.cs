using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureConfig
{
    private List<string> roomTypes;
    private List<string> structureTypes;

    private Dictionary<string, StructureWeightList> weightConfig;
    private Dictionary<string, string> roomMappings;

    private List<string> capRooms;

    private int roomLimit = 2;

    public StructureConfig()
    {
        roomTypes = new List<string>
        {
            "minorCap",
            "majorCap",
            "pathway",
            "gateway",
            "hub",
            "arena",
            "meta",
            "boss",
            "bonus",
            "deadEndCap"
        };
        structureTypes = new List<string>();

        weightConfig = new Dictionary<string, StructureWeightList>();
        roomMappings = new Dictionary<string, string>();

        capRooms = new List<string>();
    }

    public List<string> GetRoomTypes()
    {
        return roomTypes;
    }

    public List<string> GetStructureTypes()
    {
        return structureTypes;
    }

    public void AddStructureTypeDefinition(string structureType)
    {
        structureTypes.Add(structureType);
    }

    public void AddWeight(string structureType, string childStructureType, int weight)
    {
        if (!weightConfig.ContainsKey(structureType))
        {
            weightConfig[structureType] = new StructureWeightList();
        }

        weightConfig[structureType].AddWeight(childStructureType, weight);
    }

    public string GetStructureTypeByWeights(string structureType)
    {
        return weightConfig[structureType].GetStructureTypeByWeights();
    }

    public List<string> GetStructurePossibilities(string structureType)
    {
        if (weightConfig.ContainsKey(structureType))
        {
            return weightConfig[structureType].GetPossibilities();
        }
        else
        {
            return new List<string>();
        }
    }

    public List<string> GetStructureTypeOptionList(string structureType)
    {
        List<string> options = new List<string>();
        int optionsFinalCount = weightConfig[structureType].GetCount();

        while (options.Count < optionsFinalCount)
        {
            string drawOption = GetStructureTypeByWeights(structureType);

            if (!options.Contains(drawOption))
            {
                options.Add(drawOption);
            }
        }

        return options;
    }

    public void AddRoomMapping(string structureType, string roomType)
    {
        roomMappings.Add(structureType, roomType);
    }

    public string GetRoomTypeByStructureType(string structureType)
    {
        return roomMappings[structureType];
    }

    public void AddCapRoom(string structureType)
    {
        capRooms.Add(structureType);
    }

    public string GetCapStructureType()
    {
        return capRooms[Random.Range(0, capRooms.Count)];
    }

    public List<string> GetCapStructureTypeList()
    {
        return capRooms;
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
