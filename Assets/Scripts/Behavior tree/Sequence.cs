using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sequence : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        foreach (var child in children)
        {
            TaskStatus childStatus = child.Run(pet, itemManager);
            if (childStatus == TaskStatus.Failure)
                return TaskStatus.Failure;
            if (childStatus == TaskStatus.Running)
                return TaskStatus.Running;
        }
        return TaskStatus.Success;
    }
}