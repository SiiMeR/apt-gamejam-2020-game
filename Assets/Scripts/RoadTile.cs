using System;
using UnityEngine;

public class RoadTile : AbstractTile
{
    [SerializeField] private Sprite horizontalSprite;
    [SerializeField] private Sprite verticalSprite;
    [SerializeField] private Sprite tJunctionSprite;
    [SerializeField] private Sprite curveSprite;
    [SerializeField] private Sprite crossSprite;
    [SerializeField] private Sprite bridgeSprite;
    
    public override TileType TypeOfTile { get; set; } = TileType.ROAD;
    public RoadType roadType;
    public int angle;

    private void Start()
    {
        var typeToSprite = TypeToSprite(roadType);
        GetComponent<SpriteRenderer>().sprite = typeToSprite;
        transform.Rotate(Vector3.forward, angle);
    }

    private Sprite TypeToSprite(RoadType type)
    {
        switch (type)
        {
            case RoadType.HORIZONTAL:
                return horizontalSprite;
            case RoadType.VERTICAL:
                return verticalSprite;
            case RoadType.TJUNCTION:
                return tJunctionSprite;
            case RoadType.CROSS:
                return crossSprite;
            case RoadType.CURVE:
                return curveSprite;
            case RoadType.BRIDGE:
                return bridgeSprite;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}


public enum RoadType
{
    HORIZONTAL,
    VERTICAL,
    TJUNCTION,
    CROSS,
    CURVE,
    BRIDGE
}