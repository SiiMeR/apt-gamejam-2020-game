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
        AddEventToList(MetsaIstutamine(), 1);
        AddEventToList(ViljaKoristus(), 1);
        AddEventToList(ViljaIstutamine(), 1);
        AddEventToList(Vaetis(), 1);
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
        if (IsPopulationTooLow())
        {
            PopulationTooLow();
            return;
        }

        if (IsAirPollutionTooHigh())
        {
            AirPollutionTooHigh();
            return;
        }

        if (IsDeforestationTooHigh())
        {
            DeforestationTooHigh();
            return;
        }

        if (IsWellnessTooLow())
        {
            WellnessTooLow();
        }

        if (CountyProperties.Instance.food < 100)
        {
            EventDTO va = Ikaldus()();
            EventManager.Instance.AddEvent(va);
            return;
        }
        
        if (CountyProperties.Instance.food < 250)
        {
            EventDTO va = ViljaKoristus()();
            EventManager.Instance.AddEvent(va);
            return;
        }
        
        if (CountyProperties.Instance.food < 1000)
        {
            EventDTO va = ViljaIstutamine()();
            EventManager.Instance.AddEvent(va);
            return;
        }
        
        if (currentDay == 1)
        {
            EventDTO vallavanem = OledVallavanem();
            vallavanem.expires = false;
            EventManager.Instance.AddEvent(vallavanem);
            return;
        }
        if (isVallavanemAnswered && currentDay % 3 == 0)
        {
            int randomEvent = UnityEngine.Random.Range(1, events.Count);
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
                CountyProperties.Instance.SetWellness((int)(CountyProperties.Instance.wellness * 1.05));
                foreach (var tile in OledVallavanemTiles())
                {
                    tile.groundPollution += 0.05f;
                }
                isVallavanemAnswered = true;
            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.95));
                CountyProperties.Instance.SetWellness((int)(CountyProperties.Instance.wellness * 0.95));
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
    
    
    private Func<EventDTO> ViljaKoristus()
    {
        return () => new EventDTO(
            "Viljakoristus", 
            "Külaelanikud tahavad vilja koristada. Kas lubad?",
            "Jah",
            "Ei",
            () =>
            {
                AbstractTile tile = TileManager.Instance.GetRandomTileByType(TileType.FARMLAND);
                CountyProperties.Instance.SetFood(CountyProperties.Instance.food + 250);
                if (tile != null)
                {
                    TileManager.Instance.RemoveTileByPosition(tile.transform.position.ToVector3Int());
                }

            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
                CountyProperties.Instance.SetFood(CountyProperties.Instance.food - 200); 
            });
    }   
    
    private Func<EventDTO> ViljaIstutamine()
    {
        return () => new EventDTO(
            "Vilja külvamine", 
            "Külaelanikud tahavad vilja juurde külvata. Kas lubad?",
            "Jah",
            "Ei",
            () =>
            {
                AbstractTile tile = TileManager.Instance.GetRandomTileByType(TileType.GRASS);
                // CountyProperties.Instance.SetFood(CountyProperties.Instance.food + 50);
                if (tile != null)
                {
                    
                    TileManager.Instance.AddTileByPos(tile.transform.position.ToVector3Int(), TileType.FARMLAND);
                    CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness + 5); 

                    // TileManager.Instance.RemoveTileByPosition(tile.transform.position.ToVector3Int());
                }

            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
            });
    }    
    
    private Func<EventDTO> MetsaIstutamine()
    {
        return () => new EventDTO(
            "Metsa istutamine", 
            "Külaelanikud tahavad metsa juurde istutada. Kas lubad?",
            "Jah",
            "Ei",
            () =>
            {
                AbstractTile tile = TileManager.Instance.GetRandomTileByType(TileType.GRASS);
                // CountyProperties.Instance.SetFood(CountyProperties.Instance.food + 50);
                if (tile != null)
                {
                    TileManager.Instance.AddTileByPos(tile.transform.position.ToVector3Int(), TileType.FOREST);
                    CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness + 5);
                    CountyProperties.Instance.SetGlobalAirPoll(CountyProperties.Instance.globalAirpoll -2);
                    TileManager.Instance.amountOfForests++;

                    // TileManager.Instance.RemoveTileByPosition(tile.transform.position.ToVector3Int());
                }

            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
            });
    }
    
    private Func<EventDTO> TekstiiliTehas()
    {
        // if (true)
        // {
        //     return () => null;
        // }
        return () => new EventDTO(
            "Tekstiilitehas", 
            "Ärimees tahab jõe kõrvale ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                Debug.Log("accept tekstiil");
                var grassTile = TileManager.Instance.UpdateRandomTileSidingWithGrassByType(SpriteFactory.Instance.factory, TileType.RIVER);
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 1.1));
                CountyProperties.Instance.SetGlobalAirPoll(CountyProperties.Instance.globalAirpoll + 3);
                if (grassTile != null && grassTile.GetComponent<SpriteRenderer>() != null)
                {
                    grassTile.GetComponent<Animator>().enabled = false;  
                }

                var surroundingTileLayers = TileManager.Instance.getTilesInRadius(grassTile, 1);
                foreach (List<GameObject> layers in surroundingTileLayers.Values.ToList())
                {
                    foreach (var tile in layers)
                    {
                        // Surrounding tiles pollution
                        var abstractTile = tile.GetComponent<AbstractTile>();
                        abstractTile.groundPollution += 0.2f;
                    }                
                }
                // Factory tile pollution
                var component = grassTile.GetComponent<AbstractTile>();
                component.groundPollution += 0.25f;

            },
            () => {
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.population * 0.9));
                CountyProperties.Instance.SetPopulation((int)(CountyProperties.Instance.wellness * 0.95));
            });
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
                CountyProperties.Instance.SetWood(CountyProperties.Instance.wood + 50);
                if (tile != null)
                {
                    TileManager.Instance.RemoveTileByPosition(tile.transform.position.ToVector3Int());
                }

                TileManager.Instance.amountOfForests -= 1;
            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
            });
    }

    private Func<EventDTO> Vaetis()
    {
        return () => new EventDTO(
            "Uus väetis", 
            "Kaval teadlane leiutas uue väetise, mida ta korralikult kontrollida pole jõudnud, aga mis annaks külale palju suurema saagi ja heaolu",
            "Kasuta uut väetist",
            "Keela väetise kasutamine",
            () =>
            {
                AbstractTile riverTile = TileManager.Instance.GetRandomTileByType(TileType.RIVER);
                var surroundingTileLayers = TileManager.Instance.getTilesInRadius(riverTile, 2, true);
                foreach (List<GameObject> layers in surroundingTileLayers.Values.ToList())
                {
                    foreach (var tile in layers)
                    {
                        tile.GetComponent<AbstractTile>().groundPollution += 0.2f;
                    }                
                }
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness + 10); 
                CountyProperties.Instance.SetFood(CountyProperties.Instance.food + 1000); 
            },
            () =>
            {
                CountyProperties.Instance.SetWellness(CountyProperties.Instance.wellness - 10); 
                CountyProperties.Instance.SetFood(CountyProperties.Instance.food - 300); 
            });
    }

    private bool IsPopulationTooLow()
    {
        return CountyProperties.Instance.population < 100;
    }

    private void PopulationTooLow()
    {
        EndModal("Sinu valla populatsioon langes liiga madalale.");
    }
    
    private bool IsAirPollutionTooHigh()
    {
        return CountyProperties.Instance.globalAirpoll > 75;
    }

    private void AirPollutionTooHigh()
    {
        EndModal("Sinu valla õhusaastatus tõusis liiga kõrgele");
    }
    
    private bool IsDeforestationTooHigh()
    {
        return TileManager.Instance.amountOfForests < 50;
    }

    private void DeforestationTooHigh()
    {
        EndModal("Metsastus langes liiga madalale");
    }

    private bool IsWellnessTooLow()
    {
        return CountyProperties.Instance.wellness < 15;
    }
    
    private void WellnessTooLow()
    {
        EndModal("Heaolu vallas langes liiga madalale");
    }

    private void EndModal(string text)
    {
        Time.timeScale = 0.0f;
        EndingController.Instance.SetEndText(text);
        EndingController.Instance.ending.SetActive(true);
    }
}
