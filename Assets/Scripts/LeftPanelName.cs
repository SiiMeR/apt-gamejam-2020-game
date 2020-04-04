using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using TMPro;
using UnityEngine;

public class LeftPanelName : Singleton<LeftPanelName>
{
    // Start is called before the first frame update

    private string text = "";
    private int day = 1;
    private TextMeshProUGUI _textField;

    void Start()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        GameLoop.loopEvent += updateDay;
        _textField.text = $"Küla 69 | {day}";
        
    }

    void updateDay()
    {
        text = $"Küla 69 | {++day}";
    }
}
