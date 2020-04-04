using TMPro;

public class LeftPanelName : Singleton<LeftPanelName>
{
    private TextMeshProUGUI _textField;

    private void Awake()
    {
        _textField = GetComponent<TextMeshProUGUI>();
        DayChangeEvent.dayChangeEvent += updateDay;
    }

    void updateDay(int currentDay)
    {
        // TODO: Kas küla nime saab kasutaja ise määrata? Kui jah, siis lugeda see kuskilt sisse.
        _textField.text = $"Küla 69 | Päev: {currentDay}";
    }
}
