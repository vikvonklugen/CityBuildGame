using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASingleChoice : ScriptableObject
{
    public string buttonText;

    public abstract void OnClose ();

}
