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

    public void EventFight()
    {
        HeroManager.Hero hero = HeroManager.recruitedHeroes[GameManager.heroSelectScreenIndex];
        CharacterFaction enemyFaction = GameManager.currentEvent.enemyType;

        int enemyStrength = random.Next(0, 7);
        int heroStrength = random.Next(0, 7) + hero.strength;

        if (hero.type.faction.strongAgainst == enemyFaction)
        {
            heroStrength++;
        }
        else if (hero.type.faction.weakAgainst == enemyFaction)
        {
            heroStrength--;
        }

        int battleResult = Mathf.Clamp(heroStrength - enemyStrength, -2, 2);

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
                eventResultText = GameManager.currentEvent.eventWinText.Replace("HERO", hero.heroName.name) +
                    " However, HERO was injured and lost 1 strength.".Replace("HERO", hero.heroName.name);
                hero.strength--;
                hero.injured = true;
                hero.eventsInjured = 3;
                break;

            case 0:
                eventResultText = GameManager.currentEvent.eventWinText.Replace("HERO", hero.heroName.name) +
                    " However, HERO was injured.".Replace("HERO", hero.heroName.name);
                hero.injured = true;
                hero.eventsInjured = 3;
                break;

            case 1:
                eventResultText = GameManager.currentEvent.eventWinText.Replace("HERO", hero.heroName.name);
                break;

            case 2:
                if (hero.strength < 3)
                {
                    eventResultText = GameManager.currentEvent.eventWinText.Replace("HERO", hero.heroName.name) + 
                        " HERO gained 1 strength and mentality!".Replace("HERO", hero.heroName.name);
                    hero.strength++;
                }
                else
                {
                    eventResultText = GameManager.currentEvent.eventWinText.Replace("HERO", hero.heroName.name) +
                        " HERO gained 1 mentality!".Replace("HERO", hero.heroName.name);
                }
                hero.mentality++;
                break;
        }

        foreach (HeroManager.Hero hiredHero in HeroManager.recruitedHeroes)
        {
            hiredHero.eventsInjured--;
            if (hiredHero.eventsInjured == 0)
            {
                hiredHero.injured = false;
            }
        }

        GameManager.uiController.FinalizeEvent(eventResultText);
    }

    void LoseEvent()
    {
        buildingName = GameManager.uiController.DestroyBuilding(true);

        int lostFood = (int)Mathf.Pow(2, lossCount) * HeroManager.recruitedHeroes.Count;
        StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Food, -lostFood)));
        lossCount++;
    }
}
