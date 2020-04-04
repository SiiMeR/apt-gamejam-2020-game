using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : Singleton<TileManager>
{
        [SerializeField] private Tilemap bgTilemap;
        [SerializeField] private Tilemap landTilemap;
        [SerializeField] private Tilemap riverTilemap;
        [SerializeField] private Tilemap roadTilemap;

        private void Start()
        {
        }
}