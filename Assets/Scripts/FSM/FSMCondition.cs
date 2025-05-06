using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class FSMCondition : ScriptableObject
{
    public abstract bool Test(FiniteStateMachine fsm);
}
