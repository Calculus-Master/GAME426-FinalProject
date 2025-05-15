using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatingBehaviorTree
{
    private Task root;

    public EatingBehaviorTree()
    {
        // Build the tree structure
        Selector rootSelector = new Selector();

        // Immediate eat sequence
        Sequence eatSequence = new Sequence();
        eatSequence.AddChild(new IsFoodDishAvailable());
        eatSequence.AddChild(new MoveToFoodDish());
        eatSequence.AddChild(new EatLeafAction());

        // Wait sequence with TargetName set to "food"
        Sequence waitSequence = new Sequence();
        waitSequence.AddChild(new IsFoodDishAvailable());

        // Create and configure the wait action
        WaitAction waitForFood = new WaitAction();
        waitForFood.TargetName = "food";  // << Set the target to "food"

        waitSequence.AddChild(waitForFood);

        // Add both sequences to the root selector
        rootSelector.AddChild(eatSequence);
        rootSelector.AddChild(waitSequence);

        // Assign the constructed tree to the root
        root = rootSelector;
    }

    public TaskStatus Update(PetEntity pet, ItemToggleManager itemManager)
    {
        //Debug.Log("EatingBehaviorTree Update called for " + pet.name);
        return root.Run(pet, itemManager);
    }
}
