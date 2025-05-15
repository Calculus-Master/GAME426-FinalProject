using UnityEngine;
using System.Linq;

public class ToyTargetingTask : Task
{
    private readonly string[] toyNames = { "football", "basketball", "stripeball", "beachball", "bear", "bunny" };

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        ToyAffinityProfile affinityProfile = pet.GetComponent<ToyAffinityProfile>();

        if (affinityProfile == null)
        {
            Debug.LogError($"{pet.name} missing ToyAffinityProfile.");
            return TaskStatus.Failure;
        }

        var sortedToys = toyNames.OrderByDescending(toy => affinityProfile.GetAffinity(toy));

        foreach (string toyName in sortedToys)
        {
            GameObject toy = GameObject.Find(toyName);
            if (toy == null) continue;

            ItemAvailability availability = toy.GetComponent<ItemAvailability>();
            if (availability == null) continue;

            if (availability.IsAvailable() || availability.IsClaimedBy(pet))
            {
                pet.CurrentPlayTarget = toy;
                return TaskStatus.Success;
            }

            PetEntity occupier = availability.GetCurrentUser();
            PetAffinityProfile petAffinity = pet.GetComponent<PetAffinityProfile>();
            PetAffinityProfile occupierAffinity = occupier.GetComponent<PetAffinityProfile>();

            float affinityToOccupier = petAffinity.GetAffinity(occupierAffinity);
            if (affinityToOccupier > 0.85f)
            {
                pet.CurrentPlayTarget = toy; // Wait politely
                return TaskStatus.Success;
            }
            else
            {
                petAffinity.AdjustAffinity(occupierAffinity, -0.05f); // Slightly penalize and continue
            }
        }

        return TaskStatus.Failure; // No toy available or acceptable
    }
}
