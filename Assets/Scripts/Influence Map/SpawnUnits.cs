using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SpawnUnits : MonoBehaviour
{
    [FormerlySerializedAs("playerUnitPrefabs")] public List<Influencer> petPrefabs;
    [FormerlySerializedAs("playerSpawnPositions")] public List<Vector2Int> petSpawnPositions;

    private GameObject UnitParent;
    private List<Influencer> spawnedUnits = new List<Influencer>();
    
    public List<Influencer> SpawnAllUnits()
    {
        UnitParent = new GameObject("Units");

        for (int i = 0; i < petPrefabs.Count; i++)
        {
            if (i < petSpawnPositions.Count)
            {
                SpawnUnit(petPrefabs[i], petSpawnPositions[i], true);
            }
        }

        return spawnedUnits;
    }

    private void SpawnUnit(Influencer influencerPrefab, Vector2Int spawnPosition, bool isPlayer)
    {
        var gridToWorld = GridMap.Instance.GridToWorld(spawnPosition);
        Vector3 worldPosition = gridToWorld;
        // worldPosition.z = -1;
        worldPosition.y = 1f;
        Influencer influencerInstance = Instantiate(influencerPrefab, worldPosition, Quaternion.identity);
        // InfluenceMap.Instance.units.Add(unitInstance);
       //init unit
       
       
        GridMap.Instance.UpdateUnitPosition(spawnPosition, spawnPosition, influencerInstance);
        
        //init ai

        influencerInstance.transform.SetParent(UnitParent.transform); // set the parent to TileParent for organization
        spawnedUnits.Add(influencerInstance);
    }
}
