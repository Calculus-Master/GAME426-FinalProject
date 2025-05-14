using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Finite State Machine/Conditions/Max Social at Current Position")]
public class MaxSocialCondition : MaxInfluenceActivityCondition
{
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.GetMaxInfluenceLayer(fsm) == InfluenceLayers.POSITION;
    }
}
