using UnityEngine;

/// Checks if the water dish is available or already claimed by this pet.
public class IsWaterDishAvailable : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        GameObject waterDish = GameObject.Find("water");

        if (waterDish == null)
        {
            Debug.LogError("IswaterDishAvailable: water object not found!");
            return TaskStatus.Failure;
        }

        ItemAvailability availability = waterDish.GetComponent<ItemAvailability>();
        if (availability == null)
        {
            Debug.LogError("IswaterDishAvailable: ItemAvailability not found on water dish!");
            return TaskStatus.Failure;
        }

        /// Allow pet to proceed if it already owns the claim.
        if (availability.IsClaimedBy(pet))
        {
            //Debug.Log("IswaterDishAvailable: water dish already claimed by " + pet.name);
            return TaskStatus.Success;
        }

        /// Allow if not claimed by anyone.
        if (availability.IsAvailable())
        {
            //Debug.Log("IswaterDishAvailable: water dish is available for " + pet.name);
            return TaskStatus.Success;
        }

        /// Otherwise, it's occupied by another pet.
        //Debug.Log("IswaterDishAvailable: water dish is occupied, not available for " + pet.name);
        return TaskStatus.Failure;
    }
}
