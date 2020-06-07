using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WinLooseChecker : MonoBehaviour
{

    [SerializeField]
    private int looseAfterTicksWithoutFood;


    [SerializeField]
    [TextArea(2, 5)]
    private string winText, looseText;

    [SerializeField]
    private GameObject infoPanels;


    private int currentTicksWithoutFood;





    public static event Action GameIsLostEvent = delegate { };
    public static event Action GameIsWonEvent = delegate { };


    private void OnEnable()
    {
        GameManager.TickEvent += CheckIfLoose;
        UIController.ResourcesChangedEvent += CheckIfWon;
    }



    private void Awake()
    {
        infoPanels.SetActive(false);
    }


    private void CheckIfWon()
    {
        if (GameManager.resources[AResource.Type.Population] >= 15)
        {
            GameIsWonEvent();
            infoPanels.transform.GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = winText;
            GameManager.uiController.eventPanel.SetActive(false);
            infoPanels.SetActive(true);
        }
    }




    private void CheckIfLoose()
    {
        if (GameManager.resources[AResource.Type.Food] < 0)
        {
            currentTicksWithoutFood++;
            if (currentTicksWithoutFood >= looseAfterTicksWithoutFood)
            {
                GameIsLostEvent();
                infoPanels.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = looseText;
                GameManager.uiController.eventPanel.SetActive(false);
                infoPanels.SetActive(true);
            }
        }
        else
            currentTicksWithoutFood = 0;
    }



    private void OnDisable()
    {
        GameManager.TickEvent -= CheckIfLoose;
        UIController.ResourcesChangedEvent -= CheckIfWon;
    }

    // check when a tick happens if there is food

}
