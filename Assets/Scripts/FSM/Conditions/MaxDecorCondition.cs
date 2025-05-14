using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/Max Decor at Current Position")]
public class MaxDecorCondition : MaxInfluenceActivityCondition
{
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.GetMaxInfluenceLayer(fsm) == InfluenceLayers.DECOR;
    }
}
