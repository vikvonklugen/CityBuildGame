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
    private CharacterFaction[] factions;
    [SerializeField]
    private CharacterCombatType[] troopTypes;
    [SerializeField]
    private HeroCharacter[] heroes;

    // Start is called before the first frame update
    void Start()
    {
        factions = Resources.LoadAll<CharacterFaction>("Data/Factions");
        troopTypes = Resources.LoadAll<CharacterCombatType>("Data/Troop Types");
        heroes = Resources.LoadAll<HeroCharacter>("Data/Heroes");
        Debug.Log("Loading of game assets completed.");

    }

    private void GenerateRandomHeroes ()
    {
        throw new NotImplementedException();
    }
    private void GenerateRandomFactions ()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
