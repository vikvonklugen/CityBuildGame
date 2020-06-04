using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
public class AResource : ScriptableObject
{
    public enum Resource { None, Food, Materials, Luxuries, Seconds }

    [SerializeField]
    public string name;

    [SerializeField]
    public int startAmount;

    [SerializeField]
    public int maximum;

    [SerializeField]
    public Sprite resourceIcon;
}
