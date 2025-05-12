using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Decoration : Influencer
{
    public float baseHappiness;

    public override float GetStrength()
    {
        return baseHappiness;
    }
}
