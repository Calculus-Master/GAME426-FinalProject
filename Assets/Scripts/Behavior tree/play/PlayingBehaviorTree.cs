using UnityEngine;

public class PlayingBehaviorTree
{
    private Task root;
    private WaitAction waitForToy;

    public PlayingBehaviorTree()
    {
        Selector rootSelector = new Selector();

        Sequence playSequence = new Sequence();
        playSequence.AddChild(new ToyTargetingTask());
        playSequence.AddChild(new MoveToToyTask());
        playSequence.AddChild(new PlayLeafAction());

        Sequence waitSequence = new Sequence();
        waitSequence.AddChild(new ToyTargetingTask());
        waitSequence.AddChild(new IsToyAvailable());

        waitForToy = new WaitAction();
        waitSequence.AddChild(waitForToy);

        rootSelector.AddChild(playSequence);
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
        waitForToy.TargetName = pet.CurrentPlayTarget != null
            ? pet.CurrentPlayTarget.name
            : "football"; // Default fallback
    }
}
