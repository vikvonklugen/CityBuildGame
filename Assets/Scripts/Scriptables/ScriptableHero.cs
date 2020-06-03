using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Hero", menuName = "Hero", order = 101)]
public class SciptableHero : ScriptableObject
{
    [SerializeField]
    private string backGround;
    [SerializeField]
    private string name;
    [SerializeField]
    private Sprite icon;
    [SerializeField]
    private int goldCost;
    [SerializeField]
    private int power;


    #region properties
    public string BackGround
    {
        get
        {
            return backGround;
        }

        private set
        {
            backGround = value;
        }
    }

    public string Name
    {
        get
        {
            return name;
        }

        private set
        {
            name = value;
        }
    }

    public Sprite Icon
    {
        get
        {
            return icon;
        }

        private set
        {
            icon = value;
        }
    }

    public int GoldCost
    {
        get
        {
            return goldCost;
        }

        private set
        {
            goldCost = value;
        }
    }

    public int Power
    {
        get
        {
            return power;
        }

        private set
        {
            power = value;
        }
    }
    #endregion

}
