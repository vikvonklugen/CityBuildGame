using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Character", menuName = " Hero persona", order = 3)]
public class HeroCharacter : ScriptableObject
{
    [HideInInspector]
    private static int heroId;

    [SerializeField]
    [Tooltip("First name of the hero.")]
    private string firstName;

    [SerializeField]
    [Tooltip("Last name of the hero.")]
    private string lastName;

    [SerializeField]
    [Tooltip("The icon/sprite representing this hero.")]
    private Sprite icon;


    [SerializeField]
    [Range(0,120)]
    [Tooltip("Age of the hero.")]
    private int age;

    [SerializeField]
    [Tooltip("The faction of this hero.")]
    private CharacterFaction faction;

    [SerializeField]
    [Tooltip("The combat type of this hero.")]
    private CharacterCombatType combattype;

    [SerializeField]
    [Tooltip("The factions the hero have personal grudges against:")]
    private CharacterFaction[] grudgedFactions;

    [SerializeField]
    [Tooltip("The combat modifier the hero have against the factions he have grudges against.")]
    [Range(-2,2)]
    private int grudgeFactionCombatModifier;


    [SerializeField]
    [Tooltip("Is this hero recruitable?")]
    private bool recruitable;

    [SerializeField]
    [Tooltip("Expected use frequency once hired. The number indicates how much action the hero expects for every N events with combat." +
        "For example, a number of 7 will indicate the hero expects to be used once or more every 7th event with combat. Failing to do so, will make the hero upset over time.")]
    [Range(1,100)]
    private int expectedEventUsageFrequency;

    [SerializeField]
    [Tooltip("How content the hero is when recruited freshly.")]
    [Range(0,5)]
    private int recruitedHappiness;

    [SerializeField]
    private CharacterArmor currentArmor;
    [SerializeField]
    private CharacterWeapon currentWeapon;

    [SerializeField]
    private CharacterArmor desiredArmor;
    [SerializeField]
    private CharacterWeapon desiredWeapon;

}
