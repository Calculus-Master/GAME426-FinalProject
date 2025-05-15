using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "Finite State Machine/Entry Actions/Start Idle Wander Timer")]
public class StartIdleWanderingTimerAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm)
    {
        fsm.GetPet().StartIdleWandering();
    }
}
