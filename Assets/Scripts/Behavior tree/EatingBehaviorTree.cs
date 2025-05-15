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

        // Wait sequence
        Sequence waitSequence = new Sequence();
        waitSequence.AddChild(new IsFoodDishAvailable());
        waitSequence.AddChild(new WaitAction());

        rootSelector.AddChild(eatSequence);
        rootSelector.AddChild(waitSequence);

        root = rootSelector;
    }

    public TaskStatus Update(PetEntity pet, ItemToggleManager itemManager)
    {
        Debug.Log("EatingBehaviorTree Update called for " + pet.name);
        return root.Run(pet, itemManager);
    }
}
