using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<string, int> resources = new Dictionary<string, int>
    {
        { "Food", 0 },
        { "Materials", 0 },
        { "Luxuries", 0 },
        { "Population", 0 }
    };

    public static Dictionary<string, int> resourceGrowth = new Dictionary<string, int>
    {
        { "Food", 0 },
        { "Materials", 0 },
        { "Luxuries", 0 },
        { "Population", 5 }
    };

    public UIController uiController;

    private Event[] events;
    private float clockFillAmount;
    private bool eventProcessed;
    private System.Random random;


    void Start()
    {
        uiController.AddResourceIndicators();
        StartCoroutine(TickSystem());

        Object[] eventObjects = Resources.LoadAll("Data/Events/NormalEvents");
        events = new Event[eventObjects.Length];
        eventObjects.CopyTo(events, 0);

        random = new System.Random();
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
            yield return new WaitForSecondsRealtime(0.5f);

            clockFillAmount += 0.25f;
            StartCoroutine(uiController.SetClock(clockFillAmount, 0.01f));

            yield return new WaitUntil(() => UIController.clockAnimationFinished);
            UIController.clockAnimationFinished = false;

            if (clockFillAmount == 1)
            {
                EventTick();

                yield return new WaitUntil(() => eventProcessed);
                eventProcessed = false;
            }
            else if (clockFillAmount == 0.5f)
            {
                PopulationTick();
            }
        }
    }

    void PopulationTick()
    {
        resources["Population"] += 5;
        EveryTick();
    }

    void EventTick()
    {
        EveryTick();

        Event currentevent = events[random.Next(0, events.Length)];
        uiController.DisplayEvent(currentevent);
    }

    void EveryTick()
    {
        resources["Food"] += resourceGrowth["Food"];
        resources["Materials"] += resourceGrowth["Materials"];
        resources["Luxuries"] += resourceGrowth["Luxuries"];
        resources["Population"] += resourceGrowth["Population"];

        uiController.UpdateHUD();
    }
}
