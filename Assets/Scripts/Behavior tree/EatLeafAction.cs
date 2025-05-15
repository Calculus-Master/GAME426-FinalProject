using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatLeafAction : Task
{
    /// Cached reference to the food dish GameObject.
    private GameObject foodDish;

    /// Timer to track eating duration.
    private float eatingTimer = 0f;

    /// Total required eating time in seconds.
    private const float eatingDuration = 10f;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        /// Locate the food dish if not already cached.
        if (foodDish == null)
        {
            foodDish = GameObject.Find("food");
            if (foodDish == null)
            {
                Debug.LogError("EatLeafAction: Food object not found in the scene.");
                return TaskStatus.Failure;
            }
        }

        /// Increase the timer by the frame's delta time.
        eatingTimer += Time.deltaTime;

        /// Check if the pet has finished eating.
        if (eatingTimer >= eatingDuration)
        {
            /// Reset the timer for future use.
            eatingTimer = 0f;

            /// Perform the eating action.
            pet.Eat();

            /// Release the claim on the food dish if claimed by this pet.
            ItemAvailability availability = foodDish.GetComponent<ItemAvailability>();
            if (availability != null && availability.IsClaimedBy(pet))
            {
                availability.Release();
            }

            Debug.Log("EatLeafAction: " + pet.name + " has finished eating and released the food dish.");
            return TaskStatus.Success;
        }

        /// Continue running until the timer completes.
        return TaskStatus.Running;
    }
}
