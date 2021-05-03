using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomOptions
{
    private Dictionary<string, HashSet<RoomPrefab>> roomOptions;
    private Dictionary<string, int> structureOptionCounts;
    private int total;

    public RoomOptions(GameObject[] prefabs, StructureConfig config)
    {
        roomOptions = new Dictionary<string, HashSet<RoomPrefab>>();
        GameObject[] prefabObjects = Resources.LoadAll<GameObject>("rooms");

        total = 0;

        foreach (GameObject prefabObject in prefabObjects)
        {
            RoomPrefab roomPrefab = new RoomPrefab(prefabObject);
            
            if (!roomOptions.ContainsKey(roomPrefab.GetRoomType()))
            {
                roomOptions[roomPrefab.GetRoomType()] = new HashSet<RoomPrefab>();
            }

            roomOptions[roomPrefab.GetRoomType()].Add(roomPrefab);
            total += 1;
        }

        structureOptionCounts = new Dictionary<string, int>();

        foreach (string structureType in config.GetStructureTypes())
        {
            if (!structureOptionCounts.ContainsKey(structureType))
            {
                structureOptionCounts[structureType] = 0;
            }

            List<string> structureList = config.GetStructurePossibilities(structureType);

            foreach (string childStructure in structureList)
            {
                string roomType = config.GetRoomTypeByStructureType(childStructure);
                structureOptionCounts[structureType] += roomOptions[roomType].Count;
            }
        }
    }

    public HashSet<RoomPrefab> GetRoomOptions(string roomType)
    {
        if (roomOptions.Count > 0 && roomOptions.ContainsKey(roomType))
        {
            return roomOptions[roomType];
        }

        return new HashSet<RoomPrefab>();
    }

    public RoomPrefab GetRoomPrefabByName(string name)
    {
        foreach(string type in roomOptions.Keys)
        {
            foreach(RoomPrefab prefab in roomOptions[type])
            {
                if (prefab.GetPrefab().name == name)
                {
                    return prefab;
                }
            }
        }

        return null;
    }

    public int GetRoomOptionCountByParentStructure(string structureType)
    {
        return structureOptionCounts[structureType];
    }

    public int GetTotalCount()
    {
        return total;
    }
}
