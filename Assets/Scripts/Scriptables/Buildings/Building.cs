using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Building")]
public class Building : ScriptableObject
{
    public enum Resource { None, Food, Materials, Luxuries }

    public string name;
    public AudioClip selectedSound;
    public string buildingInfo;
    public Sprite buildingSprite;
    public int buildingCost;
    public Resource producedResource;
    public int resourceProducedPerTick;
    public float timeToBuild;
    public int initialLevel;

    public bool canBeUpgraded;

    [Range(0, 3)]
    public int numberOfTimesUpgradeable;

    public CharacterFaction[] factionsAffectedPositively;
    public CharacterFaction[] factionsAffectedNegatively;

    public int factionPositivelyModifier;
    public int factionNegativelyModifier;
}