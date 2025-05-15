using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Composite AND Condition")]
public class CompositeANDCondition : FSMCondition
{
    public List<FSMCondition> conditions;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.conditions.TrueForAll(c => c.Test(fsm));
    }
}
