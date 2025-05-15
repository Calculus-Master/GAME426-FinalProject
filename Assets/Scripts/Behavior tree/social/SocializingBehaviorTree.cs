using UnityEngine;

public class SocializingBehaviorTree
{
    private Task root;

    public SocializingBehaviorTree()
    {
        Sequence socializeSequence = new Sequence();
        socializeSequence.AddChild(new NearestPetTargetingTask());
        socializeSequence.AddChild(new MoveToPetTask());
        socializeSequence.AddChild(new SocializeLeafAction());

        root = socializeSequence;
    }

    public TaskStatus Update(PetEntity pet, ItemToggleManager itemManager)
    {
        return root.Run(pet, itemManager);
    }
}
