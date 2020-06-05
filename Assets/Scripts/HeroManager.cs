using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : MonoBehaviour
{
    public static List<Hero> recruitableHeroes = new List<Hero>();
    public static List<Hero> recruitedHeroes = new List<Hero>();

    public UIController uiController;

    private HeroCharacter[] types;
    private HeroName[] names;
    private System.Random random = new System.Random();

    void Start()
    {
        UnityEngine.Object[] typeObjects = Resources.LoadAll("Data/Heroes/Types");
        types = new HeroCharacter[typeObjects.Length];
        typeObjects.CopyTo(types, 0);

        UnityEngine.Object[] nameObjects = Resources.LoadAll("Data/Heroes/Names");
        names = new HeroName[nameObjects.Length];
        nameObjects.CopyTo(names, 0);

        AddRecruitableHero();
        uiController.UpdateHeroHireScreen();
    }

    public void AddRecruitableHero()
    {
        Hero randomHero = ReturnRandomHero();
        recruitableHeroes.Add(randomHero);
    }

    Hero ReturnRandomHero()
    {
        Hero randomHero;
        randomHero.type = types[random.Next(0, types.Length)];
        randomHero.heroName = names[random.Next(0, names.Length)];
        randomHero.injured = false;
        randomHero.eventsInjured = 0;
        randomHero.mentality = randomHero.heroName.recruitedMentality;

        return randomHero;
    }

    [Serializable]
    public struct Hero
    {
        public HeroCharacter type;
        public HeroName heroName;
        public int mentality;
        public bool injured;
        public int eventsInjured;
    }
}
