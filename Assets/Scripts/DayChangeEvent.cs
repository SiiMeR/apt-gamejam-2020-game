using UnityEngine;

public class DayChangeEvent : MonoBehaviour
{
    public delegate void DayChange(int currentDay);
    public static event DayChange dayChangeEvent;

    private int _currentDay = 0;
    private float _currentTime = 0.0f;
    public float loopTime = 5.0f;

    void Start()
    {
        dayChangeEvent?.Invoke(++_currentDay);
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime += Time.deltaTime;
        if (_currentTime >= loopTime)
        {
            dayChangeEvent?.Invoke(++_currentDay);
            _currentTime = 0.0f;
        }
    }
}
