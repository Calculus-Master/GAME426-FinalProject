using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsFoodDishAvailable : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        Debug.Log("food dish available for " + pet.name);
        return TaskStatus.Success;

    }
}