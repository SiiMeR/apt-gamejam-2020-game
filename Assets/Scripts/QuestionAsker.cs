using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using DTO;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class QuestionAsker : MonoBehaviour
{
    public Button yesButton;
    public Button noButton;

    public TextMeshProUGUI description;

    private Queue<EventDTO> _questions;
    // Start is called before the first frame update
    void Start()
    {
        _questions = new Queue<EventDTO>();
        AddDto(
            "Kas oled nõus väitega, et inimeste heaolu on alati tähtsam kui looduse hoidmine?",
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent += 10;
                GlobalStartingVariables.StartingWellnessPercent += 30;
                GlobalStartingVariables.StartingWood += 1000;
                GlobalStartingVariables.StartingPopulation += 1000;
                GlobalStartingVariables.StartingFood += 500;
                GlobalStartingVariables.StartingCoal += 500;
            }),
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent -= 5;
                GlobalStartingVariables.StartingWellnessPercent -= 20;
                GlobalStartingVariables.StartingPopulation -= 300;
            })
        );
        AddDto(
            "Kas sinu arvates kivisöe kaevandamine on vajalik?",
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent += 5;
                GlobalStartingVariables.StartingWellnessPercent += 10;
                GlobalStartingVariables.StartingPopulation += 200;
                GlobalStartingVariables.StartingFood -= 10;
                GlobalStartingVariables.StartingCoal += 1000;
            }),
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent -= 5;
                GlobalStartingVariables.StartingWellnessPercent -= 5;
                GlobalStartingVariables.StartingPopulation -= 300;
                GlobalStartingVariables.StartingCoal -= 1000;
            })
        );
        AddDto(
            "Tahetakse allkirjastada leping, millega keelatakse ära freoonide kasutamine külmkappides, et säästa loodust. Kas oled nõus allkirjastama lepingut?",
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent -= 5;
                GlobalStartingVariables.StartingWellnessPercent += 10;
            }),
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent += 10;
                GlobalStartingVariables.StartingWellnessPercent -= 10;
                GlobalStartingVariables.StartingWood -= 100;
            })
            );
        
        AddDto(
            "Riigikogus tahetakse vastu võtta seadus, millega lubatakse korporatsioonidel maha raiuda 80% riigimetsadest. Selle tulemusel suureneks riigieelarve. Kas võtad seaduse vastu?",
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent += 10;
                GlobalStartingVariables.StartingWellnessPercent += 10;
                GlobalStartingVariables.StartingWood = (int) (GlobalStartingVariables.StartingWood * 0.2);
                GlobalStartingVariables.StartingPopulation += 100;
                GlobalStartingVariables.StartingFood += 500;
                GlobalStartingVariables.StartingCoal += 200;
            }),
            (() =>
            {
                GlobalStartingVariables.AirPollutionPercent -= 5;
                GlobalStartingVariables.StartingWellnessPercent -= 10;
                GlobalStartingVariables.StartingPopulation -= 300;
            })
        );


        SetQuestion(_questions.Dequeue());
        
    }

    void AddDto(string desc, Action yes, Action no)
    {
        _questions.Enqueue(
            new EventDTO(
                string.Empty,
                desc,
                "Jah",
                "Ei",
                yes,
                no
                )
            );
    }

    void SetQuestion(EventDTO dto)
    {
        description.text = dto.text;
        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        EventSystem.current.SetSelectedGameObject(null);

        yesButton.onClick.AddListener(() => dto.acceptAction());
        yesButton.onClick.AddListener(DisableInteract);
        yesButton.onClick.AddListener(NextQuestion);

        noButton.onClick.AddListener(() => dto.declineAction());
        noButton.onClick.AddListener(DisableInteract);
        noButton.onClick.AddListener(NextQuestion);

        yesButton.GetComponentInChildren<TextMeshProUGUI>().text = dto.acceptText;
        noButton.GetComponentInChildren<TextMeshProUGUI>().text = dto.declineText;
    }

    void DisableInteract()
    {
        yesButton.interactable = false;
        noButton.interactable = false;
    }
    void NextQuestion()
    {
        if (_questions.Count == 0)
        {
            DOTween.Sequence()
                .Append(GlobalFade.Instance.FadeOut())
                .AppendCallback(() => SceneManager.LoadScene("Scenes/Production"))
                .Append(GlobalFade.Instance.FadeIn())
                .SetUpdate(true);
            return;
        }
        
        yesButton.interactable = true;
        noButton.interactable = true;
        
        SetQuestion(_questions.Dequeue());
    }
    
}
