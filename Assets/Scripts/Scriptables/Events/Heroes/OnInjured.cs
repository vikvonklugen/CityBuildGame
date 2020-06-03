using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroUpsetEvent", menuName = "Event/Hero/On Hero Injured")]
public class OnInjured : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}
