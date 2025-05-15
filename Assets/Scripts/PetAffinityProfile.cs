using System.Collections.Generic;
using UnityEngine;

public class PetAffinityProfile : MonoBehaviour
{
    [System.Serializable]
    public struct PetAffinity
    {
        public PetAffinityProfile targetPet;
        [Range(0f, 1f)] public float affinityValue;
    }

    public List<PetAffinity> affinities = new List<PetAffinity>();

    void Start()
    {
        BuildInitialAffinities();
    }

    // Generate affinities toward all pets currently in the scene (excluding self)
    public void BuildInitialAffinities()
    {
        PetAffinityProfile[] allPets = FindObjectsOfType<PetAffinityProfile>();
        foreach (var otherPet in allPets)
        {
            if (otherPet != this && !AffinityExists(otherPet))
            {
                AddAffinity(otherPet, Random.Range(0f, 1f));
            }
        }
    }

    // Called by new pets to let all existing pets register them
    public static void RegisterNewPet(PetAffinityProfile newPet)
    {
        PetAffinityProfile[] existingPets = FindObjectsOfType<PetAffinityProfile>();
        foreach (var pet in existingPets)
        {
            if (pet != newPet && !pet.AffinityExists(newPet))
            {
                pet.AddAffinity(newPet, Random.Range(0f, 1f));
            }

            if (newPet != pet && !newPet.AffinityExists(pet))
            {
                newPet.AddAffinity(pet, Random.Range(0f, 1f));
            }
        }
    }

    private bool AffinityExists(PetAffinityProfile target)
    {
        foreach (var affinity in affinities)
        {
            if (affinity.targetPet == target)
                return true;
        }
        return false;
    }

    private void AddAffinity(PetAffinityProfile target, float value)
    {
        affinities.Add(new PetAffinity { targetPet = target, affinityValue = value });
    }

    public float GetAffinity(PetAffinityProfile target)
    {
        foreach (var affinity in affinities)
        {
            if (affinity.targetPet == target)
                return affinity.affinityValue;
        }
        return 0.5f; // Neutral if not found
    }
}
