public class World : Singleton<World>
{
    private int currentSeason = 0;
    private int[] sunlight = {1000};

    public int GetSunlight()
    {
        return sunlight[currentSeason];
    }
}