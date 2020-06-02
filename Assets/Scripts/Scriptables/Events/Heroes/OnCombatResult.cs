using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroCombatResultEvent", menuName = "Event/hero/On Hero Combat Result")]
public class OnCombatResult : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData());
    }
}
