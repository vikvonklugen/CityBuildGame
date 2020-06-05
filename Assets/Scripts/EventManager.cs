using System.Collections;
using System.Collections.Generic;
using UnityEditor.iOS;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    System.Random random = new System.Random();
    private int lossCount = 0;
    private string eventResultText;
    private string buildingName;

    void Start()
    {
        
    }

    public void EventFight()
    {
        HeroManager.Hero hero = HeroManager.recruitedHeroes[GameManager.heroSelectScreenIndex];
        CharacterFaction enemyFaction = GameManager.currentEvent.enemyType;

        int enemyStrength = random.Next(0, 7);
        int heroStrength = random.Next(0, 7);

        if (hero.type.faction.strongAgainst == enemyFaction)
        {
            heroStrength++;
        }
        else if (hero.type.faction.weakAgainst == enemyFaction)
        {
            heroStrength--;
        }

        int battleResult = Mathf.Clamp(heroStrength - enemyStrength, -2, 2);
        Debug.Log(battleResult);

        switch (battleResult)
        {
            case -2:
                LoseEvent();
                if (buildingName == null)
                {
                    buildingName = "empty field";
                }
                eventResultText = 
                    GameManager.currentEvent.failAction.Replace("HERO", hero.heroName.name) + " " + 
                    GameManager.currentEvent.eventLoseText.Replace("BUILDING", buildingName.ToLower()) + " " + 
                    hero.heroName.name + " is injured.";
                hero.injured = true;
                hero.eventsInjured = 3;
                break;
            case -1:
                eventResultText = "";
                hero.injured = true;
                hero.eventsInjured = 3;
                break;
            case 0:
                eventResultText = "";
                GiveReward();
                hero.injured = true;
                hero.eventsInjured = 3;
                break;
            case 1:
                eventResultText = "";
                GiveReward();
                break;
            case 2:
                eventResultText = "";
                GiveReward();
                hero.mentality++;
                break;
        }

        GameManager.uiController.FinalizeEvent(eventResultText);
    }

    void GiveReward()
    {
        Debug.Log(GameManager.currentEvent.rewards.Length);
        foreach (AResource.ResourceBundle resourceBundle in GameManager.currentEvent.rewards)
        {
            StartCoroutine(GameManager.uiController.AddResources(resourceBundle));
        }
    }

    void LoseEvent()
    {
        buildingName = GameManager.uiController.DestroyBuilding(true);

        int lostFood = (int)Mathf.Pow(2, lossCount) * HeroManager.recruitedHeroes.Count;
        StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Food, -lostFood)));
        lossCount++;
    }
}
