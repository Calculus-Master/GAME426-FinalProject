using System.Collections.Generic;
using UnityEngine;

public abstract class Task
{
    protected List<Task> children = new List<Task>();
    protected TaskStatus status = TaskStatus.None;

    public abstract TaskStatus Run(PetEntity pet, ItemToggleManager itemManager);

    public void AddChild(Task child)
    {
        children.Add(child);
    }
}
