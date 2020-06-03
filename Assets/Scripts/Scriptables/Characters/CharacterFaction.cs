using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Faction", order = 2)]
public class CharacterFaction : ScriptableObject
{
    [SerializeField]
    [Tooltip("Name of the faction.")]
    private string name;

    [SerializeField]
    [Tooltip("The background of the faction.")]
    [TextArea(1,20)]
    private string backGround;

    [SerializeField]
    [Tooltip("The other factions which are considered natural allies for this faction.")]
    private CharacterFaction[] naturalAllies;

    [SerializeField]
    [Tooltip("How much this faction likes other people of same faction.")]
    [Range(-3,3)]
    private int sameFactionModifier;

    [SerializeField]
    [Tooltip("How much this faction dislikes their natural enemies.")]
    [Range(-3,3)]
    private int naturalEnemiesDislikeModifier;

    [SerializeField]
    [Tooltip("The other factions which are considered natural enemies for this faction.")]
    private CharacterFaction[] naturalEnemies;

    [SerializeField]
    [Tooltip("The icon/sprite representing this faction.")]
    private Sprite icon;

    [SerializeField]
    [Tooltip("Can members of this faction die?")]
    private bool canDie;

    [SerializeField]
    [Tooltip("Can members of this faction flee from battle?")]
    private bool canFlee;

    [SerializeField]
    [Tooltip("Can members of this faction change sides?")]
    private bool canBeTurned;

    [SerializeField]
    [Tooltip("Can members of this faction be revived?")]
    private bool canBeRevived;

    [SerializeField]
    [Tooltip("Can members of this faction be recruited?")]
    private bool canBeRecruited;

}
