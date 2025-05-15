using UnityEngine;

public class SleepTargetingTask : Task
{
    private GameObject pinkRug;
    private GameObject blackRug;

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pinkRug == null)
            pinkRug = GameObject.Find("pinkrug");
        if (blackRug == null)
            blackRug = GameObject.Find("blackrug");

        if (pinkRug == null || blackRug == null)
        {
            Debug.LogError("SleepTargetingTask: Rugs not found.");
            return TaskStatus.Failure;
        }

        ToyAffinityProfile affinityProfile = pet.GetComponent<ToyAffinityProfile>();
        if (affinityProfile == null)
        {
            Debug.LogError("SleepTargetingTask: ToyAffinityProfile missing from pet.");
            return TaskStatus.Failure;
        }

        float pinkAffinity = affinityProfile.GetAffinity("pinkrug");
        float blackAffinity = affinityProfile.GetAffinity("blackrug");

        GameObject preferredRug = pinkAffinity >= blackAffinity ? pinkRug : blackRug;
        GameObject alternateRug = preferredRug == pinkRug ? blackRug : pinkRug;

        ItemAvailability availability = preferredRug.GetComponent<ItemAvailability>();
        if (availability.IsAvailable() || availability.IsClaimedBy(pet))
        {
            pet.CurrentSleepTarget = preferredRug;
            return TaskStatus.Success;
        }
        else
        {
            PetEntity occupier = availability.GetCurrentUser();
            PetAffinityProfile petAffinity = pet.GetComponent<PetAffinityProfile>();
            PetAffinityProfile occupierAffinity = occupier.GetComponent<PetAffinityProfile>();

            if (petAffinity.GetAffinity(occupierAffinity) > 0.85f)
            {
                pet.CurrentSleepTarget = preferredRug;  // Wait politely.
                return TaskStatus.Success;
            }
            else
            {
                petAffinity.AdjustAffinity(occupierAffinity, -0.05f);
                availability = alternateRug.GetComponent<ItemAvailability>();
                if (availability.IsAvailable())
                {
                    pet.CurrentSleepTarget = alternateRug;
                    return TaskStatus.Success;
                }
                pet.CurrentSleepTarget = alternateRug;  // Fallback wait
                return TaskStatus.Success;
            }
        }
    }
}
