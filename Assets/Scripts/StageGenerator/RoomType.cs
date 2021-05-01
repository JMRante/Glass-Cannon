using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomType
{
    private int id;
    private string name;

    public RoomType(string name)
    {
        this.name = name;
        this.id = name.GetHashCode();
    }
    
    public int GetId()
    {
        return id;
    }
    
    public string GetName()
    {
        return name;
    }

    public override bool Equals(object obj)
    {
        if ((obj == null) || !this.GetType().Equals(obj.GetType()))
        {
            return false;
        }
        else
        {
            RoomType rt = (RoomType)obj;
            return rt.id == id;
        }
    }

    public override int GetHashCode()
    {
        return id;
    }

    public override string ToString()
    {
        return name;
    }
}
