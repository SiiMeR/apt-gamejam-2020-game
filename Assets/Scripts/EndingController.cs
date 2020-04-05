using System.Collections;
using System.Collections.Generic;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
        
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif

    }

    public void SetEndText(string text)
    {
        endMesh.text = text;
        

    }
}
