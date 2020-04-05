using UnityEngine;

public static class GlobalStartingVariables
{
    private static int _airPollutionPercent = 40;
    public static int AirPollutionPercent
    {
        get => _airPollutionPercent;
        set => Mathf.Clamp(value, 0, 100);
    }

    private static int _startingPopulation = 1000;
    public static int StartingPopulation
    {
        get => _startingPopulation;
        set => Mathf.Max(0, value);
    }

    private static int _startingWellnessPercentPercent = 50;
    public static int StartingWellnessPercent
    {
        get => _startingWellnessPercentPercent;
        set => Mathf.Clamp(value, 0, 100);
    }

    private static int _startingFood = 5000;
    public static int StartingFood
    {
        get => _startingFood;
        set => Mathf.Max(0, value);
    }

    private static int _startingWood = 2000;
    public static int StartingWood
    {
        get => _startingWood;
        set => Mathf.Max(0, value);
    }

    private static int _startingCoal = 1000;
    public static int StartingCoal
    {
        get => _startingCoal;
        set => Mathf.Max(0, value);
    }
}