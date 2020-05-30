using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ABuilding : ScriptableObject
{
    public string name;
    public AudioClip selectedSound;
    public int buildingCost;

    public abstract void OnBuild ();
    public abstract void OnDemolish ();

    public abstract void OnPassiveBuildingEffect ();
}
