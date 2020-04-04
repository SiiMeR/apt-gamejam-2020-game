using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
    [SerializeField] private Tilemap bgTilemap;
    [SerializeField] private Tilemap landTilemap;
    [SerializeField] private Tilemap riverTilemap;
    [SerializeField] private Tilemap roadTilemap;

    [SerializeField] private GameObject highLight;
    [SerializeField] private GameObject ourTilePrefab;

    [SerializeField] private TextMeshProUGUI nameField;
        
    public TextMeshProUGUI YText;
    public TextMeshProUGUI XText;
    public TextMeshProUGUI pollutionText;
    public TextMeshProUGUI grassText;
    public TextMeshProUGUI foxesText;
    public TextMeshProUGUI rabbitsText;
    public TextMeshProUGUI typeText;
        
                
    private List<Tile> _tiles;

    private void Awake()
    { 
        FillTilemap();
    }

    private void FillTilemap()
    {
        foreach (var tile in roadTilemap.GetTiles<Tile>())
        {
            var road = Instantiate(ourTilePrefab, transform);
            var ourTile = road.GetComponent<OurTile>();
            ourTile.SetData(false, true, 0.0f, 100, 10, 10, TileType.ROAD);
            ourTile.type = SpriteNameToEnum(GetTileFromTilemap(tile.Key).sprite);

                        
            roadTilemap.GetTile<Tile>(tile.Key).gameObject = road;
        }  
        foreach (var tile in riverTilemap.GetTiles<Tile>())
        {
            var road = Instantiate(ourTilePrefab, transform);
            road.GetComponent<OurTile>().SetData(true, false, 0.0f, 0,0,0, TileType.RIVER);
            riverTilemap.GetTile<Tile>(tile.Key).gameObject = road;
        }
                
        foreach (var tile in bgTilemap.GetTiles<Tile>())
        {
            var road = Instantiate(ourTilePrefab, transform);
            road.GetComponent<OurTile>().SetData(false, false, 0.0f, 100,10,10, TileType.GRASS);
            bgTilemap.GetTile<Tile>(tile.Key).gameObject = road;
        }
                
        foreach (var tile in landTilemap.GetTiles<Tile>())
        {
            var road = Instantiate(ourTilePrefab, transform);
            var ourTile = road.GetComponent<OurTile>();
            ourTile.SetData(false, false, 0.0f, 100,100,100, TileType.GRASS);
                        
                        
            ourTile.type = SpriteNameToEnum(GetTileFromTilemap(tile.Key).sprite);
            landTilemap.GetTile<Tile>(tile.Key).gameObject = road;
        }
    }

    private TileType SpriteNameToEnum(Sprite tile)
    {
        if (tile.name.Contains("Field"))
        {
            return TileType.FARMLAND;
        }
        if (tile.name.Contains("Forest"))
        {
            return TileType.FOREST;
        }
        if (tile.name.Contains("Mountain"))
        {
            return TileType.MOUNTAIN;
        }
        if (tile.name.Contains("Town"))
        {
            return TileType.VILLAGE;
        }
        if (tile.name.Contains("Road"))
        {
            return TileType.ROAD;
        }

        return TileType.GRASS;
    }




    public void OnTileClicked(OurTile tile, Vector3Int cellPos)
    {
        if (!tile)
        {
            Debug.Log($"Tile not found at position {cellPos}" );
            return;
        }
        
        XText.text = "X: " + cellPos.x;
        YText.text = "Y: " + cellPos.y;
                
        pollutionText.text = tile.groundPollution + "%";
        grassText.text = tile.grass.ToString();
        foxesText.text = tile.foxes.ToString();
        rabbitsText.text = tile.rabbits.ToString();
        typeText.text = tile.type.ToString();
    }

    private void Update()
    {
        // if (!Input.GetButtonDown(0)) return;
                
                
        var input = Input.mousePosition;
        input.z = 10.0f;
        var mousePos = Camera.main.ScreenToWorldPoint(input);

        var cellPos = bgTilemap.WorldToCell(mousePos);
                
        highLight.transform.position = bgTilemap.CellToWorld(cellPos) + new Vector3(2,2);
        // nameField.text = GetTileFromTilemap(cellPos)?.sprite?.name;
                
        OnTileClicked(GetTileAtPosition(cellPos), cellPos);
        // nameField.text = GetTileAtPosition(cellPos).type.ToString();
    }

    public OurTile GetTileAtPosition(Vector3Int cellPos)
    {
        return GetTileFromTilemap(cellPos)?.gameObject.GetComponent<OurTile>();
    }

    public OurTile GetRandomTileByType(params TileType[] tileTypes)
    {
        var tilesByType = GetTilesByType(tileTypes);
        return tilesByType.ElementAt(Random.Range(0, tilesByType.Count));
    }
        
    public List<OurTile> GetTilesByType(params TileType[] tileTypes)
    {
        return GetAllTilesFromTileMap()
            .Select(t => t.gameObject.GetComponent<OurTile>())
            .Where(tile => tileTypes.Contains(tile.type))
            .ToList();
    }

    public List<Tile> GetAllTilesFromTileMap()
    {
        return bgTilemap.GetTiles<Tile>().Select(p => p.Value).ToList();
    }
        
    public Tilemap GetBGTilemap()
    {
        return bgTilemap;
    }

    public Tilemap GetLandTilemap()
    {
        return landTilemap;
    }

    public Tilemap GetRoadTilemap()
    {
        return roadTilemap;
    }

    public Tilemap GetRiverTilemap()
    {
        return riverTilemap;
    }
        
    public Tile GetTileFromTilemap(Vector3Int cellPos)
    {
        var roadTile = roadTilemap.GetTile<Tile>(cellPos);
        if (roadTile)
        {
            return roadTile;
        }

        var riverTile = riverTilemap.GetTile<Tile>(cellPos);
        if (riverTile)
        {
            return riverTile;
        }
                
        var landTile = landTilemap.GetTile<Tile>(cellPos);
        if (landTile)
        {
            return landTile;
        }
                
        var grassTile = bgTilemap.GetTile<Tile>(cellPos);
        if (grassTile)
        {
            return grassTile;
        }
                
        return null;
    }
        
}