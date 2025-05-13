using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public enum InfluenceLayers
{
    HAPPINESS, 
    PLAYFULNESS,
    AFFINITY
}

public class InfluenceNode
{
    public Vector2Int location;

    // public InfluenceNode parentNode;
    public Dictionary<Influencer, float> influences = new Dictionary<Influencer, float>();

    // public float totalStrength; //TODO: add layers by making totalStrength private and getting it based on layer key
    public Dictionary<InfluenceLayers, float> totalStrength = new Dictionary<InfluenceLayers, float>();

    public InfluenceNode(Vector2Int location)
    {
        this.location = location;
        Initialize();
    }
    public void Initialize()
    {
        foreach (var layer in Enum.GetValues(typeof(InfluenceLayers)))
        {
            totalStrength.Add((InfluenceLayers)layer, 0);
        }
    }

    public float GetStrength(InfluenceLayers layer)
    {
        return totalStrength[layer];
    }

    public void UpdateStrength(InfluenceLayers layer, float strength)
    {
        totalStrength[layer] += strength;
    }
    // public bool visited;
}

// public class InfluenceHeap<T> : MaxHeap<T> where T : InfluenceNode
// {
//     public Vector2Int location;
//     public int totalStrength;
//
//     public void UpdateKeyContribution(T key, float newStrength)
//     {
//         if (key.strength > newStrength)
//         {
//             key.strength = newStrength;
//             IncreaseKey(key);
//         }else if (key.strength < newStrength)
//         {
//             key.strength = newStrength;
//             DecreaseKey(key);
//         }
//     }
//
//     public bool ContainsKey(Unit unit, InfluenceNode key)
//     {
//                 
//     }
//
//     public InfluenceHeap(Func<T, T, int> maxComparisonFunction, Vector2Int location) : base(maxComparisonFunction)
//     {
//         this.location = location;
//     }
// }

public class InfluenceMap : MonoBehaviour
{
    private int gridWidth;
    private int gridHeight;
    //use this instead of the grid. not in map, then influence below strength threshold

    private InfluenceNode[,] influenceGrid;
    public float strengthThreshold;

    // public List<LocationRecord> MapFloodDijkstra()
    // {
    //     PathfindingList open = new PathfindingList();
    //     PathfindingList closed = new PathfindingList();
    //     
    //     // List<LocationRecord> open = new List<LocationRecord>();
    //     // List<LocationRecord> closed = new List<LocationRecord>();
    //
    //     foreach (var unit in units)
    //     {
    //         LocationRecord startRecord = new LocationRecord();
    //         startRecord.location = unit.GetLocation();
    //         startRecord.nearestUnit = unit;
    //         startRecord.strength = unit.GetStrength();
    //         open.Add(startRecord);
    //     }
    //
    //     while (open.Count() > 0)
    //     {
    //         var current = open.LargestElement();
    //         var locations = GetNeighbors(current.location);
    //         foreach (var location in locations)
    //         {
    //             LocationRecord neighborRecord;
    //             float strength = StrengthFunction(current.nearestUnit, location.location);
    //             if (strength < strengthThreshold)
    //             {
    //                 continue;
    //             }
    //             else if (closed.Contains(location)) //TODO: not sure it makes sense to update already visited vertices
    //             {
    //                 neighborRecord = closed.Find(location);
    //                 if (neighborRecord.nearestUnit != current.nearestUnit && neighborRecord.strength < strength)
    //                 {
    //                     continue;
    //                 }
    //             }else if (open.Contains(location))
    //             {
    //                 neighborRecord = open.Find(location); //unsure why need to find location instead of just using location
    //                 if (neighborRecord.strength < strength)
    //                 {
    //                     continue;
    //                 }
    //             }
    //             else
    //             {
    //                 neighborRecord = new LocationRecord();
    //                 neighborRecord.location = location.location;
    //             }
    //             
    //             neighborRecord.nearestUnit = current.nearestUnit;
    //             neighborRecord.strength = strength;
    //
    //             if (!open.Contains(location))
    //             {
    //                 open.Add(neighborRecord);
    //             }
    //         }
    //
    //         open.Remove(current);
    //         closed.Add(current);
    //     }
    //     //pathfinding list with lowest cost element function
    //         //contains(node)
    //         //find(node)
    //         //reverse(path)
    //     throw new NotImplementedException();
    // }

    private List<InfluenceNode> GetNeighbors(InfluenceNode influencer)
    {
        var location = influencer.location;
        var heaps = new List<InfluenceNode>();
        if (location.x > 0)
        {
            heaps.Add(influenceGrid[location.x - 1, location.y]);
        }

        if (location.x < gridWidth - 1)
        {
            heaps.Add(influenceGrid[location.x + 1, location.y]);
        }

        if (location.y > 0)
        {
            heaps.Add(influenceGrid[location.x, location.y - 1]);
        }

        if (location.y < gridHeight - 1)
        {
            heaps.Add(influenceGrid[location.x, location.y + 1]);
        }

        return heaps;
    }

    public float StrengthFunction(Influencer influencer, Vector2Int worldToGrid)
    {
        Vector3Int gridLocation = new Vector3Int(worldToGrid.x, worldToGrid.y, 0);
        return influencer.GetStrength() /
               (1 + falloffStrength * Vector3.Distance(influencer.GetLocation(), gridLocation));
    }

