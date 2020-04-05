using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class TileManager : Singleton<TileManager>
{
    public Tilemap bgTilemap;
    public Tilemap landTilemap;
    public Tilemap riverTilemap;
    public Tilemap roadTilemap;

    // Contains all tiles.
    // Key represents tile left-top anchor as a real-world location
    // Value is a list of tile layers. Each layer has a Grass tile as first layer.
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

    private bool _shouldShowModal = true;
    public bool ShouldShowModal
    {
        get => _shouldShowModal;
        set
        {
            if (!value)
            {
                modal.SetActive(false);
            }
            _shouldShowModal = value;
        }
    }
   
    private void Awake()
    {
        FillTilemap();

        foreach (var animator in FindObjectsOfType<Animator>())
        {
            if (animator.GetComponent<RiverTile>())
            {
                animator.speed = 0.8f;
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
        /**
         * MUUTA MAP VÄIKSEMAKS, SEST OSA JÄÄB KAAMERAST VÄLJA!!!
         * TODO:
         * TODO:
         */
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

            if (ourBg != null)
            {
                _tiles.Add(ourBg.transform.position.ToVector3Int(), tilesForPos);
            }
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
                
        pollutionText.text = (int) (tile.groundPollution * 100) + "%";
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
            case TileType.FACTORY:
                return "Tehas";
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }    
    }
    
    private void Update()
    {
        // Don't interact with map when time has been stopped
        if (Math.Abs(Time.timeScale) <= 0.0f)
        {
            modal.SetActive(false);
            return;
        }
        var input = Input.mousePosition;
        input.z = 10.0f;
        var mousePos = Camera.main.ScreenToWorldPoint(input);

        var rawX = mousePos.x;
        var rawY = mousePos.y;
        var rawZ = mousePos.z;

        if (rawX < 0) rawX -= 4;
        // if (intY < 0) intY -= 4;
        
        int intX = (int) rawX;
        int intY = (int) rawY;
        int intZ = (int) rawZ;

        int moduloX = intX % 4;
        int moduloY = intY % 4;

        int tileX = intX - moduloX;
        int tileY = intY - moduloY;

        int x = tileX;
        int y = tileY;
        int z = intZ;
        
//        Debug.Log($"raw: ({rawX}, {rawY}); int: ({intX}, {intY}); modulo: ({moduloX}, {moduloY}); tile: ({tileX}, {tileY}); final: ({x}, {y})");
        
        var floored = new Vector3Int(x, y, z);
        floored += new Vector3Int(2, -2, 0);
        
        highLight.transform.position = floored;

        var tileAtPosition = GetTileAtPosition(floored);
        OnTileClicked(tileAtPosition, floored);
        
        if (!tileAtPosition)
        {
            modal.SetActive(false);
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                ShouldShowModal = !ShouldShowModal;
            }

            if (ShouldShowModal && !modal.active)
            {
                modal.SetActive(true);
            }
        }
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
    
    public void RemoveTileByPosition(Vector3Int position)
    {
        if (_tiles.ContainsKey(position) && _tiles[position].Any())
        {
            var values = _tiles[position];
            _tiles[position][_tiles[position].Count - 1].GetComponent<SpriteRenderer>().enabled = false;
            values.RemoveAt(_tiles[position].Count - 1);
            _tiles[position] = values;
        }
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

    public AbstractTile UpdateRandomTileSidingWithGrassByType(GameObject goPrefab, params TileType[] tileTypes)
    {
        var shuffledByType = GetTilesByType(tileTypes).OrderBy( x => Random.value).ToList();

        foreach (var tile in shuffledByType)
        {
            var pos = tile.transform.position.ToVector3Int();
            var xList = new List<int>() {pos.x - 4, pos.x + 4};
            var yList = new List<int>() {pos.y - 4, pos.y + 4};

            foreach (var xPos in xList)
            {
                foreach (var yPos in yList)
                {
                    var tilePos = new Vector3Int(xPos, yPos, pos.z);
                    var at = GetGameObjectByPosition(tilePos);
                    
                    if (at == null || at.GetComponent<AbstractTile>() == null || !at.GetComponent<AbstractTile>().TypeOfTile.Equals(TileType.GRASS)) continue;
                    if (!_tiles.ContainsKey(tilePos)) continue;
                    
                    var go = Instantiate(goPrefab, tilePos, Quaternion.identity);
                    _tiles[tilePos].Add(go);
                    return go.GetComponent<AbstractTile>();
                }
            }
        }
        return null;
    }

    // Returns all tile layers in some radius to current location
    public Dictionary<Vector3Int, List<GameObject>> getTilesInRadius(AbstractTile tile, int radius, bool includeSelf = false)
    {
        var tilePosition = tile.transform.position;
        var realRadius = radius * 4;
        return _tiles
            .Where(elem =>
            {
                var xPred = (tilePosition.x - realRadius) <= elem.Key.x && elem.Key.x <= (tilePosition.x + realRadius);
                var yPred = (tilePosition.y - realRadius) <= elem.Key.y && elem.Key.y <= (tilePosition.y + realRadius);
                var isSelf = elem.Key.x == tilePosition.x && elem.Key.y == tilePosition.y;

                if (xPred && yPred)
                {
                    Debug.Log($"x: {xPred}; y: {yPred}; isSelf: {isSelf};");
                }
                return xPred && yPred && (includeSelf || !isSelf);

            }).ToDictionary(pair => pair.Key, pair => pair.Value);
    }
}