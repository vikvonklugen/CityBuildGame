using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroCombatResultEvent", menuName = "Event/Hero/On Hero Combat Result")]
public class OnCombatResult : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}
