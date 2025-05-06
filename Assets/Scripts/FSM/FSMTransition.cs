using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Finite State Machine/Transition")]
public class FSMTransition : ScriptableObject
{
    public FSMCondition condition;
    public FSMAction action;
    public FSMState targetState;
    
    public bool IsTriggered(FiniteStateMachine fsm)
    {
        return this.condition && this.condition.Test(fsm);
    }
    
    public FSMState GetTargetState()
    {
        return this.targetState;
    }
    
    public FSMAction GetAction()
    {
        return this.action;
    }
}
