using UnityEngine;

public class SleepingBehaviorTree
{
    private Task root;
    private WaitAction waitForRug;  // Cached clearly here for easy access

    public SleepingBehaviorTree()
    {
        Selector rootSelector = new Selector();

        Sequence sleepSequence = new Sequence();
        sleepSequence.AddChild(new SleepTargetingTask());
        sleepSequence.AddChild(new MoveToRugTask());
        sleepSequence.AddChild(new SleepLeafAction());

        Sequence waitSequence = new Sequence();
        waitSequence.AddChild(new SleepTargetingTask());
        waitSequence.AddChild(new IsRugAvailable());

        waitForRug = new WaitAction();
        waitSequence.AddChild(waitForRug);

        rootSelector.AddChild(sleepSequence);
        rootSelector.AddChild(waitSequence);

        root = rootSelector;
    }

    public TaskStatus Update(PetEntity pet, ItemToggleManager itemManager)
    {
        SetWaitActionTarget(pet);
        return root.Run(pet, itemManager);
    }

    private void SetWaitActionTarget(PetEntity pet)
    {
        waitForRug.TargetName = pet.CurrentSleepTarget != null ? pet.CurrentSleepTarget.name : "pinkrug";
    }
}
