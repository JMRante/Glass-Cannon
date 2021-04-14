using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab
{
    private GameObject prefab;
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
    }

    public GameObject GetPrefab()
    {
        return prefab;
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
