using System;
using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestionAsker : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    private Queue<EventDTO> _questions;
    // Start is called before the first frame update
    void Start()
    {
        _questions = new Queue<EventDTO>();
        // AddDto(
        //     "Tahetakse allkirjastada leping, millega keelatakse ära freoonide kasutamine külmkappides, et säästa loodust. Kas oled nõus allkirjastama lepingut?",
        //     (() => ),
        //     (() => )
        //     );
        
        SetQuestion(_questions.Dequeue());
        
    }

    void AddDto(string desc, Action yes, Action no)
    {
        _questions.Enqueue(
            new EventDTO(
                string.Empty,
                "hello",
                "Jah",
                "Ei",
                () => print("d"),
                () => print("x")
                )
            );
    }

    void SetQuestion(EventDTO dto)
    {
        description.text = dto.text;
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() => dto.acceptAction());
        noButton.onClick.AddListener(() => dto.declineAction());
        
        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = dto.acceptText;
        noButton.GetComponentInChildren<TextMeshProUGUI>().text = dto.declineText;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
