using UnityEngine;

[CreateAssetMenu(menuName = "Finite State Machine/Actions/Socialize")]
public class SocializeAction : FSMAction
{
    public override void Act(FiniteStateMachine fsm)
    {
        if (fsm.socializingBehaviorTree == null)
        {
            fsm.socializingBehaviorTree = new SocializingBehaviorTree();
        }

        fsm.socializingBehaviorTree.Update(fsm.GetPet(), GameObject.FindObjectOfType<ItemToggleManager>());
    }
}
