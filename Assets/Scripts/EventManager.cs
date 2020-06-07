using UnityEngine;

public class EventManager : MonoBehaviour
{
    System.Random random = new System.Random();
    private int lossCount = 0;
    private string eventResultText;
    private string buildingName;
    private bool firstEvent = true;

    public HeroEventManager heroEventManager;

    public void EventFight(bool sendcitizen = false)
    {
        if (!sendcitizen)
        {
            HeroManager.Hero hero = HeroManager.recruitedHeroes[GameManager.heroSelectScreenIndex];
            CharacterFaction enemyFaction = GameManager.currentEvent.enemyType;

            int enemyStrength = random.Next(0, 7) + GameManager.enemyStrengthModifer;
            int heroStrength = random.Next(0, 7) + hero.strength;
            if (hero.heroName.name != "Jonvrab")
            {
                heroStrength += GameManager.jonvrabStrengthModifier;
            }
            GameManager.enemyStrengthModifer = 0;

            if (hero.type.faction.strongAgainst == enemyFaction)
            {
                heroStrength++;
            }
            else if (hero.type.faction.weakAgainst == enemyFaction)
            {
                heroStrength--;
            }

            hero.mentality++;

            heroEventManager.OnParticipatingEvent(hero);

            int battleResult = Mathf.Clamp(heroStrength - enemyStrength, -2, 2);

            if (firstEvent)
            {
                battleResult = 2;
                firstEvent = false;
            }

            switch (battleResult)
            {
                case -2:
                    LoseEvent();
                    if (buildingName == null)
                    {
                        buildingName = "empty field";
                    }
                    eventResultText =
                        GameManager.currentEvent.eventLoseText.Replace("X", hero.heroName.name) + " " +
                        hero.heroName.name + " is injured and lost 1 mentality.";
                    hero.injured = true;
                    hero.eventsInjured = 3 + GameManager.extraInjuryDuration;
                    hero.mentality--;
                    break;

                case -1:
                    eventResultText = GameManager.currentEvent.eventWinText.Replace("X", hero.heroName.name) +
                        " However, HERO was injured and lost 1 strength.".Replace("HERO", hero.heroName.name);
                    hero.strength--;
                    hero.injured = true;
                    hero.eventsInjured = 3 + GameManager.extraInjuryDuration;
                    break;

                case 0:
                    eventResultText = GameManager.currentEvent.eventWinText.Replace("X", hero.heroName.name) +
                        " However, HERO was injured.".Replace("HERO", hero.heroName.name);
                    hero.injured = true;
                    hero.eventsInjured = 3 + GameManager.extraInjuryDuration;
                    break;

                case 1:
                    eventResultText = GameManager.currentEvent.eventWinText.Replace("X", hero.heroName.name);
                    break;

                case 2:
                    if (hero.strength < 3)
                    {
                        eventResultText = GameManager.currentEvent.eventWinText.Replace("X", hero.heroName.name) +
                            " HERO gained 1 strength and mentality!".Replace("HERO", hero.heroName.name);
                        hero.strength++;
                    }
                    else
                    {
                        eventResultText = GameManager.currentEvent.eventWinText.Replace("X", hero.heroName.name) +
                            " HERO gained 1 mentality!".Replace("HERO", hero.heroName.name);
                    }
                    hero.mentality++;
                    break;
            }
        }
        else
        {
            LoseEvent();

            if (buildingName == null)
            {
                buildingName = "empty field";
            }

            string citizentext = GameManager.currentEvent.eventLoseText.Replace("X", "the random citizen");

            eventResultText = char.ToUpper(citizentext[0]) + citizentext.Substring(1);
        }

        foreach (HeroManager.Hero hiredHero in HeroManager.recruitedHeroes)
        {
            hiredHero.mentality--;
            hiredHero.eventsInjured--;
            if (hiredHero.eventsInjured == 0)
            {
                hiredHero.injured = false;
            }
            if (hiredHero.eventsInjured < 2 && GameManager.hospitalsBuilt > 0)
            {
                hiredHero.injured = false;
            }

            if (hiredHero.mentality <= hiredHero.heroName.upsetMentalityThreshold)
            {
                heroEventManager.UpsetEvent(hiredHero);
            }
        }

        GameManager.uiController.FinalizeEvent(eventResultText, sendcitizen);
    }

    public void SendCitizen()
    {
        EventFight(true);
    }

    void LoseEvent()
    {
        buildingName = GameManager.uiController.DestroyBuilding(true);

        int lostFood = (int)Mathf.Pow(2, lossCount) * HeroManager.recruitedHeroes.Count;
        StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Food, -lostFood)));
        lossCount++;
    }
}
