using UnityEngine;

public class SocializeLeafAction : Task
{
    private float socializeTimer = 0f;
    private const float socializeDuration = 15f; // Customize clearly

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentSocialTarget == null)
        {
            Debug.LogError("SocializeLeafAction: No pet target set.");
            return TaskStatus.Failure;
        }

        socializeTimer += Time.deltaTime;

        pet.transform.LookAt(pet.CurrentSocialTarget.transform.position);

        if (socializeTimer >= socializeDuration)
        {
            socializeTimer = 0f;

            pet.Socialize(); // Replenish social need

            PetAffinityProfile petAffinity = pet.GetComponent<PetAffinityProfile>();
            PetAffinityProfile otherAffinity = pet.CurrentSocialTarget.GetComponent<PetAffinityProfile>();
            if (petAffinity != null && otherAffinity != null)
            {
                petAffinity.AdjustAffinity(otherAffinity, +0.1f); // Improve affinity clearly
            }

            pet.CurrentSocialTarget = null;

            Debug.Log($"{pet.name} completed socializing.");
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
