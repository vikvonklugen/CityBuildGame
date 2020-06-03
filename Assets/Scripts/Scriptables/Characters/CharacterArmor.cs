using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "Armor", menuName = "Hero Armor", order = 4)]
public class CharacterArmor : ScriptableObject
{
    [SerializeField]
    [Tooltip("Trooptype this armor is designed for.")]
    private CharacterCombatType useableByTroopType;

    [SerializeField]
    [Tooltip("List of factions incompatible to use this armor.")]
    private CharacterFaction[] incompatibleFactions;

    [SerializeField]
    [Tooltip("Allows anyone to use this armor, regardless of faction or troop type.")]
    private bool genericArmorBypassUsageRestrictions;

    [SerializeField]
    private string name;

    [SerializeField]
    [TextArea(1,10)]
    private string description;

    [SerializeField]
    private int value;

    [SerializeField]
    private CharacterFaction originFaction;

    [SerializeField]
    private bool destructable;

    [SerializeField]
    private int armor;

    [SerializeField]
    [Tooltip("Positive or negative impact if the wearer of this armor is same origin as the armor.")]
    [Range(-5,5)]
    private int originBonus;
}
