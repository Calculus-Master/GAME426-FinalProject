using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu (menuName = "Finite State Machine/Conditions/Composite OR Condition")]
public class CompositeORCondition : FSMCondition
{
    public List<FSMCondition> conditions;
    
    public override bool Test(FiniteStateMachine fsm)
    {
        return this.conditions.Any(c => c.Test(fsm));
    }
}
