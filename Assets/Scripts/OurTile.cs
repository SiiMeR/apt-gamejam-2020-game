using UnityEngine;
using UnityEngine.Tilemaps;

public class OurTile : MonoBehaviour
{
    public bool isRiver;
    public bool isRoad;

    public float groundPollution = 0.05f;
    public float airPollution = 0.05f;

    public int grass = 1000;
    public int rabbits = 100;
    public int foxes = 10;
    public Vector3Int positionInTilemap;
    public Tile tile;

    public TileType type = TileType.GRASS;

    public void SetData(bool isRiver, bool isRoad, float groundPollution, int grass, int rabbits, int foxes, TileType type)
    {
        this.isRiver = isRiver;
        this.isRoad = isRoad;
        this.groundPollution = groundPollution;
        this.grass = grass;
        this.rabbits = rabbits;
        this.foxes = foxes;
        this.type = type;
    }
    
    
}
