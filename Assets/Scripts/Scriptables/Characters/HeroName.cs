using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Name", menuName = "Hero name", order = 4)]
public class HeroName : ScriptableObject
{
    public int recruitmentCost;

    public string description;

    [Header("Events")]
    [Tooltip("The event which happens if you recruit the hero. ")]
    public OnRecruited recruitedEvent;

    [Tooltip("The event which happen if the hero dies. ")]
    public OnDeath deathEvent;

    [Tooltip("The event which happends if the hero enters combat. ")]
    public OnCombat combatEvent;

    [Tooltip("The event which happens after a combat result. ")]
    public OnCombatResult combatResultEvent;

    [Tooltip("The event which represent the upset event of this hero. ")]
    public OnUpset upsetEvent;



    [Header("Other stuff")]
    [Tooltip("Expected use frequency once hired. The number indicates how much action the hero expects for every N events with combat." +
        "For example, a number of 7 will indicate the hero expects to be used once or more every 7th event with combat. Failing to do so, will make the hero upset over time.")]
    [Range(1, 100)]
    public int expectedEventUsageFrequency;

    [Tooltip("How content the hero is when recruited freshly.")]
    [Range(-10, 10)]
    public int recruitedHappiness;

    [Tooltip("Don't change this variable pls")]
    public int mentality;
}
