using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MountainTile : AbstractTile
{
    [SerializeField] private List<Sprite> shapedMountain;
    
    public override TileType TypeOfTile { get; set; } = TileType.MOUNTAIN;

    public string originalSpriteName;
    
    private void Start()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        if (!originalSpriteName.StartsWith("mountain 1_"))
        {
            return;
        }
        spriteRenderer.sprite = shapedMountain.First(sprite => sprite.name == originalSpriteName);
    }
}