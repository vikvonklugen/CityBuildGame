using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Character", menuName = "Hero persona", order = 3)]
public class HeroCharacter : ScriptableObject
{
    [HideInInspector]
    private static int heroId;

    [SerializeField]
    [Tooltip("Type of the hero.")]
    public string heroType;

    [SerializeField]
    [Tooltip("The icon/sprite representing this hero.")]
    public Sprite icon;

    [Header("Other stuff")]
    [SerializeField]
    [Range(0,120)]
    [Tooltip("Age of the hero.")]
    public int age;

    [SerializeField]
    [Tooltip("The faction of this hero.")]
    public CharacterFaction faction;

    [SerializeField]
    [Tooltip("The combat type of this hero.")]
    public CharacterCombatType combattype;

    [SerializeField]
    [Tooltip("The factions the hero have personal grudges against:")]
    public CharacterFaction[] grudgedFactions;

    [SerializeField]
    [Tooltip("The combat modifier the hero have against the factions he have grudges against.")]
    [Range(-2,2)]
    public int grudgeFactionCombatModifier;


    [SerializeField]
    [Tooltip("Is this hero recruitable?")]
    public bool recruitable;

    [SerializeField]
    [Tooltip("Expected use frequency once hired. The number indicates how much action the hero expects for every N events with combat." +
        "For example, a number of 7 will indicate the hero expects to be used once or more every 7th event with combat. Failing to do so, will make the hero upset over time.")]
    [Range(1,100)]
    public int expectedEventUsageFrequency;

    [SerializeField]
    [Tooltip("How content the hero is when recruited freshly.")]
    [Range(-10,10)]
    public int recruitedHappiness;

    [Header("Sounds")]
    [SerializeField]
    [Tooltip("The sound which is played when the hero is recruited. ")]
    public AudioClip recruitSound;

    [SerializeField]
    [Tooltip("The sound which is played when the hero is dead. ")]
    public AudioClip deathSound;

    [SerializeField]
    [Tooltip("The sound which is played when the hero is upset. ")]
    public AudioClip upsetSound;


    [Header("Armor and weapons")]
    [SerializeField]
    public CharacterArmor currentArmor;
    [SerializeField]
    public CharacterWeapon currentWeapon;

    [SerializeField]
    public CharacterArmor desiredArmor;
    [SerializeField]
    public CharacterWeapon desiredWeapon;
}
