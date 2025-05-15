using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatLeafAction : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        Debug.Log("Ealeafaction entered for " + pet.name);
        pet.Eat();
        return TaskStatus.Success;
    }
}

