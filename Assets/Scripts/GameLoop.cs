using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoop : MonoBehaviour
{
    public delegate void Loop();
    public static event Loop loopEvent;

    public float currentTime = 0.0f;
    public float loopTime = 5.0f;
    void Start()
    {
        
    }

    private void Update()
    {
        
    }

    void gameLoop()
    {
        loopEvent?.Invoke();
    }
}