    public void AddInfluencer(Influencer influencer)
    {
        units.Add(influencer);
        UpdateUnitInfluence(influencer);
        UpdateVisualization();

    }

    public void RemoveInfluencer(Influencer influencer)
    {
        influencer.ResetInfluences();
        units.Remove(influencer);
        UpdateVisualization();

    }
    
    public float falloffStrength = 2f;
    public GameObject influenceTextPrefab;
    public bool showInfluenceMap = true;
    public List<Influencer> units = new List<Influencer>();
    public Color positiveInfluenceColor = Color.green;
    public Color neutralInfluenceColor = Color.grey;

    private GameObject InfluenceTextParentHolder;

    private void Start()
    {
        gridHeight = GridMap.Instance.height;
        gridWidth = GridMap.Instance.width;
        units = GridMap.Instance.units;
        foreach (var unit in units)
        {
            if (unit is MovingInfluencerTest test)
            {
                test.influenceMap = this;
            }
        }

        influenceGrid = new InfluenceNode[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                influenceGrid[x, y] = new InfluenceNode  (new Vector2Int(x, y))/*, totalStrength = 0f }*/;
            }
        }

        CalculateInfluenceAllUnits();
    }

    public void ToggleInfluenceMap()
    {
        showInfluenceMap = !showInfluenceMap;
        CalculateInfluenceAllUnits();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInfluenceMap();
        }
    }

    // public int CompareInfluenceNodes(InfluenceNode t1, InfluenceNode t2)
    // {
    //     float diff = t1.totalStrength - t2.totalStrength;
    //     if (Mathf.Approximately(diff, 0))
    //     {
    //         return 0;
    //     }
    //
    //     if (diff > 0f)
    //     {
    //         return 1;
    //     }
    //     else
    //     {
    //         return -1;
    //     }
    // }

    public void UpdateUnitInfluence(Influencer influencer)
    {
        influencer.ResetInfluences();
        // add unit's new strength 
        HashSet<InfluenceNode> open = new HashSet<InfluenceNode>();
        var loc = GridMap.Instance.WorldToGrid(influencer.transform.position);
        var current = influenceGrid[loc.x, loc.y];
        open.Add(current);
        List<InfluenceNode> keys = new List<InfluenceNode> { current };
        UpdateStrength(influencer, current, open, keys, false);
        while (open.Count > 0)
        {
            current = keys[^1];
            keys.RemoveAt(keys.Count - 1);
            var neighbors = GetNeighbors(current);
            foreach (var neighbor in neighbors)
            {
                UpdateStrength(influencer, neighbor, open, keys);
            }

            open.Remove(current);
            // Debug.Log("hello");
        }
    }

    private void UpdateStrength(Influencer influencer, InfluenceNode neighbor, HashSet<InfluenceNode> open,
        List<InfluenceNode> keys, bool addOpen = true)
    {
        float strength = StrengthFunction(influencer, neighbor.location);
        bool contains = neighbor.influences.ContainsKey(influencer);
        // if (contains && Mathf.Approximately(strength, neighbor.influences[influencer]))
        // if (contains && Mathf.Approximately(strength, neighbor.influences[influencer]))
        if ((contains && neighbor.influences[influencer] >= strengthThreshold) || strength < strengthThreshold)
        {
            return;
        }

        //TODO: update influence in updateunit influence
        if (!contains)
        {
            neighbor.influences.Add(influencer, strength);
        }
        else
        {
            neighbor.influences[influencer] = strength;
        }
        influencer.influenced.Add(neighbor);
        // neighbor.totalStrength += strength;
        neighbor.UpdateStrength(influencer.myLayer, strength);
        if (addOpen && open.Add(neighbor))
        {
            keys.Add(neighbor);
        }
    }

    public void CalculateInfluenceAllUnits()
    {
        foreach (var unit in units)
        {
            UpdateUnitInfluence(unit);
        }

        UpdateVisualization();
    }
   
    public float minStrength;
    public float maxStrength;

    public void UpdateVisualization()
    {
        if (!showInfluenceMap)
        {
            GridMap.Instance.ResetTileColors(); // reset tile colors if we don't want to show the influence map
            return;
        }

        foreach (var tile in GridMap.Instance.GetTiles())
        {
            Vector2Int position = tile.Key;
            if (position.x >= 0 && position.x < gridWidth && position.y >= 0 && position.y < gridHeight)
            {
                float influence = Mathf.Clamp(influenceGrid[position.x, position.y].totalStrength[InfluenceLayers.HAPPINESS], minStrength,
                    maxStrength);
                float normalizedInfluence = influence / maxStrength;
                Color tileColor;
                if (showInfluenceMap)
                {
                    tileColor = Color.Lerp(neutralInfluenceColor, positiveInfluenceColor, normalizedInfluence);
                }
                else
                {
                    tileColor = neutralInfluenceColor;
                }

                tile.Value.GetComponent<Renderer>().material.color = tileColor;

                // Show 'w' or 'b'
                // UpdateInfluenceText(position, influence);
            }
        }
    }
    
    public float GetInfluenceAt(InfluenceLayers layer, Vector3 pos)
    {
        Vector2Int position = GridMap.Instance.WorldToGrid(pos);
        if (position.x < 0 || position.x >= gridWidth || position.y < 0 || position.y >= gridHeight)
            return 0;
        return influenceGrid[position.x, position.y].totalStrength[layer];
    }
}

