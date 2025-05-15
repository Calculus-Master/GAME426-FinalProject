using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Play")]
public class PlayAction : FSMAction
{
    private float playDuration = 20f; // Must match PlayLeafAction
    private readonly TimerMap _timers = new();

    public override void Act(FiniteStateMachine fsm)
    {
        int id = fsm.GetPet().GetInstanceID();
        
        if (fsm.playingBehaviorTree == null)
        {
            fsm.playingBehaviorTree = new PlayingBehaviorTree();
            fsm.GetPet().IsDonePlaying = false;
        }

        var status = fsm.playingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());
        
        if (status == TaskStatus.Success)
        {
            this._timers.Update(id, Time.deltaTime); 
            if (true || this._timers.Get(id) >= playDuration)
            {
                Debug.Log($"{fsm.GetPet().name} completed playing.");
                fsm.GetPet().IsDonePlaying = true;
                this._timers.Reset(id);
            }
        }
        else
        {
            this._timers.Reset(id); // Reset timer if not successful

            if (status == TaskStatus.Failure) fsm.GetPet().IsDonePlaying = true;
        }
    }
}
