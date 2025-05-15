using UnityEngine;
using System.Linq;

public class NearestPetTargetingTask : Task
{
    public override TaskStatus Run(PetEntity pet, ItemToggleManager itemManager)
    {
        PetEntity[] allPets = GameObject.FindObjectsOfType<PetEntity>();
        PetEntity nearestPet = allPets
            .Where(p => p != pet)
            .OrderBy(p => Vector3.Distance(p.transform.position, pet.transform.position))
            .FirstOrDefault();

        if (nearestPet == null)
        {
            Debug.LogError("No other pets found for socializing.");
            return TaskStatus.Failure;
        }

        pet.CurrentSocialTarget = nearestPet.gameObject;
        return TaskStatus.Success;
    }
}
