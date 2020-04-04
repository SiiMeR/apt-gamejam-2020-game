using UnityEngine;
using UnityEngine.Tilemaps;

public class OurTile : MonoBehaviour
{
    public const int monthLength = 30;
    
    public bool isRiver;
    public bool isRoad;

    public float groundPollution = 0.05f;

    public int grass = 1000;
    public int rabbits = 100;
    public int foxes = 10;
    public Vector3Int positionInTilemap;

    public TileType type = TileType.GRASS;

    public void SetData(bool isRiver, bool isRoad, float groundPollution, int grasss, int rabbits, int foxes, TileType type)
    {
        this.isRiver = isRiver;
        this.isRoad = isRoad;
        this.groundPollution = groundPollution;
        this.grass = grasss;
        this.foxes = foxes;
        this.type = type;
    }
    
    
}
