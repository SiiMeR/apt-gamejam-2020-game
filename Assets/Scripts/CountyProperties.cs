using System.Collections;
using System.Collections.Generic;
using DTO;
using TMPro;
using UnityEngine;

public class CountyProperties : Singleton<CountyProperties>
{
    public TextMeshProUGUI countyNameMesh;
    public TextMeshProUGUI populationMesh;
    public TextMeshProUGUI wellnessMesh;
    public TextMeshProUGUI foodMesh;
    public TextMeshProUGUI woodMesh;
    public TextMeshProUGUI coalMesh;

    public int population = 420;
    public int wellness = 69;
    public int food = 1337;
    public int wood = 2000;
    public int coal = 101; 
    private void Awake()
    {
        SetInitialTexts();
        DayChangeEvent.dayChangeEvent += OnDayEvent;
    }
    
    private void SetInitialTexts()
    {
        populationMesh.text = $"Elanike arv: {population}";
        wellnessMesh.text = $"Heaolu: {wellness}%";
        foodMesh.text = $"Söök: {food}";
        woodMesh.text = $"Puit: {wood}";
        coalMesh.text = $"Süsi: {coal}";
    }
    
    private void OnDayEvent(int currentDay)
    {
        // TODO: Kas küla nime saab kasutaja ise määrata? Kui jah, siis lugeda see kuskilt sisse.
        countyNameMesh.text = $"Vald 69 | Päev: {currentDay}";
        if (currentDay % 14 == 0)
        {
            // do smth
            // return;
        }
        if (currentDay % 7 == 0)
        {
            if (currentDay < 36)
            {
                // EventManager.Instance.AddEvent(EventManager.CreateEventDto($"Event {currentDay}"));   
            }
        }
        if (currentDay % 3 == 0)
        {
            if (currentDay < 13)
            {
                EventManager.Instance.AddEvent(new EventDTO($"Event {currentDay}", 
                    "tekst mida näeb modalis", 
                    "nupp mida näeb modalis", 
                    "nupp mida näeb modalis", 
                    () => Debug.Log("Nõustun ja muudan mingeid parameetreid"),
                    () => Debug.Log("Keeldun ja muudan mingeid parameetreid"),
                    new Vector3Int(0, 0, 0)));   
            }
        }
    }
    
}
