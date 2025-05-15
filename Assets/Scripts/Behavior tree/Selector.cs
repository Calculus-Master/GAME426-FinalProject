using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selector : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        foreach (var child in children)
        {
            TaskStatus childStatus = child.Run(pet, itemManager);
            if (childStatus == TaskStatus.Success)
                return TaskStatus.Success;
            if (childStatus == TaskStatus.Running)
                return TaskStatus.Running;
        }
        return TaskStatus.Failure;
    }
}
