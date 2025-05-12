using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Drink")]
public class DrinkAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm)
    {
        // TODO: Insert behavior tree here
        
        // once the tree is done, make sure to update the need:
        fsm.GetPet().Drink();
    }
}
