using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource", menuName = "Resource")]
public class AResource : ScriptableObject
{
    [SerializeField]
    public string name;

    [SerializeField]
    public int startAmount;

    [SerializeField]
    public int maximum;

    [SerializeField]
    public Sprite resourceIcon;
}
