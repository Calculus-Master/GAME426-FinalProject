using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/All Needs Met Condition")]
public class AllNeedsMetCondition : FSMCondition
{
    public List<FSMCondition> needsMetConditions;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.needsMetConditions.TrueForAll(c => c.Test(fsm));
    }
}
