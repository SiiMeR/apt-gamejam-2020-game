using UnityEditor.UIElements;

public class Ecology : Singleton<Ecology>
{
    public const float logicalTurnLength = 7;
    public const float length = 30;

    public float grassLoss = 0.1f;
    public float grassGain = 0.1f;
    
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
        float grassDelta = - tile.grass * (dt * grassLoss);
        grassDelta += World.Instance.GetSunlight() * (dt * grassGain) * (1 - tile.groundPollution);

        tile.grass += (int) grassDelta;
    }

    void RabbitChange(Tile tile, float dt)
    {
        float grassDelta;
        float r = 1.0f;
        float K = tile.grass;
        float rabbitDelta = r*tile.rabbits * (1 - tile.rabbits / K);
    }

    void FoxChange(Tile tile, float dt)
    {
        
    }
}