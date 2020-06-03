using UnityEngine;

[CreateAssetMenu(fileName = "OnBuildingUpgradedEvent", menuName = "Event/Building/On Building upgraded")]
public class OnBuildingUpgraded : AGameEvent<BuildingEventData>
{
    public void Raise (BuildingEventData data)
    {
        base.Raise(data);
    }
}