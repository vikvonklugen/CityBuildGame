using UnityEngine;

[CreateAssetMenu(fileName = "Hero Name", menuName = "Hero name", order = 4)]
public class HeroName : ScriptableObject
{
    public int recruitmentCost;

    public string description;

    public string upsetEventText;

    public int upsetMentalityThreshold;

    [Tooltip("How content the hero is when recruited freshly.")]
    [Range(-10, 10)]
    public int recruitedMentality;
}
