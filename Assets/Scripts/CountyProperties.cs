using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountyProperties : MonoBehaviour
{
    public TextMeshProUGUI _countyName;
    public TextMeshProUGUI _population;
    public TextMeshProUGUI _wellness;
    public TextMeshProUGUI _food;
    public TextMeshProUGUI _wood;
    public TextMeshProUGUI _coal;

    private int population = 420;
    private int wellness = 69;
    private int food = 1337;
    private int wood = 2000;
    private int coal = 101; 
    void Awake()
    {
        setInitialTexts();
        DayChangeEvent.dayChangeEvent += onDayEvent;
    }
    
    private void setInitialTexts()
    {
        _population.text = $"Elanike arv: {population}";
        _wellness.text = $"Heaolu: {wellness}%";
        _food.text = $"Söök: {food}";
        _wood.text = $"Puit: {wood}";
        _coal.text = $"Süsi: {coal}";
    }
    
    void onDayEvent(int currentDay)
    {
        // TODO: Kas küla nime saab kasutaja ise määrata? Kui jah, siis lugeda see kuskilt sisse.
        _countyName.text = $"Vald 69 | Päev: {currentDay}";
        if (currentDay % 14 == 0)
        {
            // do smth
            return;
        }
        if (currentDay % 7 == 0)
        {
            
        }
    }
    
}
