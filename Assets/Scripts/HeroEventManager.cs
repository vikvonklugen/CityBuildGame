using UnityEngine;

public class HeroEventManager : MonoBehaviour
{
    private System.Random random = new System.Random();

    public void OnParticipatingEvent(HeroManager.Hero hero)
    {
        Debug.Log("Participate");
        switch (hero.heroName.name)
        {
            case "Haldorf":
                StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Materials, -GameManager.resources[AResource.Type.Materials] / 2)));
                break;

            case "Lartrid":
                GameManager.stopLuxuryProduction = 1;
                break;

            case "Zrantic":
                GameManager.enemyStrengthModifer = 2;
                break;

            case "Werthuz":
                StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Luxuries, -hero.heroName.recruitmentCost / 2)));
                break;

            case "Klpytrill":
                GameManager.forceKlyptrill = false;
                break;

        }
    }

    public void OnRecruit(HeroManager.Hero hero)
    {
        switch (hero.heroName.name)
        {
            case "Friptali":
                GameManager.extraInjuryDuration += 2;
                break;

            case "Jonvrab":
                GameManager.jonvrabStrengthModifier--;
                break;

            case "Klyptrill":
                GameManager.extraBuildingPrice += 2;
                GameManager.uiController.UpdateBuildMenu();
                break;

        }
    }

    public void UpsetEvent(HeroManager.Hero hero)
    {
        HeroManager.Hero randomHero;
        string friptaliBattleResult = "";

        switch (hero.heroName.name)
        {
            case "Haldorf":

                break;

            case "Lartrid":
                GameManager.stopLuxuryProduction = 3;
                break;

            case "Friptali":
                randomHero = HeroManager.recruitedHeroes[random.Next(0, HeroManager.recruitedHeroes.Count)];

                int randomHeroStrength = random.Next(0, 7) + randomHero.strength;
                int heroStrength = random.Next(0, 7) + hero.strength;
                GameManager.enemyStrengthModifer = 0;

                if (hero.type.faction.strongAgainst == randomHero.type.faction)
                {
                    heroStrength += 2;
                }
                else if (hero.type.faction.weakAgainst == randomHero.type.faction)
                {
                    heroStrength -= 2;
                }

                int battleResult = Mathf.Clamp(heroStrength - randomHeroStrength, -2, 2);

                switch (battleResult)
                {
                    case 2:
                        friptaliBattleResult = "He fought " + randomHero.heroName.name + " the " + randomHero.type.heroType + " and won! " + randomHero.heroName.name + " is injured, but Friptali gained 1 strength!";
                        randomHero.injured = true;
                        randomHero.eventsInjured = 3;
                        hero.strength++;
                        break;
                    case 1:
                        friptaliBattleResult = "He fought " + randomHero.heroName.name + " the " + randomHero.type.heroType + " and won! " + randomHero.heroName.name + " is injured!";
                        randomHero.injured = true;
                        randomHero.eventsInjured = 3;
                        break;
                    case 0:
                        friptaliBattleResult = "He fought " + randomHero.heroName.name + " the " + randomHero.type.heroType + ". They both injured each other!";
                        hero.injured = true;
                        hero.eventsInjured = 3;
                        randomHero.injured = true;
                        randomHero.eventsInjured = 3;
                        break;
                    case -1:
                        friptaliBattleResult = "He fought " + randomHero.heroName.name + " the " + randomHero.type.heroType + " and lost! Friptali is injured!";
                        hero.injured = true;
                        hero.eventsInjured = 3;
                        break;
                    case -2:
                        friptaliBattleResult = "He fought " + randomHero.heroName.name + " the " + randomHero.type.heroType + " and won! Friptali is injured, but " + randomHero.heroName.name + " gained 1 strength!";
                        hero.injured = true;
                        hero.eventsInjured = 3;
                        randomHero.strength++;
                        break;
                }

                break;

            case "Plarkull":
                foreach (HeroManager.Hero _hero in HeroManager.recruitedHeroes)
                {
                    _hero.mentality--;
                }
                break;

            case "Jonvrab":
                hero.injured = true;
                hero.eventsInjured = 3;
                break;

            case "Zrantic":
                randomHero = HeroManager.recruitedHeroes[random.Next(0, HeroManager.recruitedHeroes.Count)];
                randomHero.injured = true;
                randomHero.eventsInjured = 3;
                break;

            case "Werthuz":
                StartCoroutine(GameManager.uiController.AddResources(new AResource.ResourceBundle(AResource.Type.Food, -HeroManager.recruitedHeroes.Count * 2)));
                break;

            case "Klyptrill":
                GameManager.forceKlyptrill = true;
                break;

        }

        hero.mentality = 0;
        GameManager.uiController.DisplayUpsetEvent(hero, friptaliBattleResult);
    }
}
