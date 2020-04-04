using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileManager : Singleton<TileManager>
{
    public Tilemap bgTilemap;
    public Tilemap landTilemap;
    public Tilemap riverTilemap;
    public Tilemap roadTilemap;

    private Dictionary<Vector3Int, List<GameObject>> _tiles = new Dictionary<Vector3Int, List<GameObject>>();

    [SerializeField] private GameObject highLight;
    
    [SerializeField] private GameObject grassPrefab;
    [SerializeField] private GameObject forestPrefab;
    [SerializeField] private GameObject riverPrefab;
    [SerializeField] private GameObject roadPrefab;
    [SerializeField] private GameObject villagePrefab;
    [SerializeField] private GameObject farmPrefab;
    [SerializeField] private GameObject mountainPrefab;
    [SerializeField] private GameObject factoryPrefab;

    
    [SerializeField] private TextMeshProUGUI nameField;
        
    public TextMeshProUGUI YText;
    public TextMeshProUGUI XText;
    public TextMeshProUGUI pollutionText;
    public TextMeshProUGUI grassText;
    public TextMeshProUGUI foxesText;
    public TextMeshProUGUI rabbitsText;
    public TextMeshProUGUI typeText;

    public GameObject modal;
   
    private void Awake()
    { 
        FillTilemap();
    }

    private void FillTilemap()
    {
        var positions = bgTilemap.GetTilePositions();
        
        foreach (var pos in positions)
        {
            var tilesForPos = new List<GameObject>();
            
            if (CreateGroundTile(bgTilemap, pos, out var ourBg)) 
                tilesForPos.Add(ourBg.gameObject);
            if (CreateGroundTile(landTilemap, pos, out var ourLand)) 
                tilesForPos.Add(ourLand.gameObject);
            if (CreateGroundTile(riverTilemap, pos, out var ourRiver)) 
                tilesForPos.Add(ourRiver.gameObject);
            if (CreateGroundTile(roadTilemap, pos, out var ourRoad)) 
                tilesForPos.Add(ourRoad.gameObject);
                
            _tiles.Add(pos * 4, tilesForPos);
        }
    }

    private bool CreateGroundTile(Tilemap tilemap, Vector3Int pos, out AbstractTile groundTile)
    {
        var tile = tilemap.GetTile<Tile>(pos);
        if (!tile)
        {
            groundTile = null;
            return false;
        }
        
        groundTile = Instantiate(GetPrefab(tile.sprite), transform).GetComponent<AbstractTile>();
        // ourTile.SetData(false, false, 0.0f, 100, 100, 100, TileType.GRASS);
        // ourTile.type = NameToEnum(tile.sprite);
        groundTile.transform.position = bgTilemap.CellToWorld(pos) + new Vector3(2, 2, 0);
        
        tilemap.gameObject.SetActive(false);
        return true;
    }

    private GameObject GetPrefab(Sprite sprite)
    {
        switch (NameToEnum(sprite.name))
        {
            case TileType.GRASS:
                return grassPrefab;
            case TileType.FARMLAND:
                return farmPrefab;
            case TileType.RIVER:
                return riverPrefab;
            case TileType.FOREST:
                return forestPrefab;
            case TileType.VILLAGE:
                return villagePrefab;
            case TileType.MOUNTAIN:
                return mountainPrefab;
            case TileType.ROAD:
                return roadPrefab; 
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    private TileType NameToEnum(string spriteName)
    {
        if (spriteName.ToLowerInvariant().Contains("field"))
        {
            return TileType.FARMLAND;
        }
        if (spriteName.ToLowerInvariant().Contains("forest"))
        {
            return TileType.FOREST;
        }
        if (spriteName.ToLowerInvariant().Contains("mountain"))
        {
            return TileType.MOUNTAIN;
        }
        if (spriteName.ToLowerInvariant().Contains("town"))
        {
            return TileType.VILLAGE;
        }
        if (spriteName.ToLowerInvariant().Contains("road"))
        {
            return TileType.ROAD;
        }
        if (spriteName.ToLowerInvariant().Contains("river"))
        {
            return TileType.RIVER;
        }

        return TileType.GRASS;
    }




    public void OnTileClicked(AbstractTile tile, Vector3Int cellPos)
    {
        if (!tile)
        {
            // Debug.Log($"Tile not found at position {cellPos}" );
            return;
        }
        
        XText.text = "X: " + cellPos.x;
        YText.text = "Y: " + cellPos.y;
                
        pollutionText.text = tile.groundPollution + "%";
        grassText.text = tile.grass.ToString();
        foxesText.text = tile.foxes.ToString();
        rabbitsText.text = tile.rabbits.ToString();
        typeText.text = tile.TypeOfTile.ToString();
    }

    private void Update()
    {
                
                
        var input = Input.mousePosition;
        input.z = 10.0f;
        var mousePos = Camera.main.ScreenToWorldPoint(input);


        var floored = new Vector3Int((int) (mousePos.x - mousePos.x % 4), (int) (mousePos.y - mousePos.y % 4), (int) mousePos.z);
        highLight.transform.position = floored - new Vector3(2,2);
        // highLight.transform.position = floored;

        if (Input.GetMouseButtonDown(0))
        {
            modal.SetActive(!modal.activeInHierarchy);
        };

        OnTileClicked(GetTileAtPosition(floored), floored);
    }

    public AbstractTile GetTileAtPosition(Vector3Int cellPos)
    {
        return GetTileByPosition(cellPos)?.gameObject.GetComponent<AbstractTile>();
    }

    public AbstractTile GetRandomTileByType(params TileType[] tileTypes)
    {
        var tilesByType = GetTilesByType(tileTypes);
        return tilesByType.ElementAt(Random.Range(0, tilesByType.Count));
    }
        
    public List<AbstractTile> GetTilesByType(params TileType[] tileTypes)
    {
        return _tiles.Values.SelectMany(go => go)
            .Select(go => go.GetComponent<AbstractTile>())
            .Where(tile => tileTypes.Contains(tile.TypeOfTile))
            .ToList();
    }

    // public List<GameObject> GetAllTilesFromTileMap()
    // {
    //     
    //     return bgTilemap.GetTiles<Tile>().Select(p => p.Value).ToList();
    // }
    
    public GameObject GetTileByPosition(Vector3Int position)
    {
        return _tiles.TryGetValue(position, out var tile) 
            ? tile.Last() 
            : null;
    }
    
    public GameObject GetTileByPosition2(Vector3Int position)
    {
        Vector3Int pos = _tiles.Keys.FirstOrDefault(k => k.Equals(position));
        return pos == default ? null : _tiles[pos].Last();
    }
}