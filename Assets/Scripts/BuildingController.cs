using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingController : MonoBehaviour
{
    public Building building;

    [Header("Building events.")]
    public OnBuildingBuilt onBuildingBuiltEvent;
    public OnBuildingDestroyed onBuildingDestroyedEvent;
    public OnBuildingUpgraded onBuildingUpgraded;
    public OnBuildingProducing onBuildingProducing;

    public int returnedMaterialsOnDestroy;
    public int productionPerTick;
    public int level;
    public bool buildable;
    public bool unlockedForBuilding;


    // Start is called before the first frame update
    void Start()
    {
        if (building == null)
        {
            buildable = true;
        }
        else
        {
            buildable = false;
            productionPerTick = building.resourceProducedPerTick;
        }
    }
}
