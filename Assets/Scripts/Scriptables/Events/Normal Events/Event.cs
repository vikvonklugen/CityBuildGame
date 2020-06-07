using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "NormalEvent")]
public class Event : ScriptableObject
{
    public string description;
    public string eventLoseText;
    public string eventWinText;
    public CharacterFaction enemyType;
    public AResource.ResourceBundle[] rewards;
}