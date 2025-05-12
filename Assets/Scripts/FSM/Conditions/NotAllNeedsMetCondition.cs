using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Not All Needs Met Condition")]
public class NotAllNeedsMetCondition : FSMCondition
{
    public List<FSMCondition> needsNotMetConditions;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.needsNotMetConditions.Any(c => c.Test(fsm));
    }
}
