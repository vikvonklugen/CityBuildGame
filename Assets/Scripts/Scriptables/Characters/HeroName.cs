using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "Hero Name", menuName = "Hero name", order = 4)]
public class HeroName : ScriptableObject
{
    [SerializeField]
    [Tooltip("First name of the hero.")]
    private string firstName;

    [Header("Events")]
    [SerializeField]
    [Tooltip("The event which happens if you recruit the hero. ")]
    private OnRecruited recruitedEvent;

    [SerializeField]
    [Tooltip("The event which happen if the hero dies. ")]
    private OnDeath deathEvent;

    [SerializeField]
    [Tooltip("The event which happends if the hero enters combat. ")]
    private OnCombat combatEvent;

    [SerializeField]
    [Tooltip("The event which happens after a combat result. ")]
    private OnCombatResult combatResultEvent;

    [SerializeField]
    [Tooltip("The event which represent the upset event of this hero. ")]
    private OnUpset upsetEvent;



    [Header("Other stuff")]
    [SerializeField]
    [Tooltip("Expected use frequency once hired. The number indicates how much action the hero expects for every N events with combat." +
        "For example, a number of 7 will indicate the hero expects to be used once or more every 7th event with combat. Failing to do so, will make the hero upset over time.")]
    [Range(1, 100)]
    private int expectedEventUsageFrequency;

    [SerializeField]
    [Tooltip("How content the hero is when recruited freshly.")]
    [Range(-10, 10)]
    private int recruitedHappiness;
}
