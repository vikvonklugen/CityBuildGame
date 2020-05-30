using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Trooptype", menuName = "Trooptype", order = 1)]
public class CharacterCombatType : ScriptableObject
{
    public enum Combattype
    {
        melee = 0,
        ranged = 1,
        special = 2
    }


    [SerializeField]
    private Combattype combattype;

    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int goldCost;
    [SerializeField]
    private int power;

    [SerializeField]
    [Tooltip("Which combat type this unit is weak against.")]
    private Combattype weakAgainst;
    [SerializeField]
    [Tooltip("How much weaker this is against the specified combat type.")]
    private int weakAgainstCombatModifier;

    [SerializeField]
    [Tooltip("Which combat type this unit is strong against.")]
    private Combattype strongAgainst;
    [SerializeField]
    [Tooltip("How much stronger this is against the specified combat type.")]
    private int strongAgainstCombatModifier;

}
