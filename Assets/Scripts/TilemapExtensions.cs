using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static class TilemapExtensions
{
    public static Dictionary<Vector3Int, T> GetTiles<T>(this Tilemap tilemap) where T : TileBase
    {
        var tiles = new Dictionary<Vector3Int, T>();

        var y = tilemap.origin.y;
        for (; y < (tilemap.origin.y + tilemap.size.y); y++)
        {
            for (var x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                var vector3Int = new Vector3Int(x, y, 0);
                var tile = tilemap.GetTile<T>(vector3Int);
                tiles.Add(vector3Int, tile);
            }
        }

        return tiles;
    }   
    
    public static List<Vector3Int> GetTilePositions(this Tilemap tilemap)
    {
        var tiles = new List<Vector3Int>();

        var y = tilemap.origin.y;
        for (; y < (tilemap.origin.y + tilemap.size.y); y++)
        {
            for (var x = tilemap.origin.x; x < (tilemap.origin.x + tilemap.size.x); x++)
            {
                var vector3Int = new Vector3Int(x, y, 0);
                tiles.Add(vector3Int);
            }
        }

        return tiles;
    }
}