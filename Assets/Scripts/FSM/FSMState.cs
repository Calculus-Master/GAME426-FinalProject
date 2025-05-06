using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (menuName = "Finite State Machine/State")]
public class FSMState : ScriptableObject
{
    public FSMAction entryAction;
    public FSMAction[] stateActions;
    public FSMAction exitAction;
    
    public FSMTransition[] transitions;
    
    public FSMAction GetEntryAction()
    {
        return entryAction;
    }
    
    public FSMAction[] GetActions()
    {
        return stateActions;
    }
    
    public FSMAction GetExitAction()
    {
        return exitAction;
    }
    
    public FSMTransition[] GetTransitions()
    {
        return transitions;
    }
}
