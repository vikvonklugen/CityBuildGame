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
        HeroName name = names[random.Next(0, names.Length)];
        Hero randomHero = new Hero(types[random.Next(0, types.Length)], name, 0, name.recruitedMentality, false, 0);

        return randomHero;
    }

    [Serializable]
    public class Hero
    {
        public Hero(HeroCharacter _type, HeroName _heroName, int _strength, int _mentality, bool _injured, int _eventsInjured)
        {
            type = _type;
            heroName = _heroName;
            strength = _strength;
            mentality = _mentality;
            injured = _injured;
            eventsInjured = _eventsInjured;
        }

        public HeroCharacter type;
        public HeroName heroName;
        public int strength;
        public int mentality;
        public bool injured;
        public int eventsInjured;
    }
}
