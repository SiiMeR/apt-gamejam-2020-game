using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndingController : Singleton<EndingController>
{
    public GameObject ending;

    public TextMeshProUGUI endMesh;
    // Start is called before the first frame update

    public void end()
    {
        ending.SetActive(false);
        Application.Quit();
    }

    public void SetEndText(string text)
    {
        endMesh.text = text;
    }
}
