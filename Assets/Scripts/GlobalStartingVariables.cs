using UnityEngine;

public static class GlobalStartingVariables
{
    private static int _airPollutionPercent = 40;
    public static int AirPollutionPercent
    {
        get => _airPollutionPercent;
        set => _airPollutionPercent = Mathf.Clamp(value, 0, 100);
    }

    private static int _startingPopulation = 1000;
    public static int StartingPopulation
    {
        get => _startingPopulation;
        set => _startingPopulation = Mathf.Max(0, value);
    }

    private static int _startingWellnessPercentPercent = 50;
    public static int StartingWellnessPercent
    {
        get => _startingWellnessPercentPercent;
        set => _startingWellnessPercentPercent = Mathf.Clamp(value, 0, 100);
    }

    private static int _startingFood = 2000;
    public static int StartingFood
    {
        get => _startingFood;
        set => _startingFood = Mathf.Max(0, value);
    }

    private static int _startingWood = 2000;
    public static int StartingWood
    {
        get => _startingWood;
        set => _startingWood = Mathf.Max(0, value);
    }

    private static int _startingCoal = 1000;
    public static int StartingCoal
    {
        get => _startingCoal;
        set => _startingCoal = Mathf.Max(0, value);
    }
}