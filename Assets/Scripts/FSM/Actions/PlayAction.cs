using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Play")]
public class PlayAction : FSMAction
{
    private float timer = 0f;
    private float playDuration = 20f; // Must match PlayLeafAction
    private PlayingBehaviorTree playingBT;

    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.playingBehaviorTree == null)
        {
            fsm.playingBehaviorTree = new PlayingBehaviorTree();
            fsm.GetPet().IsDonePlaying = false;
        }

        var status = fsm.playingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());
        
        if (status == TaskStatus.Success)
        {
            timer += Time.deltaTime;
            if (timer >= playDuration)
            {
                Debug.Log($"{fsm.GetPet().name} completed playing.");
                fsm.GetPet().IsDonePlaying = true;
                timer = 0f;
            }
        }
        else
        {
            timer = 0f; // Reset timer if not successful
        }
    }

    public void Reset()
    {
        timer = 0f;
        playingBT = null;
    }
}
