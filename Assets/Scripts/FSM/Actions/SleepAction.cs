using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Sleep")]
public class SleepAction : FSMAction
{
    private float sleepingDuration = 60f; // Sleeping duration set to 60 seconds clearly as required.
    private readonly TimerMap _timers = new();

    public override void Act(FiniteStateMachine fsm)
    {
        int id = fsm.GetPet().GetInstanceID();
        
        if (fsm.sleepingBehaviorTree == null)
        {
            fsm.sleepingBehaviorTree = new SleepingBehaviorTree();
        }

        fsm.GetPet().IsNeedDecayPaused = true;
        var status = fsm.sleepingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());

        if (status == TaskStatus.Success)
        {
            this._timers.Update(id, Time.deltaTime);
            if (true || this._timers.Get(id) >= sleepingDuration)
            {
                fsm.GetPet().Sleep();
                Debug.Log($"{fsm.GetPet().name} completed sleeping after {sleepingDuration} seconds.");
                fsm.GetPet().IsNeedDecayPaused = false;
                this._timers.Reset(id); // Reset timer clearly after sleep finishes.
            }
        }
        else
        {
            this._timers.Reset(id); // Reset if tree is still running or fails.
        }
    }
}
