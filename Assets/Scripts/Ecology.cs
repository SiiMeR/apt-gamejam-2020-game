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

    void TilePassing(OurTile ourTile)
    {
        GrassChange(ourTile, logicalTurnLength / length);
    }

    void GrassChange(OurTile ourTile, float dt)
    {
        float grassDelta = World.Instance.GetSunlight() * (dt * grassGain) * (1 - ourTile.groundPollution);

        ourTile.grass += (int) grassDelta;
    }

    void RabbitChange(OurTile ourTile, float dt)
    {
        float r = rabbitR; // Kui palju jänes soovib paljuneda
        float K = ourTile.grass*rabbitK; // Kui palju jänes saab paljuneda
        float rabbitsDelta = r*ourTile.rabbits * (1 - ourTile.rabbits / K) * dt; // Kui palju jäneste arv muutub
        
        float grassDelta = - Math.Max(rabbitsDelta * rabbitK,0); // Kui palju nad rohtu söövad

        ourTile.rabbits += (int) rabbitsDelta;
        ourTile.grass += (int) grassDelta;
    }

    void FoxChange(OurTile ourTile, float dt)
    {
        float r = foxesR; // Kui palju jänes soovib paljuneda
        float K = ourTile.rabbits*foxesK; // Kui palju jänes saab paljuneda
        float foxesDelta = r*ourTile.foxes * (1 - ourTile.foxes / K) * dt; // Kui palju jäneste arv muutub
        
        float rabbitsDelta = - Math.Max(foxesDelta * foxesK,0); // Kui palju nad rohtu söövad

        ourTile.foxes += (int) foxesDelta;
        ourTile.rabbits += (int) rabbitsDelta;
    }
        
}