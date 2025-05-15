using UnityEngine;

public class PetSpawnerButton : MonoBehaviour
{
    public GameObject petPrefab;
    public Transform spawnPoint;
    public Mesh[] availableMeshes;
    public Material[] availableMaterials;

    private int spawnCount = 0;
    private const int maxSpawns = 4;

    public void SpawnPet()
    {
        if (spawnCount >= maxSpawns)
        {
            Debug.Log("Maximum pet spawn limit reached.");
            return;
        }

        GameObject newPet = Instantiate(petPrefab, spawnPoint.position, spawnPoint.rotation);
        CustomizePet(newPet);
        spawnCount++;
    }

    private void CustomizePet(GameObject petObject)
    {
        PetEntity pet = petObject.GetComponent<PetEntity>();
        if (pet == null)
        {
            Debug.LogError("Spawned object does not have PetEntity.");
            return;
        }

        // Assign random name
        pet.name = "Pet_" + Random.Range(1000, 9999);

        // Randomize needs rates
        pet.hungerDecayRate = Random.Range(0.05f, 0.2f);
        pet.thirstDecayRate = Random.Range(0.02f, 0.1f);
        pet.energyDecayRate = Random.Range(0.05f, 0.15f);
        pet.socialDecayRate = Random.Range(0.05f, 0.15f);

        // Assign random mesh
        if (availableMeshes.Length > 0)
        {
            MeshFilter meshFilter = petObject.GetComponentInChildren<MeshFilter>();
            if (meshFilter != null)
            {
                meshFilter.mesh = availableMeshes[Random.Range(0, availableMeshes.Length)];
            }
        }

        // Assign random color/material
        if (availableMaterials.Length > 0)
        {
            MeshRenderer renderer = petObject.GetComponentInChildren<MeshRenderer>();
            if (renderer != null)
            {
                renderer.material = availableMaterials[Random.Range(0, availableMaterials.Length)];
            }
        }
    }
}
