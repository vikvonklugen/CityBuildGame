using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroDeathEvent", menuName = "Event/Hero/On Hero Death")]
public class OnDeath : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}
