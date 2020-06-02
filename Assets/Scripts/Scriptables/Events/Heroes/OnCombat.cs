using UnityEngine;

[CreateAssetMenu(fileName = "OnHeroCombatEvent", menuName = "Event/Hero/On Hero Combat")]
public class OnCombat : AGameEvent<HeroEventData>
{
    public void Raise ()
    {
        base.Raise(new HeroEventData("some hero name",explanation));
    }
}