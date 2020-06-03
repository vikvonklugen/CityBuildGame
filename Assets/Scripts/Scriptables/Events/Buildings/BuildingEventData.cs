using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuildingEventData
{
    Vector2 positionbuilt;
    int cost;

    public BuildingEventData (Vector2 pos, int cos)
    {
        positionbuilt = pos;
        cost = cos;
    }
    // Start is called before the first frame update
}
