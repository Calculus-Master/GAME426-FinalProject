using UnityEngine;

public class PlayLeafAction : Task
{
    private float playTimer = 0f;
    private const float playDuration = 20f; // Customize play duration here

    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        if (pet.CurrentPlayTarget == null)
        {
            Debug.LogError("PlayLeafAction: No toy target set.");
            return TaskStatus.Failure;
        }

        ItemAvailability availability = pet.CurrentPlayTarget.GetComponent<ItemAvailability>();
        if (availability == null)
        {
            Debug.LogError("PlayLeafAction: Toy lacks ItemAvailability.");
            return TaskStatus.Failure;
        }

        // Claim toy if not already claimed
        if (availability.IsAvailable())
        {
            availability.Claim(pet);
        }

        playTimer += Time.deltaTime;

        if (playTimer >= playDuration)
        {
            playTimer = 0f;

            availability.Release();
            pet.CurrentPlayTarget = null;

            Debug.Log($"{pet.name} has finished playing with {availability.gameObject.name}.");
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
