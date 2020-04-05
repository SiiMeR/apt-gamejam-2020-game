using System;
using System.Collections.Generic;
using System.Linq;
using DTO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventCreator : MonoBehaviour
{
    private List<Func<EventDTO>> events = new List<Func<EventDTO>>();
    private List<int> eventWeights = new List<int>();
    private bool isVallavanemAnswered = false;
    public Sprite pold;
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += OnDayEvent;
        
        AddEventToList(null,10);
        AddEventToList(TekstiiliTehas(), 1);
        AddEventToList(Ikaldus(), 1);
    }

    private void AddEventToList(Func<EventDTO> createEvent, int weight)
    {
        events.Add(createEvent);
        eventWeights.Add(weight);
    }

    private void RemoveEvent(Func<EventDTO> eventDto)
    {
        int index = events.FindIndex(e => e.Equals(eventDto));
        events.RemoveAt(index);
        eventWeights.RemoveAt(index);
    }
    
    private void ChangeEventWeight(Func<EventDTO> eventDto, int weight)
    {
        int index = events.FindIndex(e => e.Equals(eventDto));
        eventWeights[index] = weight;
    }

    private void OnDayEvent(int currentDay)
    {
        if (currentDay == 1)
        {
            EventDTO vallavanem = OledVallavanem();
            vallavanem.expires = false;
            EventManager.Instance.AddEvent(vallavanem);
            return;
        }
        if (isVallavanemAnswered && currentDay % 1 == 0)
        {
            int seek = UnityEngine.Random.Range(1, eventWeights.Sum() + 1);

            int sum = 0;
            for (int i = 0; i < eventWeights.Count; i++)
            {
                sum += eventWeights[i];
                if (sum >= seek && events[i] != null)
                {
                    EventManager.Instance.AddEvent(events[i].Invoke());
                    break;
                }
            }
        }
    }

    // Esimene event, ainult korra
    private EventDTO OledVallavanem()
    {
        return new EventDTO(
            "Vallavanem", 
            "Sinu ülesanne on hallata oma valda ja siinset loodust. Selleks pead hoidma tasakaalus inimeste tegevuse ja looduse elukäigu.",
            "Olen inimeste poolt",
            "Olen looduse poolt",
            () =>
            {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 1.05));
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.wellness * 1.05));
                foreach (var tile in OledVallavanemTiles())
                {
                    tile.groundPollution += 0.05f;
                }
                isVallavanemAnswered = true;
            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.95));
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.wellness * 0.95));
                foreach (var tile in OledVallavanemTiles())
                {
                    tile.groundPollution -= 0.05f;
                }
                isVallavanemAnswered = true;
            });
    }

    private List<AbstractTile> OledVallavanemTiles()
    {
        return TileManager.Instance.GetTilesByType(TileType.GRASS, TileType.FOREST, TileType.RIVER);
    }
    
    private Func<EventDTO> TekstiiliTehas()
    {
        return () => new EventDTO(
            "Tekstiilitehas", 
            "Ärimees tahab jõele ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                var grassTile = TileManager.Instance.GetRandomTileSidingWithGrassByType(SpriteFactory.Instance.factory, TileType.RIVER);
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 1.1));
                if (grassTile != null && grassTile.GetComponent<SpriteRenderer>() != null)
                {
                    grassTile.GetComponent<Animator>().enabled = false;  
                }
            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.9));
                // TODO remove pollution to nearby river
            },
            new Vector3Int());
    }
    
    private Func<EventDTO> Ikaldus()
    {
        // TODO: get random town tile
        // Add its coordinates to EventDTO
        // get route to another town
        
        return () => new EventDTO(
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
