using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

[CreateAssetMenu(fileName = "Event", menuName = "NormalEvent")]
public class Event : ScriptableObject
{
    public string description;
    public CharacterFaction enemyType;
    public Reward[] rewards;

    [Serializable]
    public struct Reward
    {
        public AResource.Resource resourceType;
        public int resourceAmount;
    }
}