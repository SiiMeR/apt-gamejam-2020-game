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

        foreach (var animator in FindObjectsOfType<Animator>())
        {
            if (animator.GetComponent<RiverTile>())
            {
                animator.speed = 0.85f;
                continue;
            }
            
            if (animator.GetComponent<FarmTile>())
            {
                animator.speed = 0.1f;
                var farmState = animator.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
                animator.Play(farmState.fullPathHash, -1, Random.Range(0f, 0.5f));
                continue;
            }
            
            animator.speed = Random.Range(0.15f, .25f);
            var state = animator.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
            animator.Play(state.fullPathHash, -1, Random.Range(0f, 0.1f));
        }
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

        switch (groundTile)
        {
            case RoadTile roadTile:
            {
                var roadNameToType = RoadNameToType(tile.sprite.name);
                roadTile.roadType = roadNameToType;
                roadTile.angle = (int) tilemap.GetTransformMatrix(pos).rotation.eulerAngles.z;
                break;
            }
            case RiverTile riverTile:
            {
                var riverNameToType = RiverNameToType(tile.sprite.name);
                riverTile.riverType = riverNameToType;
                riverTile.angle = (int) tilemap.GetTransformMatrix(pos).rotation.eulerAngles.z;
                break;
            }
            
            case MountainTile mountainTile:
                mountainTile.originalSpriteName = tile.sprite.name;
                break;
        }
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


    private RoadType RoadNameToType(string roadName)
    {
        switch (roadName.ToLowerInvariant())
        {
            case "path_cross":
                return RoadType.CROSS;
            case "path_horizontal":
                return RoadType.HORIZONTAL;
            case "path_t":
                return RoadType.TJUNCTION;
            case "path_vertical":
                return RoadType.VERTICAL;
            case "path_turn":
                return RoadType.CURVE;           
            case "bridge_horizontal":
                return RoadType.BRIDGE;
            
            default:
                return RoadType.HORIZONTAL;
        }    
    }
    
    private RiverType RiverNameToType(string riverName)
    {
        switch (riverName.ToLowerInvariant())
        {
            case "river_horizontal_0":
                return RiverType.HORIZONTAL;
            case "river_t_0":
                return RiverType.TJUNCTION;
            case "river_turn_0":
                return RiverType.CURVE;
            case "river_vertical_0":
                return RiverType.VERTICAL;
            default:
                return RiverType.VERTICAL;
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
        if (spriteName.ToLowerInvariant().Contains("path") || spriteName.ToLowerInvariant().Contains("bridge"))
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
        typeText.text = EstonianTranslationType(tile.TypeOfTile);
    }

    public string EstonianTranslationType(TileType type)
    {
        switch (type)
        {
            case TileType.UNKNOWN:
                return "Teadmata";
            case TileType.GRASS:
                return "Muru";
            case TileType.FARMLAND:
                return "Põld";
            case TileType.RIVER:
                return "Jõgi";
            case TileType.FOREST:
                return "Mets";
            case TileType.VILLAGE:
                return "Küla";
            case TileType.MOUNTAIN:
                return "Mägi";
            case TileType.ROAD:
                return "Tee";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }    
    }
    
    private void Update()
    {
        var input = Input.mousePosition;
        input.z = 10.0f;
        var mousePos = Camera.main.ScreenToWorldPoint(input);
        
        int x = (int) mousePos.x;
        int y = (int) mousePos.y;
        int z = (int) mousePos.z;

        x = x - x % 4;
        y = y - y % 4;
        
        if (x <= 0) x += -4;
        if (y <= 0) y += -4;
        
        var floored = new Vector3Int(x, y, z);
        highLight.transform.position = floored + new Vector3(2,2);
        //highLight.transform.position = floored;

        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // if (Physics.Raycast(ray, out var hit)) // UI CLiek, dondc work
        // {
        //     print(hit.transform.gameObject.name);
        //     if (hit.transform.GetComponent<RectTransform>() != null)
        //     {
        //         return;
        //     }
        // }

        if (Input.GetMouseButtonDown(0) && Time.timeScale > 0.0f)
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
            ? tile.LastOrDefault() 
            : null;
    }
    
    public GameObject GetGameObjectByPosition(Vector3Int position)
    {
        return (from values in _tiles.Values 
            where values != null 
                  && values.Any() 
                  && values.Last().transform.position.x.Equals(position.x) 
                  && values.Last().transform.position.y.Equals(position.y) 
            select values.Last()).FirstOrDefault();
    }
    
    public AbstractTile GetGameObjectByPosition(Vector3Int position, List<AbstractTile> values)
    {
        return values.FirstOrDefault(v =>
            transform.position.x.Equals(position.x) && v.transform.position.y.Equals(position.y));
    }
    
    public AbstractTile GetRandomTileSidingWithGrassByType(params TileType[] tileTypes)
    {
        var shuffledByType = GetTilesByType(tileTypes).OrderBy( x => Random.value).ToList();
        var grass = GetTilesByType(TileType.GRASS);

        foreach (var g in grass)
        {
            //print(g.transform.position.ToVector3Int());
        }
        
        foreach (var tile in shuffledByType)
        {
            var pos = tile.transform.position.ToVector3Int() * 4;
            pos.x -= 1;
            var at = GetGameObjectByPosition(pos, grass);
            
            if (at != null && at.TypeOfTile.Equals(TileType.GRASS))
            {
                return at.GetComponent<AbstractTile>();
            }

            pos.x += 2;
            at = GetGameObjectByPosition(pos, grass);
            if (at != null && at.TypeOfTile.Equals(TileType.GRASS))
            {
                return at.GetComponent<AbstractTile>();
            }

            pos.x -= 1;
            pos.y += 1;
            at = GetGameObjectByPosition(pos, grass);
            if (at != null && at.TypeOfTile.Equals(TileType.GRASS))
            {
                return at.GetComponent<AbstractTile>();
            }
            pos.y -= 2;
            at = GetGameObjectByPosition(pos, grass);
            if (at != null && at.TypeOfTile.Equals(TileType.GRASS))
            {
                return at.GetComponent<AbstractTile>();
            }
        }

        return null;
    }
}