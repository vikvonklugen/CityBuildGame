using System;
using TMPro;
using UnityEngine;

public class WinLoseManager : MonoBehaviour
{

    [SerializeField]
    private int loseAfterTicksWithoutFood;


    [SerializeField]
    [TextArea(2, 5)]
    private string winText, loseText;

    public GameObject infoPanels;


    private int currentTicksWithoutFood;

    public static event Action GameIsLostEvent = delegate { };
    public static event Action GameIsWonEvent = delegate { };


    private void OnEnable()
    {
        GameManager.TickEvent += CheckIfLost;
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
            infoPanels.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = winText;
            GameManager.uiController.heroSelectScreen.SetActive(false);
            GameManager.uiController.buildPanel.SetActive(false);
            GameManager.uiController.upgradePanel.SetActive(false);
            GameManager.uiController.heroHireScreen.SetActive(false);
            GameManager.uiController.eventPanel.SetActive(false);
            GameManager.uiController.eventResultPanel.SetActive(false);
            infoPanels.SetActive(true);
        }
    }

    private void CheckIfLost()
    {
        if (GameManager.resources[AResource.Type.Food] < 0)
        {
            currentTicksWithoutFood++;
            if (currentTicksWithoutFood >= loseAfterTicksWithoutFood)
            {
                infoPanels.transform.GetChild(0).GetChild(0).GetChild(0).gameObject.GetComponent<TextMeshProUGUI>().text = loseText;
                GameManager.uiController.heroSelectScreen.SetActive(false);
                GameManager.uiController.buildPanel.SetActive(false);
                GameManager.uiController.upgradePanel.SetActive(false);
                GameManager.uiController.heroHireScreen.SetActive(false);
                GameManager.uiController.eventPanel.SetActive(false);
                GameManager.uiController.eventResultPanel.SetActive(false);
                infoPanels.SetActive(true);
            }
        }
        else
            currentTicksWithoutFood = 0;
    }

    private void OnDisable()
    {
        GameManager.TickEvent -= CheckIfLost;
        UIController.ResourcesChangedEvent -= CheckIfWon;
    }
}
