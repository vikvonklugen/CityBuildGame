using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingController : MonoBehaviour
{
    public Building building;

    public int returnedMaterialsOnDestroy;
    public int productionPerTick;
    public int level;
    public bool buildable;
    public bool unlockedForBuilding;

    private void Start()
    {
        buildable = true;
    }
}
