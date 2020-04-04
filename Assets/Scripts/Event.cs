using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Event : MonoBehaviour
{
    public EventDTO EventDto;
    
    // Start is called before the first frame update
    void Start()
    {
        var questionObject = this.transform.Find("Question");
        var questionTextMeshProComponent = questionObject.GetComponent<TextMeshProUGUI>();
        questionTextMeshProComponent.SetText(EventDto.name);
        
        // var questionButtonComponent = questionObject.GetComponent<Button>();
        // questionButtonComponent.onClick.AddListener(this.OpenEventModel);

        var buttonComponent = this.GetComponent<Button>();
        buttonComponent.onClick.AddListener(this.OpenEventModal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenEventModal()
    {
        Debug.Log(EventDto.name);
    }
}
