using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "Hero Weapon", order = 5)]
public class CharacterWeapon : ScriptableObject
{
    [SerializeField]
    [Tooltip("Trooptype this weapon is designed for.")]
    private CharacterCombatType useableByTroopType;

    [SerializeField]
    [Tooltip("List of factions incompatible to use this weapon.")]
    private CharacterFaction[] incompatibleFactions;

    [SerializeField]
    [Tooltip("Allows anyone to use this weapon, regardless of faction or troop type.")]
    private bool genericArmorBypassUsageRestrictions;

    [SerializeField]
    private string name;
    [SerializeField]
    private string description;
    [SerializeField]
    private int value;
    [SerializeField]
    private CharacterFaction originFaction;
    [SerializeField]
    private bool destructable;
    [SerializeField]
    private int damage;

    [SerializeField]
    [Tooltip("Positive or negative impact if the wearer of this weapon is same origin as the weapon.")]
    [Range(-5,5)]
    private int originBonus;

}
