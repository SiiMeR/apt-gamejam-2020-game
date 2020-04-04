using System.Collections.Generic;
using DTO;
using UnityEngine;

public class EventCreator : MonoBehaviour
{
    private List<EventDTO> events = new List<EventDTO>();
    private List<int> eventWeights = new List<int>();


    public Sprite pold;
    
    // TODO: Variant luua list numbridest, ja need randomilt eventile määrata et mis päevadel event tuleb
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += OnDayEvent;
        
        events.Add(null);
        eventWeights.Add(10);
        events.Add(TekstiiliTehas());
        eventWeights.Add(1);
    }

    private void OnDayEvent(int currentDay)
    {
        // TODO: add randomness for event creation

        
        
        if (currentDay % 1 == 0)
        {
            int sum = 0;
            foreach (int weight in eventWeights)
            {
                sum += weight;
            }

            int seek = UnityEngine.Random.Range(1, sum+1);

            sum = 0;
            for (int i = 0; i < eventWeights.Count; i++)
            {
                sum += eventWeights[i];
                if (sum >= seek)
                {
                    if (events[i] != null) EventManager.Instance.AddEvent(events[i]);
                    break;
                }
            }
        }
    }

    private EventDTO TekstiiliTehas()
    {
        // TODO: get random river tile
        // Add its coordinates to EventDTO
        // get river tiles nearby
        OurTile ourTile = TileManager.Instance.GetRandomTileByType(TileType.GRASS);
        Debug.Log(ourTile.positionInTilemap);
        
        return new EventDTO(
            "Teksiilitehas", 
            "Ärimees tahab jõele ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 1.1));
                // TODO add pollution to nearby river
            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.9));
                // TODO remove pollution to nearby river

                var tile = TileManager.Instance.GetTileFromTilemap(ourTile.positionInTilemap);
                tile.sprite = pold;
                print(ourTile.positionInTilemap);
                TileManager.Instance.roadTilemap.SetTile(ourTile.positionInTilemap, tile);
                TileManager.Instance.roadTilemap.RefreshTile(ourTile.positionInTilemap);
                
            },
            ourTile.positionInTilemap);
    }
    
    private EventDTO LinnadevahelineTee()
    {
        // TODO: get random town tile
        // Add its coordinates to EventDTO
        // get route to another town
        OurTile tile = TileManager.Instance.GetRandomTileByType(TileType.RIVER);
        
        return new EventDTO(
            "Linnadevaheline tee", 
            "Asulate vahele on vaja otsemat teed, vana tee on liiga pikk. Selleks on vaja maha võtta ka veidi metsa. Kuidas toimid?",
            "Luba tee-ehitus",
            "Keela tee-ehitus",
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness + 5);
            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 5);
            },
            tile.positionInTilemap);
    }
    
}
