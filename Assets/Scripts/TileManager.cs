using System;
using System.Collections.Generic;
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

        [SerializeField] private TextMeshProUGUI nameField;
        
        
        private List<Tile> _tiles;

        private void Awake()
        {
                // bgTilemap.
        }

        private void Start()
        {
                
        }

        private void Update()
        {
                var input = Input.mousePosition;
                input.z = 10.0f;
                var mousePos = Camera.main.ScreenToWorldPoint(input);

                var cellPos = bgTilemap.WorldToCell(mousePos);
                
                highLight.transform.position = bgTilemap.CellToWorld(cellPos) + new Vector3(2,2);
                nameField.text = GetTile(cellPos)?.sprite.name;

        }

        public UnityEngine.Tilemaps.Tile GetTile(Vector3Int cellPos)
        {
                var roadTile = roadTilemap.GetTile<UnityEngine.Tilemaps.Tile>(cellPos);
                if (roadTile)
                {
                        return roadTile;
                }

                var riverTile = riverTilemap.GetTile<UnityEngine.Tilemaps.Tile>(cellPos);
                if (riverTile)
                {
                        return riverTile;
                }
                
                var landTile = landTilemap.GetTile<UnityEngine.Tilemaps.Tile>(cellPos);
                if (landTile)
                {
                        return landTile;
                }
                
                var grassTile = bgTilemap.GetTile<UnityEngine.Tilemaps.Tile>(cellPos);
                if (grassTile)
                {
                        return grassTile;
                }
                
                return null;
        }
        
}