using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toy : Influencer
{
    public float basePlayfulness;

    public override float GetStrength()
    {
        return basePlayfulness;
    }
}
