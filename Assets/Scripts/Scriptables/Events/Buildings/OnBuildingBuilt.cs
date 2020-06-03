using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OnBuildingBuiltEvent", menuName = "Event/Building/On Building Built")]
public class OnBuildingBuilt : AGameEvent<BuildingEventData>
{
    public void Raise(BuildingEventData data)
    {
        base.Raise(data);
    }
}