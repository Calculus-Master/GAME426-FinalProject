using UnityEngine;

public class IsRugAvailable : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentSleepTarget == null)
        {
            Debug.LogError("IsRugAvailable: No CurrentSleepTarget set for pet.");
            return TaskStatus.Failure;
        }

        ItemAvailability availability = pet.CurrentSleepTarget.GetComponent<ItemAvailability>();
        if (availability == null)
        {
            Debug.LogError("IsRugAvailable: ItemAvailability missing from rug.");
            return TaskStatus.Failure;
        }

        if (availability.IsAvailable() || availability.IsClaimedBy(pet))
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }
}
