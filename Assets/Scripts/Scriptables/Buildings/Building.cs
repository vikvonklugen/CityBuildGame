using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Building", menuName ="Building")]
public class Building : ScriptableObject
{
    [SerializeField]
    private string name;
    [SerializeField]
    private AudioClip selectedSound;
    [SerializeField]
    private int buildingCost;
    [SerializeField]
    private float timeToBuild;
    [SerializeField]
    private int deconstructionCost;
    [SerializeField]
    private int initialLevel;

    [SerializeField]
    private bool canBeUpgraded;

    [SerializeField]
    [Range(0,3)]
    private int numberOfTimesUpgradeable;

    [SerializeField]
    public CharacterFaction[] factionsAffectedPositively;
    [SerializeField]
    public CharacterFaction[] factionsAffectedNegatively;

    [SerializeField]
    public int factionPositivelyModifier;
    [SerializeField]
    public int factionNegativelyModifier;

}
