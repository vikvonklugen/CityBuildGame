using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : AResource
{
    [SerializeField]
    private int startingAmount;

    public override void OnEnable ()
    {
        currentAmount = startingAmount;
    }
}
