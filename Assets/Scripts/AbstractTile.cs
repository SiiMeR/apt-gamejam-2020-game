using System;
using UnityEngine;
using Random = UnityEngine.Random;

public abstract class AbstractTile : MonoBehaviour
{
    public float groundPollution = 0.2f;
    public float airPollution = 0.1f;
    public int grass = 1000;
    public int rabbits = 100;
    public int foxes = 10;

    public virtual TileType TypeOfTile { get; set; } = TileType.UNKNOWN;
    
}