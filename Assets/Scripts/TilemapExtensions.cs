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
                if (tile != null)
                {
                    tiles.Add(vector3Int, tile);
                }
            }
        }

        return tiles;
    }
}