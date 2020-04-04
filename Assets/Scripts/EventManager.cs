using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DTO;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : Singleton<EventManager>
{
    public GameObject eventPrefab;

    public List<EventDTO> events = new List<EventDTO>{
        new EventDTO("Event 1"), 
        new EventDTO("Event 222")
    };
    
    private Dictionary<EventDTO, GameObject> eventToGameObject = new Dictionary<EventDTO, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateEvents();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void PurgeEvents()
    {
        foreach (Transform child in this.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void AddEvents()
    {
        foreach (var eventDto in this.events)
        {
            InsertEvent(eventDto);
        }
    }

    private void UpdateEvents()
    {
        PurgeEvents();
        AddEvents();
    }
    
    public void SetEvents(List<EventDTO> eventsDtos)
    {
        this.events = eventsDtos;
        UpdateEvents();
    }

    public void AddEvent(EventDTO eventDto)
    {
        InsertEvent(eventDto);
    }

    public void RemoveEvent(EventDTO eventDto)
    {
        if (eventToGameObject[eventDto])
        {
            Destroy(eventToGameObject[eventDto]);
        }
    }

    public static EventDTO CreateEventDto(string name)
    {
        return new EventDTO(name);
    }

    private void InsertEvent(EventDTO eventDto)
    {
        var eventObject = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        eventObject.transform.SetParent(this.transform);
        eventObject.transform.localScale = new Vector3(1, 1, 1);
        var eventScript = eventObject.GetComponent<Event>();
        eventScript.EventDto = eventDto;
        eventToGameObject.Add(eventDto, eventObject);
    }
}
