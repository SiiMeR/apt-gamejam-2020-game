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

    public int population { get; private set; } = 420;
    public int wellness { get; private set; } = 69;
    public int food { get; private set; } = 1337;
    public int wood { get; private set; } = 2000;
    public int coal { get; private set; } = 101; 
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
    }

    public void SetPopulation(int value)
    {
        this.population = value;
        populationMesh.text = $"Elanike arv: {population}";
    }
    
    public void SetWellness(int value)
    {
        this.wellness = value;
        wellnessMesh.text = $"Heaolu: {wellness}%";
    }
    
    public void SetFood(int value)
    {
        this.food = value;
        foodMesh.text = $"Söök: {food}";
    }
    
    public void SetWood(int value)
    {
        this.wood = value;
        woodMesh.text = $"Puit: {wood}";
    }
    
    public void SetCoal(int value)
    {
        this.coal = value;
        coalMesh.text = $"Süsi: {coal}";
    }
    
}
