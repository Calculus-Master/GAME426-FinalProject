using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GridMap : MonoBehaviour
{
    public static GridMap Instance;
    public int width = 20;
    public int height = 20;
    public List<Influencer> units;
    public float cellSize = 1.0f;
    public GameObject tilePrefab;
    private Dictionary<Vector2Int, GameObject> gridTiles = new Dictionary<Vector2Int, GameObject>();
    private GameObject TileParent;


    public Vector2Int GetRandomGridLocation()
    {
        return gridTiles.Keys.ElementAt(UnityEngine.Random.Range(0, gridTiles.Count));
    }
    //public GameObject playerPrefab;
    // public GameObject enemyPrefab;

    public Dictionary<Vector2Int, GameObject> GetTiles()
    {
        return gridTiles;
    }

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

        TileParent = new GameObject("TileParent");

        GenerateGrid();
        SpawnUnits();
    }

    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                GameObject tile = Instantiate(tilePrefab, GridToWorld(gridPos), Quaternion.identity);
                tile.layer = LayerMask.NameToLayer("canPlace");
                gridTiles[gridPos] = tile;
                tile.transform.parent = TileParent.transform; // Set the parent to TileParent
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x / cellSize),
            Mathf.RoundToInt(worldPosition.y / cellSize));
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0);
    }

    public bool IsValidTile(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void HighlightTile(Vector2Int position, Color color)
    {
        if (gridTiles.ContainsKey(position))
        {
            //Debug.Log(gridTiles[position].transform.position.x);
            gridTiles[position].GetComponent<SpriteRenderer>().color = Color.green;
        }
    }


    public void ResetTileColors()
    {
        foreach (var tile in gridTiles.Values)
        {
            tile.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    
    private Dictionary<Vector2Int, Influencer> unitPositions = new Dictionary<Vector2Int, Influencer>();
    //
    public bool IsTileOccupied(Vector2Int position)
    {
        return unitPositions.ContainsKey(position);
    }
    
    public void UpdateUnitPosition(Vector2Int oldPosition, Vector2Int newPosition, Influencer influencer)
    {
        if (unitPositions.ContainsKey(oldPosition))
        {
            unitPositions.Remove(oldPosition);
        }
    
        unitPositions[newPosition] = influencer;
    }
    //
    public Influencer GetUnitAtPosition(Vector2Int position)
    {
        unitPositions.TryGetValue(position, out Influencer unit);
        return unit;
    }
    
    public SpawnUnits spawnUnits;
    
    public void SpawnUnits()
    {
        units = spawnUnits.SpawnAllUnits();
    }
}
/*
using UnityEngine;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{
    public static GridManager Instance;
    public int width = 20;
    public int height = 20;
    public float cellSize = 1.0f;
    public GameObject tilePrefab;
    private Dictionary<Vector2Int, GameObject> gridTiles = new Dictionary<Vector2Int, GameObject>();

    private GameObject TileParent;


    //public GameObject playerPrefab;
   // public GameObject enemyPrefab;

    public Dictionary<Vector2Int, GameObject> GetTiles()
    {
        return gridTiles;
    }

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

        TileParent = new GameObject("TileParent");

        GenerateGrid();

    }

    void Start()
    {
        SpawnUnits();
    }


    private void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int gridPos = new Vector2Int(x, y);
                GameObject tile = Instantiate(tilePrefab, GridToWorld(gridPos), Quaternion.identity);
                gridTiles[gridPos] = tile;
                tile.transform.parent = TileParent.transform; // Set the parent to TileParent
            }
        }
    }

    public Vector2Int WorldToGrid(Vector3 worldPosition)
    {
        return new Vector2Int(Mathf.RoundToInt(worldPosition.x / cellSize), Mathf.RoundToInt(worldPosition.y / cellSize));
    }

    public Vector3 GridToWorld(Vector2Int gridPosition)
    {
        return new Vector3(gridPosition.x * cellSize, gridPosition.y * cellSize, 0);
    }

    public bool IsValidTile(Vector2Int position)
    {
        return position.x >= 0 && position.x < width && position.y >= 0 && position.y < height;
    }

    public void HighlightTile(Vector2Int position, Color color)
    {
        if (gridTiles.ContainsKey(position))
        {
            //Debug.Log(gridTiles[position].transform.position.x);
            gridTiles[position].GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private Dictionary<Vector2Int, Unit> unitPositions = new Dictionary<Vector2Int, Unit>();

    public bool IsTileOccupied(Vector2Int position)
    {
        return unitPositions.ContainsKey(position);
    }

    public void UpdateUnitPosition(Vector2Int oldPosition, Vector2Int newPosition, Unit unit)
    {
        if (unitPositions.ContainsKey(oldPosition))
        {
            unitPositions.Remove(oldPosition);
        }
        unitPositions[newPosition] = unit;
    }


    public void ResetTileColors()
    {
        foreach (var tile in gridTiles.Values)
        {
            tile.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    public Unit GetUnitAtPosition(Vector2Int position)
    {
        unitPositions.TryGetValue(position, out Unit unit);
        return unit;
    }

    public SpawnUnits spawnUnits;
    public void SpawnUnits()
    {
        spawnUnits.SpawnAllUnits();
    }

}

*/