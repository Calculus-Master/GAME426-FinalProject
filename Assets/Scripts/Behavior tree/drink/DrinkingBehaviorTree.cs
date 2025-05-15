using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkingBehaviorTree
{
    private Task root;

    public DrinkingBehaviorTree()
    {
        // Build the tree structure
        Selector rootSelector = new Selector();

        // Immediate drink sequence
        Sequence drinkSequence = new Sequence();
        drinkSequence.AddChild(new IsWaterDishAvailable());
        drinkSequence.AddChild(new MoveToWaterDish());
        drinkSequence.AddChild(new DrinkLeafAction());

        // Wait sequence with TargetName set to "water"
        Sequence waitSequence = new Sequence();
        waitSequence.AddChild(new IsWaterDishAvailable());

        // Create and configure the wait action
        WaitAction waitForWater = new WaitAction();
        waitForWater.TargetName = "water";  // << Set the target to "water"

        waitSequence.AddChild(waitForWater);

        // Add both sequences to the root selector
        rootSelector.AddChild(drinkSequence);
        rootSelector.AddChild(waitSequence);

        // Assign the constructed tree to the root
        root = rootSelector;
    }

    public TaskStatus Update(PetEntity pet, ItemToggleManager itemManager)
    {
        //Debug.Log("DrinkingBehaviorTree Update called for " + pet.name);
        return root.Run(pet, itemManager);
    }
}
