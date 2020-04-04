using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EventCreator : MonoBehaviour
{

    public Sprite pold;
    
    // TODO: Variant luua list numbridest, ja need randomilt eventile määrata et mis päevadel event tuleb
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += OnDayEvent;
    }

    private void OnDayEvent(int currentDay)
    {
        // TODO: add randomness for event creation
        Debug.Log("fdkoi");
        Debug.Log(currentDay);
        if (currentDay % 4 == 0)
        {
            // do smth
            // return;
            Debug.Log("AddEvent");
            EventManager.Instance.AddEvent(TekstiiliTehas());
        }
    }

    private EventDTO TekstiiliTehas()
    {
        // TODO: get random river tile
        // Add its coordinates to EventDTO
        // get river tiles nearby
        OurTile ourTile = TileManager.Instance.GetRandomTileByType(TileType.GRASS);
        Debug.Log("TekstiiliTehas");
        Debug.Log(ourTile.positionInTilemap);
        
        return new EventDTO(
            "Teksiilitehas", 
            "Ärimees tahab jõele ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                CountyProperties.Instance.SetPopulation((int) (CountyProperties.Instance.population * 1.1));
                // TODO add pollution to nearby river
            },
            () => {
                CountyProperties.Instance.SetPopulation((int) (CountyProperties.Instance.population * 0.9));
                // TODO remove pollution to nearby river
                Tile tile = Instantiate(ourTile.tile);
                tile.sprite = pold;
                Debug.Log("Keela ehitus");
                Debug.Log(ourTile.positionInTilemap);
                TileManager.Instance.fff(ourTile.positionInTilemap, tile);
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
               // CountyProperties.Instance.wellness += 5;
            },
            () =>
            {
               // CountyProperties.Instance.wellness -= 5;
            },
            tile.positionInTilemap);
    }
    
}
