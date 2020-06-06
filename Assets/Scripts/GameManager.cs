using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static Dictionary<AResource.Type, int> resources = new Dictionary<AResource.Type, int>
    {
        { AResource.Type.Food, 0 },
        { AResource.Type.Materials, 0 },
        { AResource.Type.Luxuries, 0 },
        { AResource.Type.Population, 0 }
    };

    public static Dictionary<AResource.Type, int> resourceGrowth = new Dictionary<AResource.Type, int>
    {
        { AResource.Type.Food, 0 },
        { AResource.Type.Materials, 0 },
        { AResource.Type.Luxuries, 0 },
        { AResource.Type.Population, 5 }
    };


    public static event Action TickEvent = delegate { };

    public static int hospitalsBuilt;
    public static int stopLuxuryProduction;
    public static int enemyStrengthModifer;
    public static int extraInjuryDuration = 0;
    public static int extraBuildingPrice;
    public static bool forceKlyptrill;
    public static int jonvrabStrengthModifier;

    public UIController UIController;
    public static UIController uiController;
    public HeroManager heroManager;
    public PlayRandom playRandom;

    public static Event currentEvent;

    private Event[] events;
    private float clockFillAmount;
    private bool eventProcessed;
    private System.Random random;

    public static int heroSelectScreenIndex = 0;


    void Start()
    {
        uiController = UIController;

        StartCoroutine(TickSystem());

        UnityEngine.Object[] eventObjects = Resources.LoadAll("Data/Events/NormalEvents");
        events = new Event[eventObjects.Length];
        eventObjects.CopyTo(events, 0);

        random = new System.Random();
        uiController.UpdateHUD();
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
                EventTick();

                yield return new WaitUntil(() => eventProcessed);
                eventProcessed = false;
                currentEvent = null;
            }
            else if (clockFillAmount == 0.5f)
            {
                PopulationTick();
            }
        }
    }

    void PopulationTick()
    {
        StartCoroutine(uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Population, resourceGrowth[AResource.Type.Population])));
        EveryTick();
    }

    void EventTick()
    {
        TickEvent();
        EveryTick();

        currentEvent = events[random.Next(0, events.Length)];
        uiController.DisplayEvent();
        uiController.UpdateHeroSelectScreen();
        uiController.UpdateHeroHireScreen();
    }

    void EveryTick()
    {
        heroManager.AddRecruitableHero();
        uiController.UpdateHeroHireScreen();
        StartCoroutine(uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Food, resourceGrowth[AResource.Type.Food])));
        StartCoroutine(uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Materials, resourceGrowth[AResource.Type.Materials])));
        if (stopLuxuryProduction == 0)
        {
            StartCoroutine(uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Luxuries, resourceGrowth[AResource.Type.Luxuries])));
        }
        else
        {
            stopLuxuryProduction--;
        }

        uiController.UpdateHUD();

        playRandom.StartSound();
    }
}
