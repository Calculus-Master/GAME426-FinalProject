using System.Collections.Generic;
using UnityEngine;

public class ToyAffinityProfile : MonoBehaviour
{
    [System.Serializable]
    public struct Affinity
    {
        public string itemName;
        [Range(0f, 1f)] public float affinityValue;
    }

    public List<Affinity> toyAffinities = new List<Affinity>();

    [Header("Available Toy Names")]
    public List<string> availableToys = new List<string>
    {
        "football",
        "basketball",
        "stripeball",
        "beachball",
        "bear",
        "bunny",
        "pinkrug",
        "blackrug"
    };

    void Awake()
    {
        if (toyAffinities.Count == 0)
        {
            GenerateRandomAffinities();
        }
    }

    void GenerateRandomAffinities()
    {
        toyAffinities.Clear();
        foreach (var toy in availableToys)
        {
            Affinity affinity = new Affinity
            {
                itemName = toy,
                affinityValue = Random.Range(0f, 1f)
            };
            toyAffinities.Add(affinity);
        }
    }

    public float GetAffinity(string itemName)
    {
        foreach (var affinity in toyAffinities)
        {
            if (affinity.itemName == itemName)
                return affinity.affinityValue;
        }
        return 0.5f; // Neutral if not found
    }
}
