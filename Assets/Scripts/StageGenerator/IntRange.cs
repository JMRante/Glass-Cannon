using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntRange
{
    private int min;
    private int max;

    public IntRange(int min, int max)
    {
        this.min = min;
        this.max = max;
    }

    public int GetMin()
    {
        return min;
    }

    public int GetMax()
    {
        return max;
    }
}
