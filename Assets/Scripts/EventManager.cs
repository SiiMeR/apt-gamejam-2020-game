using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using DTO;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public GameObject eventPrefab;

    public EventDTO[] events = new []
    {
        new EventDTO("Event 1"), 
        new EventDTO("Event 222"), 
    };
    
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

    private void InsertEvents()
    {
        foreach (var eventDto in this.events)
        {
            var eventObject = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            eventObject.transform.SetParent(this.transform);
            eventObject.transform.localScale = new Vector3(1, 1, 1);
            var eventScript = eventObject.GetComponent<Event>();
            eventScript.EventDto = eventDto;
        }
    }

    private void UpdateEvents()
    {
        PurgeEvents();
        InsertEvents();
    }
    
    public void SetEvents(EventDTO[] eventsDtos)
    {
        this.events = eventsDtos;
        UpdateEvents();
    }
}
