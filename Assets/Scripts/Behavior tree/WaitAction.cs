using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        return TaskStatus.Running;
    }
}