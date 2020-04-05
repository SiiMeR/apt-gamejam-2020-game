using System;
using System.Collections.Generic;
using UnityEditor.UIElements;

public class Ecology : Singleton<Ecology>
{
    public const float logicalTurnLength = 1;
    public const float length = 7;

    public float grassGain = 0.1f;

    public float rabbitR = 2.0f;
    public float rabbitK = 0.2f;

    public float foxesR = 0.3f;
    public float foxesK = 0.5f;
    
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += EcologialPassing;
    }

    void EcologialPassing(int currentDay)
    {
        if (currentDay % logicalTurnLength == 0)
        {
            var tiles = TileManager.Instance.GetTilesByType(TileType.ROAD,TileType.GRASS,TileType.RIVER,TileType.FOREST,TileType.UNKNOWN,TileType.VILLAGE,TileType.FARMLAND,TileType.MOUNTAIN);
            foreach (var tile in tiles)
            {
                TilePassing(tile);
            }
        }
    }

    void TilePassing(AbstractTile ourTile)
    {
        float dt = logicalTurnLength / length;
        GrassChange(ourTile, dt);
        RabbitChange(ourTile, dt);
        FoxChange(ourTile, dt);
    }

    void GrassChange(AbstractTile ourTile, float dt)
    {
        float grassDelta = World.Instance.GetSunlight() * (dt * grassGain) * (1 - ourTile.groundPollution);

        ourTile.grass += (int) grassDelta;
    }

    void RabbitChange(AbstractTile ourTile, float dt)
    {
        float r = rabbitR;
        float K = ourTile.grass*rabbitK;
        
        int rabbitsDelta = 0;
        for (int i = 0; i < ourTile.rabbits; i++)
        {
            if (UnityEngine.Random.Range(1, 100 + 1) < 100*(1-ourTile.rabbits/K))
                rabbitsDelta--;
            if (UnityEngine.Random.Range(1, 1000 + 1) == 1000)
                rabbitsDelta--;
        }
            
        for (int i = 0; i < (int) (0.7 * ourTile.rabbits / 2); i++)
            rabbitsDelta += UnityEngine.Random.Range(0, (int) (2 * r) + 1);

        int grassDelta = 0;
        for (int i = 0; i < ourTile.rabbits; i++)
            if (UnityEngine.Random.Range(1, 100 + 1) < 100 * rabbitK)
                grassDelta--;

        ourTile.rabbits = Math.Max(ourTile.rabbits - rabbitsDelta, 0);
        ourTile.grass = Math.Max(ourTile.grass - grassDelta, 0);
    }

    void FoxChange(AbstractTile ourTile, float dt)
    {
        float r = foxesR;
        float K = ourTile.rabbits*foxesK;
        
        int foxesDelta = 0;
        for (int i = 0; i < ourTile.foxes; i++)
        {
            if (UnityEngine.Random.Range(1, 100 + 1) < 100*(1-ourTile.foxes/K))
                foxesDelta--;
            if (UnityEngine.Random.Range(1, 1000 + 1) == 1000)
                foxesDelta--;    
        }

        for (int i = 0; i < (int) (0.5 * ourTile.foxes / 2); i++)
            foxesDelta += UnityEngine.Random.Range(0, (int) (2 * r) + 1);

        int rabbitsDelta = 0;
        for (int i = 0; i < ourTile.foxes; i++)
            if (UnityEngine.Random.Range(1, 100 + 1) < 100 * foxesK)
                rabbitsDelta--;

        ourTile.foxes = Math.Max(ourTile.foxes - foxesDelta, 0);
        ourTile.rabbits = Math.Max(ourTile.rabbits - rabbitsDelta, 0);
    }
        
}