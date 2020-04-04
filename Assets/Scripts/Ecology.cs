using System;
using UnityEditor.UIElements;

public class Ecology : Singleton<Ecology>
{
    public const float logicalTurnLength = 7;
    public const float length = 30;

    public float grassGain = 0.1f;

    public float rabbitR = 1.0f;
    public float rabbitK = 0.1f;

    public float foxesR = 0.2f;
    public float foxesK = 0.1f;
    
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += EcologialPassing;
    }

    void EcologialPassing(int currentDay)
    {
        if (currentDay % logicalTurnLength == 0)
        {
            //tilePassing(tile);
        }
    }

    void TilePassing(Tile tile)
    {
        GrassChange(tile, logicalTurnLength / length);
    }

    void GrassChange(Tile tile, float dt)
    {
        float grassDelta = World.Instance.GetSunlight() * (dt * grassGain) * (1 - tile.groundPollution);

        tile.grass += (int) grassDelta;
    }

    void RabbitChange(Tile tile, float dt)
    {
        float r = rabbitR; // Kui palju jänes soovib paljuneda
        float K = tile.grass*rabbitK; // Kui palju jänes saab paljuneda
        float rabbitsDelta = r*tile.rabbits * (1 - tile.rabbits / K) * dt; // Kui palju jäneste arv muutub
        
        float grassDelta = - Math.Max(rabbitsDelta * rabbitK,0); // Kui palju nad rohtu söövad

        tile.rabbits += (int) rabbitsDelta;
        tile.grass += (int) grassDelta;
    }

    void FoxChange(Tile tile, float dt)
    {
        float r = foxesR; // Kui palju jänes soovib paljuneda
        float K = tile.rabbits*foxesK; // Kui palju jänes saab paljuneda
        float foxesDelta = r*tile.foxes * (1 - tile.foxes / K) * dt; // Kui palju jäneste arv muutub
        
        float rabbitsDelta = - Math.Max(foxesDelta * foxesK,0); // Kui palju nad rohtu söövad

        tile.foxes += (int) foxesDelta;
        tile.rabbits += (int) rabbitsDelta;
    }
        
}