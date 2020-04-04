using System;
using UnityEngine;

public class RiverTile : AbstractTile
{
    public override TileType TypeOfTile { get; set; } = TileType.RIVER;
    
    [SerializeField] private Sprite horizontalSprite;
    [SerializeField] private Sprite verticalSprite;
    [SerializeField] private Sprite tJunctionSprite;
    [SerializeField] private Sprite curveSprite;

    [SerializeField] private RuntimeAnimatorController horizontalAnimator;
    [SerializeField] private RuntimeAnimatorController verticalAnimator;
    [SerializeField] private RuntimeAnimatorController tJunctionAnimator;
    [SerializeField] private RuntimeAnimatorController curveAnimator;
    
    public RiverType riverType;
    public int angle;

    private void Start()
    {
        var typeToSprite = TypeToSprite(riverType);
        GetComponent<SpriteRenderer>().sprite = typeToSprite;
        GetComponent<Animator>().runtimeAnimatorController = TypeToAnimator(riverType);
        transform.Rotate(Vector3.forward, angle);
    }

    private Sprite TypeToSprite(RiverType type)
    {
        switch (type)
        {
            case RiverType.HORIZONTAL:
                return horizontalSprite;
            case RiverType.VERTICAL:
                return verticalSprite;
            case RiverType.TJUNCTION:
                return tJunctionSprite;
            case RiverType.CURVE:
                return curveSprite;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }   
    
    private RuntimeAnimatorController TypeToAnimator(RiverType type)
    {
        switch (type)
        {
            case RiverType.HORIZONTAL:
                return horizontalAnimator;
            case RiverType.VERTICAL:
                return verticalAnimator;
            case RiverType.TJUNCTION:
                return tJunctionAnimator;
            case RiverType.CURVE:
                return curveAnimator;
            default:
                throw new ArgumentOutOfRangeException(nameof(type), type, null);
        }
    }
}


public enum RiverType
{
    HORIZONTAL,
    VERTICAL,
    TJUNCTION,
    CURVE
}