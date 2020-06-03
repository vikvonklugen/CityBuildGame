using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="OnHeroRecruitedEvent", menuName ="Event/Hero/On Hero Recruitment")]
public class OnRecruited : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}