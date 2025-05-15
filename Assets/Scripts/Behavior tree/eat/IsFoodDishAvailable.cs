using UnityEngine;

/// Checks if the food dish is available or already claimed by this pet.
public class IsFoodDishAvailable : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        GameObject foodDish = GameObject.Find("food");

        if (foodDish == null)
        {
            Debug.LogError("IsFoodDishAvailable: Food object not found!");
            return TaskStatus.Failure;
        }

        ItemAvailability availability = foodDish.GetComponent<ItemAvailability>();
        if (availability == null)
        {
            Debug.LogError("IsFoodDishAvailable: ItemAvailability not found on food dish!");
            return TaskStatus.Failure;
        }

        /// Allow pet to proceed if it already owns the claim.
        if (availability.IsClaimedBy(pet))
        {
            //Debug.Log("IsFoodDishAvailable: Food dish already claimed by " + pet.name);
            return TaskStatus.Success;
        }

        /// Allow if not claimed by anyone.
        if (availability.IsAvailable())
        {
            //Debug.Log("IsFoodDishAvailable: Food dish is available for " + pet.name);
            return TaskStatus.Success;
        }

        /// Otherwise, it's occupied by another pet.
        //Debug.Log("IsFoodDishAvailable: Food dish is occupied, not available for " + pet.name);
        return TaskStatus.Failure;
    }
}
