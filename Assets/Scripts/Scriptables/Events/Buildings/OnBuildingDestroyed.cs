using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnBuildingDestroyedEvent", menuName = "Event/Building/On Building Destroyed")]
public class OnBuildingDestroyed : AGameEvent<BuildingEventData>
{
    public void Raise (BuildingEventData data)
    {
        base.Raise(data);
    }
}