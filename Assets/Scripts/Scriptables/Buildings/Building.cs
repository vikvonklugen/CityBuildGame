using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public string name;
    public AudioClip selectedSound;
    public string buildingInfo;
    public Sprite buildingSprite;
    public int buildingCost;
    public AResource.Resource producedResource;
    public int resourceProducedPerTick;
    public float timeToBuild;
    public int initialLevel;

    public Upgrade[] upgrades = new Upgrade[0];

    public CharacterFaction[] factionsAffectedPositively;
    public CharacterFaction[] factionsAffectedNegatively;

    public int factionPositivelyModifier;
    public int factionNegativelyModifier;

    [Serializable]
    public struct Upgrade
    {
        public int materialCost;
        public string description;
        public int productionBoost;
    }
}