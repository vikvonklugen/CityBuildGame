using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static Dictionary<string, int> resources = new Dictionary<string, int>
    {
        { "Food", 0 },
        { "Materials", 0 },
        { "Luxuries", 0 },
        { "Population", 0 }
    };

    public UIController uiController;

    private float clockFillAmount;
    private bool eventProcessed;


    void Start()
    {
        uiController.AddResourceIndicators();
        StartCoroutine(TickSystem());
    }

    public void ProcessEvent()
    {
        eventProcessed = true;
        clockFillAmount = 0f;
        StartCoroutine(uiController.ResetClock());
    }

    IEnumerator TickSystem()
    {
        while (true)
        {
            yield return new WaitForSecondsRealtime(7.5f);

            clockFillAmount += 0.25f;
            StartCoroutine(uiController.SetClock(clockFillAmount, 0.01f));

            yield return new WaitUntil(() => UIController.clockAnimationFinished);
            UIController.clockAnimationFinished = false;

            if (clockFillAmount == 1)
            {
                //Do event stuff
                uiController.GenerateEvent();

                yield return new WaitUntil(() => eventProcessed);
                eventProcessed = false;
            }
            else if (clockFillAmount == 0.5f)
            {
                //Grow population
            }

            uiController.UpdateHUD();
        }
    }
}
