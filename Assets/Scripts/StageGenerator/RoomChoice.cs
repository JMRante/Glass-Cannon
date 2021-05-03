using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomChoice
{
    private string structureType;
    private RoomPrefab prefab;

    public RoomChoice(string structureType, RoomPrefab prefab)
    {
        this.structureType = structureType;
        this.prefab = prefab;
    }

    public string GetStructureType()
    {
        return structureType;
    }

    public RoomPrefab GetRoomPrefab()
    {
        return prefab;
    }
}
