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
        AddEventToList(Metsaraie(), 1);
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
        //EventManager.Instance.AddEvent(Metsaraie().Invoke());
        
        if (currentDay == 1)
        {
            EventDTO vallavanem = OledVallavanem();
            vallavanem.expires = false;
            EventManager.Instance.AddEvent(vallavanem);
            return;
        }
        var eventChance = UnityEngine.Random.Range(currentDay - 1, currentDay + 2); //33% chance
        if (isVallavanemAnswered && currentDay == eventChance)
        {
            int randomEvent = UnityEngine.Random.Range(1, events.Count + 1);
            if (events[randomEvent] != null)
            {
                EventManager.Instance.AddEvent(events[randomEvent].Invoke());
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
            "Ärimees tahab jõe kõrvale ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                TileManager.Instance.UpdateRandomTileSidingWithGrassByType(SpriteFactory.Instance.factory, TileType.RIVER);
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 1.1));
            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.9));
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.wellness * 0.95));
            });
    }
    
    private Func<EventDTO> Ikaldus()
    {
        return () => new EventDTO(
            "Ikaldus", 
            "Külaelanike põlde tabab ikaldus ja inimesed on näljas.",
            "Las nälgivad",
            "Söögu jäneseid",
            () =>
            {
                CountyProperties.Instance.SetFood(Math.Max(CountyProperties.Instance.food - 200, 0));
            },
            () =>
            {
                CountyProperties.Instance.SetFood(Math.Max(CountyProperties.Instance.food + 100, 0)); 
            });
    }
    
    private Func<EventDTO> Metsaraie()
    {
        return () => new EventDTO(
            "Metsaraie", 
            "Külaelanikel on vaja talve jaoks koguda küttepuid. Kas lubad neil raiuda metsa?",
            "Jah",
            "Ei",
            () =>
            {
                AbstractTile tile = TileManager.Instance.GetRandomTileByType(TileType.FOREST);
                print(tile.transform.position.ToVector3Int());
                CountyProperties.Instance.SetWood(CountyProperties.Instance.wood + 50);
                if (tile != null)
                {
                    TileManager.Instance.RemoveTileByPosition(tile.transform.position.ToVector3Int());
                }
            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
            });
    }

}
