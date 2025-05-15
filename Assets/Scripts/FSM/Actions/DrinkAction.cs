using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Drink")]
public class DrinkAction : FSMAction
{
    private DrinkingBehaviorTree drinkingBT;
    private bool isDrinkingComplete = false;
    private float timer = 0f;
    private float drinkingDuration = 10f;

    public override void Act(FiniteStateMachine fsm)
    {
        // Ensure FSM holds the BT instance
        if (fsm.drinkingBehaviorTree == null)
        {
            fsm.drinkingBehaviorTree = new DrinkingBehaviorTree();
        }

        var status = fsm.drinkingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());

        if (status == TaskStatus.Success)
        {
            timer += Time.deltaTime;
            if (timer >= drinkingDuration)
            {
                fsm.GetPet().Drink();
                Debug.Log("Drinking complete after 10 seconds");
                isDrinkingComplete = true;
            }
        }
    }

    public void Reset()
    {
        isDrinkingComplete = false;
        timer = 0f;
        drinkingBT = null;
    }
}
