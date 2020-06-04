using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Faction", menuName = "Faction", order = 2)]
public class CharacterFaction : ScriptableObject
{
    public Sprite icon;

    public CharacterFaction strongAgainst;
    public CharacterFaction weakAgainst;
}
