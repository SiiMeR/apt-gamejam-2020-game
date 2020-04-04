﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using UnityEngine;

public class EventCreator : MonoBehaviour
{
    // TODO: Variant luua list numbridest, ja need randomilt eventile määrata et mis päevadel event tuleb
    private void Awake()
    {
        DayChangeEvent.dayChangeEvent += OnDayEvent;
    }

    private void OnDayEvent(int currentDay)
    {
        // TODO: add randomness for event creation
        if (currentDay % 4 == 0)
        {
            // do smth
            // return;
        }
    }

    private EventDTO TekstiiliTehas()
    {
        // TODO: get random river tile
        // Add its coordinates to EventDTO
        // get river tiles nearby
        OurTile tile = TileManager.Instance.GetRandomTileByType(TileType.RIVER);
        Debug.Log(tile.positionInTilemap);
        
        return new EventDTO(
            "Teksiilitehas", 
            "Ärimees tahab jõele ehitada tekstiilitehast. Kuidas toimid?",
            "Luba ehitus",
            "Keela ehitus",
            () =>
            {
                CountyProperties.Instance.population += (int) (CountyProperties.Instance.population * 0.1);
                // TODO add pollution to nearby river
            },
            () => {
                CountyProperties.Instance.population -= (int) (CountyProperties.Instance.population * 0.1);
                // TODO remove pollution to nearby river
            },
            tile.positionInTilemap);
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
                CountyProperties.Instance.wellness += 5;
            },
            () =>
            {
                CountyProperties.Instance.wellness -= 5;
            },
            tile.positionInTilemap);
    }
    
}
