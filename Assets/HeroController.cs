using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class HeroController : MonoBehaviour
{
    [SerializeField]
    [Tooltip("Allow heroes with same faction and troop type to be generated?")]
    private bool allowDuplicateFactionTroopTypes;

    [SerializeField]
    private List<CharacterFaction> factions;
    [SerializeField]
    private List<CharacterCombatType> troopTypes;
    [SerializeField]
    private List<HeroCharacter> heroes;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }
}
