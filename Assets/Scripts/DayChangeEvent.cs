using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayChangeEvent : MonoBehaviour
{
    public delegate void Loop();
    public static event Loop loopEvent;

    public float currentTime = 0.0f;
    public float loopTime = 5.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
