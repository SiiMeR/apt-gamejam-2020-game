﻿using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    public EventDTO EventDto;

    private GameObject dialog;
    
    public Slider slider;
    private bool exists = true;
    
    // Start is called before the first frame update
    void Start()
    {
        var questionObject = this.transform.Find("Question");
        var questionTextMeshProComponent = questionObject.GetComponent<TextMeshProUGUI>();
        questionTextMeshProComponent.SetText(EventDto.name);
        
        // var questionButtonComponent = questionObject.GetComponent<Button>();
        // questionButtonComponent.onClick.AddListener(this.OpenEventModel);

        slider.value = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        if (EventDto != null && EventDto.expires && slider.value <= 100f)
        {
            slider.value -= 10f * Time.deltaTime;
        }
        if (slider.value <= 0f && exists)
        {
            exists = false;
            if (EventModal.Instance.GetEventDTO() == this.EventDto)
            {
                EventModal.Instance.SetEventDTO(null);
                EventModal.Instance.gameObject.SetActive(false);
            }
            EventManager.Instance.RemoveEvent(EventDto, true);
        }
    }

    public delegate void OnClickHandler(EventDTO eventDto);

    public void SetOnClickListener(OnClickHandler handler)
    {
        var buttonComponent = this.GetComponent<Button>();
        buttonComponent.onClick.RemoveAllListeners();
        buttonComponent.onClick.AddListener(() => { handler(this.EventDto); });
    }
}
