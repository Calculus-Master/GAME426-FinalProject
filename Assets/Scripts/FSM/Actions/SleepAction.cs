using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Sleep")]
public class SleepAction : FSMAction
{
    private SleepingBehaviorTree sleepingBT;
    private float timer = 0f;
    private float sleepingDuration = 60f; // Sleeping duration set to 60 seconds clearly as required.

    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.sleepingBehaviorTree == null)
        {
            fsm.sleepingBehaviorTree = new SleepingBehaviorTree();
        }

        var status = fsm.sleepingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());

        if (status == TaskStatus.Success)
        {
            timer += Time.deltaTime;
            if (timer >= sleepingDuration)
            {
                fsm.GetPet().Sleep();
                Debug.Log($"{fsm.GetPet().name} completed sleeping after {sleepingDuration} seconds.");
                timer = 0f; // Reset timer clearly after sleep finishes.
            }
        }
        else
        {
            timer = 0f; // Reset if tree is still running or fails.
        }
    }

    public void Reset()
    {
        timer = 0f;
        sleepingBT = null;
    }
}
