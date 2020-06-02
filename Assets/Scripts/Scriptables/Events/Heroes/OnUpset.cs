using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroUpsetEvent", menuName = "Event/Hero/On Hero Upset")]
public class OnUpset : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}
