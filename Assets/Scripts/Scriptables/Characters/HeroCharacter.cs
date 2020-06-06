using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Character", menuName = "Hero persona", order = 3)]
public class HeroCharacter : ScriptableObject
{
    [Tooltip("Type of the hero.")]
    public string heroType;

    [Tooltip("The icon/sprite representing this hero.")]
    public Sprite icon;

    [Tooltip("The faction of this hero.")]
    public CharacterFaction faction;

    [Tooltip("The combat type of this hero.")]
    public CharacterCombatType combattype;

    [Tooltip("The faction the hero has dislikes")]
    public CharacterFaction dislikedFaction;

    [Header("Sounds")]
    [Tooltip("The sound which is played when the hero is recruited. ")]
    public AudioClip recruitSound;

    [Tooltip("The sound which is played when the hero is dead. ")]
    public AudioClip deathSound;

    [Tooltip("The sound which is played when the hero is upset. ")]
    public AudioClip upsetSound;


    [Header("Armor and weapons")]
    public CharacterArmor currentArmor;
    public CharacterWeapon currentWeapon;

    public CharacterArmor desiredArmor;
    public CharacterWeapon desiredWeapon;
}
