using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class InfluenceNode
{
    public Vector2Int location;
    // public InfluenceNode parentNode;
    public Dictionary<Influencer, float> influences = new Dictionary<Influencer, float>();
    public float totalStrength;
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
            heaps.Add(influenceGrid[location.x-1, location.y]);
        }

        if (location.x < gridWidth-1)
        {
            heaps.Add(influenceGrid[location.x+1, location.y]);
        }

        if (location.y > 0)
        {
            heaps.Add(influenceGrid[location.x, location.y-1]);
        }

        if (location.y < gridHeight - 1)
        {
            heaps.Add(influenceGrid[location.x, location.y+1]);
        }

        return heaps;
    }
    
    public float StrengthFunction(Influencer influencer, Vector2Int worldToGrid)
    {
        Vector3Int gridLocation = new Vector3Int(worldToGrid.x, worldToGrid.y, 0);
        return influencer.GetStrength() / (1 + falloffStrength * Vector3.Distance(influencer.GetLocation(), gridLocation));
    }

    public float falloffStrength = 2f;
    public GameObject influenceTextPrefab; 
    public bool showInfluenceMap = true;
    public List<Influencer> units = new List<Influencer>();
    private Dictionary<Vector2Int, GameObject> influenceTextObjects = new Dictionary<Vector2Int, GameObject>();
    public Color positiveInfluenceColor = Color.green;
    public Color neutralInfluenceColor = Color.grey;

    private GameObject InfluenceTextParentHolder;

    private void Awake()
    {
        InfluenceTextParentHolder = new GameObject("InfluenceTextParentHolder");
    }

    private void Start()
    {
        gridHeight = GridMap.Instance.height;
        gridWidth = GridMap.Instance.width;
        units = GridMap.Instance.units;
        influenceGrid = new InfluenceNode[gridWidth, gridHeight];
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                influenceGrid[x, y] = new InfluenceNode{ location= new Vector2Int(x, y), totalStrength = 0f};
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

    public int CompareInfluenceNodes(InfluenceNode t1, InfluenceNode t2)
    {
        float diff = t1.totalStrength - t2.totalStrength;
        if (Mathf.Approximately(diff, 0))
        {
            return 0;
        }

        if (diff > 0f)
        {
            return 1;
        }
        else
        {
            return -1;
        }
    }

    public void UpdateUnitInfluence(Influencer influencer)
    {
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
            Debug.Log("hello");
        }
    }

    private void UpdateStrength(Influencer influencer, InfluenceNode neighbor, HashSet<InfluenceNode> open, List<InfluenceNode> keys, bool addOpen = true)
    {
        float strength = StrengthFunction(influencer, neighbor.location);
        bool contains = neighbor.influences.ContainsKey(influencer);
        if (contains && Mathf.Approximately(strength, neighbor.influences[influencer]))
        {
            return;
        }
        
        if ( !neighbor.influences.TryAdd(influencer, strength))
        {
            float influence = neighbor.influences[influencer];
            neighbor.totalStrength -= influence;
            if (strength < strengthThreshold)
            {
                neighbor.influences.Remove(influencer);
            }
            else
            {
                neighbor.totalStrength += strength;
            }
        }
        else
        {
            neighbor.totalStrength += strength;
        }

        if (strength >= strengthThreshold && addOpen && open.Add(neighbor))
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
    //
    // private void ApplyInfluence(Vector2Int position)
    // {
    //     //TODO: apply influence to the tiles
    //     // Make sure to update these influences in the influenceGrid
    // }

    public float minStrength;
    public float maxStrength;
    
    private void UpdateVisualization()
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
                float influence = Mathf.Clamp(influenceGrid[position.x, position.y].totalStrength, minStrength, maxStrength);
                float normalizedInfluence = influence/maxStrength;
                Color tileColor;
                if (showInfluenceMap)
                {
                    tileColor = Color.Lerp(neutralInfluenceColor,positiveInfluenceColor, normalizedInfluence);
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

    private void UpdateInfluenceText(Vector2Int position, float influence)
    {
        if (!showInfluenceMap)
        {
            foreach (var obj in influenceTextObjects.Values)
            {
                obj.SetActive(false);
            }
            return;
        }

        string symbol = "";
        if (influence > 2) symbol = "W"; // player has advantage
        else if (influence < -2) symbol = "B"; // enemy has advantage

        if (!influenceTextObjects.ContainsKey(position))
        {
            GameObject textObj = Instantiate(influenceTextPrefab, GridMap.Instance.GridToWorld(position), Quaternion.identity);
            influenceTextObjects[position] = textObj;
            textObj.transform.SetParent(InfluenceTextParentHolder.transform); // set parent to keep hierarchy clean
        }

        influenceTextObjects[position].GetComponent<TextMeshPro>().text = symbol;
        influenceTextObjects[position].SetActive(!string.IsNullOrEmpty(symbol));
    }

    public float GetInfluenceAt(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridWidth || position.y < 0 || position.y >= gridHeight)
            return 0;
        return influenceGrid[position.x, position.y].totalStrength;
    }
}

