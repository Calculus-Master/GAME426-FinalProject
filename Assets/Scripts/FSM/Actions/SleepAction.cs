using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Sleep")]
public class SleepAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm)
    {
        // TODO: Insert behavior tree here
        
        // once the tree is done, make sure to update the need:
        fsm.GetPet().Sleep();
    }
}
