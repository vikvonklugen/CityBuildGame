using UnityEngine;

[CreateAssetMenu(fileName = "OnBuildingProducingEvent", menuName = "Event/Building/On Building Producing")]
public class OnBuildingProducing : AGameEvent<BuildingEventData>
{
    public void Raise (BuildingEventData data)
    {
        base.Raise(data);
    }
}