using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AResource : ScriptableObject
{
    [SerializeField]
    public string name;

    [SerializeField]
    public int currentAmount;

    [SerializeField]
    public int bottomLimit;

    [SerializeField]
    public int maximum;

    public abstract void OnEnable ();
}
