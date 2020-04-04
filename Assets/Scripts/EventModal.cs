using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventModal : Singleton<EventModal>
{
    public GameObject nameObject;
    public GameObject descriptionObject;

    public GameObject acceptButton;
    public GameObject declineButton;
    
    private EventDTO _eventDto;
    
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void SetEventDTO(EventDTO eventDto)
    {
        this._eventDto = eventDto;
        SetNameText(this._eventDto?.name);
        SetDescriptionText(this._eventDto?.text);
        SetAcceptText(this._eventDto?.acceptText);
        SetAcceptListener(this._eventDto?.acceptAction);
        SetDeclineText(this._eventDto?.declineText);
        SetDeclineListener(this._eventDto?.declineAction);
    }

    public EventDTO GetEventDTO()
    {
        return this._eventDto;
    }
    
    private void SetNameText(string text)
    {
        this.nameObject.GetComponent<TextMeshProUGUI>()?.SetText(text);
    }

    private void SetDescriptionText(string text)
    {
        this.descriptionObject.GetComponent<TextMeshProUGUI>()?.SetText(text);
    }

    private void SetAcceptListener(Action call)
    {
        var listeners = this.acceptButton.GetComponent<Button>()?.onClick;
        listeners?.RemoveAllListeners();
        listeners?.AddListener(() =>
        {
            call();
            this.gameObject.SetActive(false);
            EventManager.Instance.RemoveEvent(this._eventDto);
        });
    }

    private void SetAcceptText(string text)
    {
        var textMesh = this.acceptButton.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
        textMesh?.SetText(text);
    }
    
    private void SetDeclineListener(Action call)
    {
        var listeners = this.declineButton.GetComponent<Button>()?.onClick;
        listeners?.RemoveAllListeners();
        listeners?.AddListener(() =>
        {
            call();
            this.gameObject.SetActive(false);
            EventManager.Instance.RemoveEvent(this._eventDto);
        });
    }

    private void SetDeclineText(string text)
    {
        var textMesh = this.declineButton.transform.Find("Text")?.GetComponent<TextMeshProUGUI>();
        textMesh?.SetText(text);
    }
}
