using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureType
{
    private int id;
    private string name;

    public StructureType(string name)
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
            StructureType st = (StructureType) obj;
            return st.id == id;
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
