using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;

public class Event : MonoBehaviour
{
    public EventDTO EventDto;
    
    // Start is called before the first frame update
    void Start()
    {
        var questionObject = this.transform.Find("Question");
        var questionTextMeshProComponent = questionObject.GetComponent<TextMeshProUGUI>();
        questionTextMeshProComponent.SetText(EventDto.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
