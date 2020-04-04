using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using UnityEngine;

public class EventCreator : MonoBehaviour
{
    private List<EventDTO> events = new List<EventDTO>();
    private List<int> eventWeights = new List<int>();

    public Sprite pold;
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += OnDayEvent;
        
        AddEventToList(null,10);
        AddEventToList(TekstiiliTehas(), 1);
        AddEventToList(Ikaldus(), 1);
    }

    private void AddEventToList(EventDTO eventDto, int weight)
    {
        events.Add(eventDto);
        eventWeights.Add(weight);
    }

    private void RemoveEvent(EventDTO eventDto)
    {
        int index = events.FindIndex(e => e.Equals(eventDto));
        events.RemoveAt(index);
        eventWeights.RemoveAt(index);
    }
    
    private void ChangeEventWeight(EventDTO eventDto, int weight)
    {
        int index = events.FindIndex(e => e.Equals(eventDto));
        eventWeights[index] = weight;
    }

    private void OnDayEvent(int currentDay)
    {
        if (currentDay % 1 == 0)
        {
            int seek = UnityEngine.Random.Range(1, eventWeights.Sum() + 1);

            int sum = 0;
            for (int i = 0; i < eventWeights.Count; i++)
            {
                sum += eventWeights[i];
                if (sum >= seek && events[i] != null)
                {
                    EventManager.Instance.AddEvent(events[i]);
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
        var ourTile = TileManager.Instance.GetRandomTileByType(TileType.GRASS);

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
            },
            new Vector3Int());
    }
    
    private EventDTO Ikaldus()
    {
        // TODO: get random town tile
        // Add its coordinates to EventDTO
        // get route to another town
        
        return new EventDTO(
            "Ikaldus", 
            "Külaelanike põlde tabab ikaldus ja inimesed on näljas.",
            "Las nälgivad",
            "Söögu jäneseid",
            () =>
            {
                CountyProperties.Instance.SetFood(Math.Max(CountyProperties.Instance.food - 200, 0));
                // TODO: Alternatiivselt, lõhu põllud.
            },
            () =>
            {
                CountyProperties.Instance.SetFood(Math.Max(CountyProperties.Instance.food + 100, 0));
                // TODO: Tapa jäneseid lähedastes alades.
            });
    }
    
    private EventDTO LinnadevahelineTee()
    {
        // TODO: get random town tile
        // Add its coordinates to EventDTO
        // get route to another town
        var tile = TileManager.Instance.GetRandomTileByType(TileType.RIVER);
        
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
               // CountyProperties.Instance.wellness -= 5;
            },
            Vector3Int.down);
    }
    
}
