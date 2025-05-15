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

        /// Adjusts the affinity toward a target pet by a delta amount, clamped between 0 and 1.
    public void AdjustAffinity(PetAffinityProfile target, float delta)
    {
        for (int i = 0; i < affinities.Count; i++)
        {
            if (affinities[i].targetPet == target)
            {
                float newValue = Mathf.Clamp01(affinities[i].affinityValue + delta);
                affinities[i] = new PetAffinity { targetPet = target, affinityValue = newValue };
                return;
            }
        }

        /// If affinity does not exist, add a neutral starting point plus delta.
        AddAffinity(target, Mathf.Clamp01(0.5f + delta));
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
