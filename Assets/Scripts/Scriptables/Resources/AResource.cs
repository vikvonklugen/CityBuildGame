using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
public class AResource : ScriptableObject
{
    [Serializable]
    public enum Type { Food, Luxuries, Materials, Population, Seconds, None }

    public Type resource;

    public int startAmount;

    public int maximum;

    public Sprite resourceIcon;

    public Sprite resourceBorder;

    [Serializable]
    public struct ResourceBundle
    {
        public ResourceBundle(Type type, int amount)
        {
            resourceType = type;
            resourceAmount = amount;
        }

        public Type resourceType;
        public int resourceAmount;
    }
}
