using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitAction : Task
{
    /// Reference to the target item the pet is waiting for.
    private GameObject targetItem;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        /// Locate the food dish in the scene.
        if (targetItem == null)
        {
            targetItem = GameObject.Find("food");
            if (targetItem == null)
            {
                Debug.LogError("WaitAction: Target item 'food' not found in the scene.");
                return TaskStatus.Failure;
            }
        }

        /// Get the ItemAvailability component.
        ItemAvailability availability = targetItem.GetComponent<ItemAvailability>();
        if (availability == null)
        {
            Debug.LogError("WaitAction: No ItemAvailability component found on " + targetItem.name);
            return TaskStatus.Failure;
        }

        /// If the item becomes available, succeed.
        if (availability.IsAvailable())
        {
            return TaskStatus.Success;
        }

        /// Get the occupying pet.
        PetEntity occupyingPet = availability.GetCurrentUser();
        if (occupyingPet == null)
        {
            /// Defensive fallback if no pet is actually claiming it.
            return TaskStatus.Success;
        }

        /// Get PetAffinityProfile components.
        PetAffinityProfile petAffinityProfile = pet.GetComponent<PetAffinityProfile>();
        PetAffinityProfile occupyingAffinityProfile = occupyingPet.GetComponent<PetAffinityProfile>();

        if (petAffinityProfile == null || occupyingAffinityProfile == null)
        {
            Debug.LogError("WaitAction: Missing PetAffinityProfile on one of the pets.");
            return TaskStatus.Failure;
        }

        /// Evaluate affinity toward the occupying pet.
        float affinity = petAffinityProfile.GetAffinity(occupyingAffinityProfile);

        /// If affinity is high, wait politely.
        if (affinity > 0.85f)
        {
            Debug.Log("WaitAction: waiting politely.");
            return TaskStatus.Running;
        }
        else
        {
            /// If affinity is low, decrease it further as punishment for being blocked.
            petAffinityProfile.AdjustAffinity(occupyingAffinityProfile, -0.05f);
            Debug.Log("WaitAction: upset about being blcked.");
            return TaskStatus.Running;
        }
    }
}
