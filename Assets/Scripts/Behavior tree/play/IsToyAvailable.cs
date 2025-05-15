using UnityEngine;

public class IsToyAvailable : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentPlayTarget == null)
        {
            Debug.LogError("IsToyAvailable: No CurrentPlayTarget set.");
            return TaskStatus.Failure;
        }

        ItemAvailability availability = pet.CurrentPlayTarget.GetComponent<ItemAvailability>();

        if (availability == null)
        {
            Debug.LogError("IsToyAvailable: No ItemAvailability component on toy.");
            return TaskStatus.Failure;
        }

        return (availability.IsAvailable() || availability.IsClaimedBy(pet))
            ? TaskStatus.Success
            : TaskStatus.Failure;
    }
}