/*
using UnityEngine;
using System.Collections.Generic;
using TMPro;

public class InfluenceMap : MonoBehaviour
{
    public static InfluenceMap Instance;
    private int gridWidth;
    private int gridHeight;
    private float[,] influenceGrid;
    public GameObject influenceTextPrefab; 
    public bool showInfluenceMap = true;

    private Dictionary<Vector2Int, GameObject> influenceTextObjects = new Dictionary<Vector2Int, GameObject>();


    private GameObject InfluenceTextParentHolder;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        InfluenceTextParentHolder = new GameObject("InfluenceTextParentHolder");

    }

    private void Start()
    {
        gridHeight = GridMap.Instance.height;
        gridWidth = GridMap.Instance.width;
        influenceGrid = new float[gridWidth, gridHeight];
        CalculateInfluence();      
    }

    public void ToggleInfluenceMap()
    {
        showInfluenceMap = !showInfluenceMap;
        CalculateInfluence();
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInfluenceMap();
        }
    }

    public void CalculateInfluence()
    {
        
        // reset influence grid
        for (int x = 0; x < gridWidth; x++)
        {
            for (int y = 0; y < gridHeight; y++)
            {
                influenceGrid[x, y] = 0;
            }
        }

        // apply influence from all units
        foreach (Unit unit in TurnManager.Instance.playerUnits)
        {
            ApplyInfluence(unit.gridPosition, 10); // positive influence for player
        }

        foreach (Unit unit in TurnManager.Instance.enemyUnits)
        {
            ApplyInfluence(unit.gridPosition, -10); // negative influence for enemy
        }

        UpdateVisualization();
    }

    private void ApplyInfluence(Vector2Int position, float baseInfluence)
    {
        //TODO: apply influence to the tiles
        // Make sure to update these influences in the influenceGrid
    }

    private void UpdateVisualization()
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
                float influence = Mathf.Clamp(influenceGrid[position.x, position.y], -10f, 10f);
                float normalizedInfluence = (influence + 10f) / 20f;
                Color tileColor;
                if (showInfluenceMap)
                {
                    tileColor = Color.Lerp(Color.black, Color.white, normalizedInfluence);
                }
                else
                {
                    tileColor = Color.white;
                }
                tile.Value.GetComponent<SpriteRenderer>().color = tileColor;

                // Show 'w' or 'b'
                UpdateInfluenceText(position, influence);
            }
        }
        
    }

    private void UpdateInfluenceText(Vector2Int position, float influence)
    {
        if (!showInfluenceMap)
        {
            foreach (var obj in influenceTextObjects.Values)
            {
                obj.SetActive(false);
            }
            return;
        }

        string symbol = "";
        if (influence > 2) symbol = "W"; // player has advantage
        else if (influence < -2) symbol = "B"; // enemy has advantage

        if (!influenceTextObjects.ContainsKey(position))
        {
            GameObject textObj = Instantiate(influenceTextPrefab, GridMap.Instance.GridToWorld(position), Quaternion.identity);
            influenceTextObjects[position] = textObj;
            textObj.transform.SetParent(InfluenceTextParentHolder.transform); // set parent to keep hierarchy clean
        }

        influenceTextObjects[position].GetComponent<TextMeshPro>().text = symbol;
        influenceTextObjects[position].SetActive(!string.IsNullOrEmpty(symbol));
    }

    public float GetInfluenceAt(Vector2Int position)
    {
        if (position.x < 0 || position.x >= gridWidth || position.y < 0 || position.y >= gridHeight)
            return 0;
        return influenceGrid[position.x, position.y];
    }
}

*/