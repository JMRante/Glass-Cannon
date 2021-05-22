using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomOptions
{
    private Dictionary<string, HashSet<RoomPrefab>> roomOptions;
    private Dictionary<string, int> structureOptionCounts;
    private int total;
    private StructureConfig config;
    private Queue<string> syllabus;

    public RoomOptions(GameObject[] prefabs, StructureConfig config)
    {
        roomOptions = new Dictionary<string, HashSet<RoomPrefab>>();
        GameObject[] prefabObjects = Resources.LoadAll<GameObject>("rooms");

        total = 0;
        this.config = config;

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

        syllabus = new Queue<string>();
        config.GetSyllabus().ForEach(x => syllabus.Enqueue(x));
    }

    public void Refresh()
    {
        syllabus = new Queue<string>();
        config.GetSyllabus().ForEach(x => syllabus.Enqueue(x));
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

    public RoomChoice GetRandomRoomOption(string structureType)
    {
        string roomType = config.GetRoomTypeByStructureType(structureType);
        List<RoomPrefab> roomList = GetRoomOptions(roomType).ToList();

        int randomPrefabIndex = Random.Range(0, roomList.Count);
        RoomPrefab randomPrefab = roomList[randomPrefabIndex];
        return new RoomChoice(structureType, randomPrefab);
    }

    public Queue<RoomChoice> GetRoomOptionQueue(string structureType, List<RoomPrefab> roomsAdded, RoomPrefab parentRoom, int openDoorsLeft)
    {
        List<string> structureOptionList = config.GetStructureTypeOptionList(structureType);
        Queue<RoomChoice> roomOptionsList = new Queue<RoomChoice>();

        if (roomsAdded.Count <= config.GetRoomLimit())
        {
        }

        foreach (string structureOption in structureOptionList)
        {
            string roomType = config.GetRoomTypeByStructureType(structureOption);
            List<RoomPrefab> roomList = GetRoomOptions(roomType).ToList();

            while (roomList.Count > 0)
            {
                int randomPrefabIndex = Random.Range(0, roomList.Count);
                RoomPrefab randomPrefab = roomList[randomPrefabIndex];
                roomOptionsList.Enqueue(new RoomChoice(structureOption, randomPrefab));
                roomList.RemoveAt(randomPrefabIndex);
            }
        }

        return roomOptionsList;
    }

    public Queue<RoomChoice> GetCapRoomOptionQueue()
    {
        List<string> structureOptionList = config.GetCapStructureTypeList();
        Queue<RoomChoice> roomOptionsList = new Queue<RoomChoice>();

        foreach (string structureOption in structureOptionList)
        {
            string roomType = config.GetRoomTypeByStructureType(structureOption);
            List<RoomPrefab> roomList = GetRoomOptions(roomType).ToList();

            while (roomList.Count > 0)
            {
                int randomPrefabIndex = Random.Range(0, roomList.Count);
                RoomPrefab randomPrefab = roomList[randomPrefabIndex];
                roomOptionsList.Enqueue(new RoomChoice(structureOption, randomPrefab));
                roomList.RemoveAt(randomPrefabIndex);
            }
        }

        return roomOptionsList;
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
