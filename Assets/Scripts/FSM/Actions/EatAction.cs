using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Eat")]
public class EatAction : FSMAction
{
    private EatingBehaviorTree eatingBT;
    private bool isEatingComplete = false;
    private float timer = 0f;
    private float eatingDuration = 10f;

    public override void Act(FiniteStateMachine fsm)
{
    Debug.Log("EatAction Act() called for " + fsm.GetPet().name);

    // Ensure FSM holds the BT instance
    if (fsm.eatingBehaviorTree == null)
    {
        fsm.eatingBehaviorTree = new EatingBehaviorTree();
    }

    var status = fsm.eatingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());

    if (status == TaskStatus.Success)
    {
        timer += Time.deltaTime;
        if (timer >= eatingDuration)
        {
            fsm.GetPet().Eat();
            Debug.Log("Eating complete after 10 seconds");
            isEatingComplete = true;
        }
    }
}


    public void Reset()
    {
        isEatingComplete = false;
        timer = 0f;
        eatingBT = null;
    }
}
