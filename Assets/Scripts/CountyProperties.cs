using System;
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

    public int population { get; private set; } = 50;
    public int wellness { get; private set; } = 100;
    public int food { get; private set; } = 5000;
    public int wood { get; private set; } = 2000;
    public int coal { get; private set; } = 100;

    private int foodUse = 1;
    private float[] foodUses = {0.5f,0.67f,1f,1.33f,2f};
    
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
            
        }
        if (currentDay % 7 == 0)
        {
            // if (currentDay < 36) EventManager.Instance.AddEvent(EventManager.CreateEventDto($"Event {currentDay}"));
        }

        if (currentDay % 1 == 0)
        {
            DoEating();
            DoBreeding();
        }
    }

    private void DoEating()
    {
        int foodDelta = -(int)(population*foodUses[foodUse]);
        int wellnessDelta = -(int)(1f - foodUses[foodUse]);

        if (food + foodDelta < 0)
        {
            int humanDelta = 0;
            int hungryPopulation = (int) (Math.Abs(food + foodDelta) / foodUses[foodUse]);
            for (int i = 0; i < hungryPopulation; i++)
            {
                if (UnityEngine.Random.Range(1, 20 + 1) == 1) humanDelta--;
            }
            SetPopulation(population + humanDelta);
            foodDelta = -food;
        }
        SetFood(food + foodDelta);
        SetWellness(Math.Max(Math.Min(wellness + wellnessDelta,100),0));
    }

    private void DoBreeding()
    {
        int humanDelta = 0;
        for (int i = 0; i < (int) (0.4 * population / 2); i++)
        {
            if (UnityEngine.Random.Range(1, 1000 + 1) + wellness/3 >= 999) humanDelta++;
        }
        SetPopulation(population + humanDelta);
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
