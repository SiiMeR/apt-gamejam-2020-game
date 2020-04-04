using System.Collections.Generic;
using DTO;
using UnityEditor;
using UnityEngine;

public class EventManager : Singleton<EventManager>
{
    public GameObject eventPrefab;
    public GameObject eventModal;

    public List<EventDTO> events = new List<EventDTO>{
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
            if (child.name == "Event")
            {
                Destroy(child.gameObject);
            }
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

    public void RemoveEvent(EventDTO eventDto, bool invokeDeclineAction)
    {
        if (eventToGameObject.ContainsKey(eventDto))
        {
            if (invokeDeclineAction)
            {
                eventDto.declineAction?.Invoke();   
            }
            Destroy(eventToGameObject[eventDto]);
            eventToGameObject.Remove(eventDto);
        }
    }
    
    private void InsertEvent(EventDTO eventDto)
    {
        if (!eventToGameObject.ContainsKey(eventDto))
        {
            var eventObject = Instantiate(eventPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            eventObject.transform.SetParent(this.transform);
            eventObject.transform.localScale = new Vector3(1, 1, 1);
            var eventScript = eventObject.GetComponent<Event>();
            eventScript.EventDto = eventDto;
            eventScript.SetOnClickListener(this.OpenEventModal);
            eventToGameObject.Add(eventDto, eventObject);
        }
    }

    private void OpenEventModal(EventDTO dto)
    {
        var eventModelScript = this.eventModal.GetComponent<EventModal>();
        eventModelScript.SetEventDTO(dto);
        this.eventModal.SetActive(true);
    }

    private void CloseEventModal()
    {
        var eventModelScript = this.eventModal.GetComponent<EventModal>();
        eventModelScript.SetEventDTO(null);
        this.eventModal.SetActive(false);
    }
}
