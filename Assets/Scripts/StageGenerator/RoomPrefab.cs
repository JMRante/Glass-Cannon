using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab
{
    private GameObject prefab;
    private string roomType;
    private int doorCount;

    public RoomPrefab(GameObject prefab)
    {
        this.prefab = prefab;

        doorCount = 0;
        foreach (Transform child in this.prefab.transform)
        {
            if (child.gameObject.tag.Equals("DoorNode"))
            {
                doorCount += 1;
            }
        }

        roomType = this.prefab.GetComponent<RoomConfig>().roomType;
    }

    public GameObject GetPrefab()
    {
        return prefab;
    }

    public string GetRoomType()
    {
        return roomType;
    }

    public int GetDoorCount()
    {
        return doorCount;
    }

    public override bool Equals(object other)
    {
        return prefab == ((RoomPrefab)other).prefab;
    }
    
    public override int GetHashCode()
    {
        return prefab.name.GetHashCode();
    }

    public override string ToString()
    {
        return prefab.name;
    }
}
