using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct HeroEventData
{
    string heroName;
    string explanation;

    public HeroEventData (string heroName, string explanation)
    {
        this.heroName = heroName ?? throw new ArgumentNullException(nameof(heroName));
        this.explanation = explanation ?? throw new ArgumentNullException(nameof(explanation));
    }

    // Start is called before the first frame update
}
